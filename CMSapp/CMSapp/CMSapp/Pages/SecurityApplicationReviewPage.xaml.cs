// A new securityApplicationReviewPage is created everytime is list item on Security Home Page is clicked.
//The page receives the corresponding application's id through its constructor.
using CMSapp.Controllers;
using CMSapp.Models;
using CMSapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CMSapp.Pages
{ 
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecurityApplicationReviewPage : ContentPage
	{
        public string applicationid; 
        public SecurityApplicationReviewPage(string appid)
        {
            InitializeComponent();
            applicationid = appid;
            Cab cab = CabController.GetCab(int.Parse(applicationid));
            if(cab == null)
                SecurityCabOptions_Button.Text = "Book Cab";
            else if(cab.cab_status == "BOOKED")
            {
                SecurityCabOptions_Button.Text = "Cab Arrived";
            }
            else SecurityCabOptions_Button.IsEnabled = false;
        }

        private  void SecurityPassengerListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            ((ListView)sender).SelectedItem = null;
        }

        private async void SecurityCabOptions_Button_Clicked(object sender, EventArgs e)
        {
            //get cab and check against cab_status.
            Cab cab = CabController.GetCab(int.Parse(applicationid));

            if (cab == null)
                await Navigation.PushAsync(new NavigationPage(new FillCabDetailsPage(applicationid)));
            else if(cab.cab_status == "ARRIVED")
            {
                await DisplayAlert("Error", "Ride is already in transit", "OK");
            }
            else if(cab.cab_status == "CANCELLED")
            {
                await DisplayAlert("Error", "Cab has been Cancelled", "OK");
            }
            else
            {
                cab.cab_status = "ARRIVED";
                cab.arrival_time = DateTime.UtcNow.ToString();
                CabController.UpdateCab(cab);
                await DisplayAlert("Cab has arrived.", "Employee has been notified.", "OK");
            }


            
        }

        private async void SecurityRejectButton_Clicked(object sender, EventArgs e)
        {
            
            CommentsStack.IsVisible = true;
           

            if (string.IsNullOrEmpty(CommentsEditor.Text) || string.IsNullOrWhiteSpace(CommentsEditor.Text))
                await DisplayAlert("Comments Required", "Please enter a brief description of your reason for rejecting this application.", "OK");
            else
            {

                bool confirm = await DisplayAlert("Confirm", "Are you sure you want to reject this application?", "Yes", "No");
                if (confirm)
                {
                    Comment comment = new Comment
                    {
                        applicationid = applicationid,
                        commenter = "NA",
                        comment_by = "SECURITY",
                        comment_time = DateTime.UtcNow,
                        comment = CommentsEditor.Text
                    };

                    //SEND COMMENTS TO API and Update app_status to REJECTED
                    CommentController.AddComment(comment);

                    Models.Application app = ApplicationController.GetApplication(int.Parse(applicationid));
                }
            }
        }
    }
}
