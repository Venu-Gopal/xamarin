//Employees Home Page
//Creates a static instance of HomePageViewModel that is accessed from anywhere in the app when the HomePage's UI has to updated.

using CMSapp.Controllers;
using CMSapp.Data;
using CMSapp.Models;
using CMSapp.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CMSapp.Pages
{
    public class DisplayApplication
    {
        public string applicationid { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public ApplicationReviewPage AppReviewPage;
        public static HomePageViewModel viewModel;


        //HomePage Constructor
        public HomePage()
        {
            viewModel = new HomePageViewModel();
            AppReviewPage = new ApplicationReviewPage();
			InitializeComponent ();
            BindingContext = viewModel;
            ApplicationViewStackLayout.SetBinding(IsVisibleProperty, nameof(viewModel.ApplicationView_IsVisible));
            CabViewStackLayout.SetBinding(IsVisibleProperty, nameof(viewModel.CabViewIsVisible));
            CancelCabWrapper.SetBinding(IsVisibleProperty, nameof(viewModel.CancelCab_IsVisible));
            StartTripWrapper.SetBinding(IsVisibleProperty, nameof(viewModel.IsCabArrived));
            ApplicationReviewWrapper.SetBinding(IsVisibleProperty, nameof(viewModel.ReviewApplications_IsVisible));
			Login login = DataHandler.getLoginData();
            Employee employee = EmployeeController.GetEmployeeAsync(login);
            DataHandler.refreshApplicationData();
            DataHandler.refreshManagerData(employee);


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //To be fixed : unReadApplications does not update dynamically is user is currenly ON the Page. only onAppearing is the value updated. Add unreadapplications as a property to the viewModel to fix.
            ApplicationReviewButton.Text = $"Applications to review ({DataHandler.unreadApplications})";
            DisplayAppListView.ItemsSource = viewModel.DisplayAppList;
            DisplayPassengerListView.ItemsSource = viewModel.DisplayPassengerList;
            DisplayCabListView.ItemsSource = viewModel.DisplayCabList;


            if (viewModel.DisplayAppList.Count == 0) //no application submitted
            {
                EmptyDisplayMessageStack.IsVisible = true;
                VehicleRequestButton.IsVisible = true; //disable employee from submitting more than one application at a time.
            }
            else
            {
                EmptyDisplayMessageStack.IsVisible = false;
                VehicleRequestButton.IsVisible = false;
            }
        }

        //Button Onclick Handlers

        private async void GoToApplicationPage()
        {
            await Navigation.PushAsync(new ApplicationPage());
        }

        private async void GoToApplicationReviewPage()
        {
            await Navigation.PushAsync(AppReviewPage);
        }

        private void SendToSecurityButton_Clicked()
        {
            //Update Application status to SECURITY (to inform manager that application was sent to security directly and so that they can approve it later)
            
            //Send application to security 
        }

        private async void CancelCabButton_Clicked(object sender, EventArgs e)
        {
            //SEND NOTIFICATION TO SECURITY ABOUT CANCELLATION
            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to cancel this application?", "Yes", "No");
            if (confirm)
            {
                int appid = int.Parse(viewModel.DisplayAppList[0].applicationid);
                Cab cab = CabController.GetCab(appid);
                cab.cab_status = "CANCELLED";
                CabController.UpdateCab(cab);
                //Update HomePage UI
                HomePage.viewModel.DisplayCabList[0].cab_status = "CANCELLED";
            }
        }
        private  void StartTripButton_Clicked(object sender, EventArgs e)
        {
            DateTime departuretime = DateTime.Today;
            
            int appid = int.Parse(viewModel.DisplayAppList[0].applicationid);
            Cab cab = CabController.GetCab(appid);
            cab.departure_time = departuretime.ToString(); 
            CabController.UpdateCab(cab);
            HomePage.viewModel.DisplayCabList[0].departure_time = departuretime.ToString(); ;

            viewModel.IsCabArrived = false;
            EndTripButton.IsVisible = true;
        }

        private void EndTripButton_Clicked(object sender, EventArgs e)
        {
            DateTime tripendtime = DateTime.UtcNow;

            int appid = int.Parse(viewModel.DisplayAppList[0].applicationid);
            Cab cab = CabController.GetCab(appid);
            cab.trip_end_time = tripendtime.ToString();
            cab.cab_status = "REACHED";
            CabController.UpdateCab(cab);
            HomePage.viewModel.DisplayCabList[0].trip_end_time = tripendtime.ToString(); ;
        

            DisplayAlert("", "Your trip has ended.", "OK");

            viewModel.ResetHomePageUI_OnCabReached();
            EndTripButton.IsVisible = false;

        }

     

        //Item Tapped Handlers  to disable selected item highlighting 
        private void DisplayAppListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
           ((ListView)sender).SelectedItem = null;

        }

        private void DisplayPassengerListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            ((ListView)sender).SelectedItem = null;
        }

        private void DisplayCabListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            ((ListView)sender).SelectedItem = null;
        }

        
    }
}