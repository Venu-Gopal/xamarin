using CMSapp.Controllers;
using CMSapp.Data;
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
    public partial class ApplicationReviewPage : CarouselPage
    {
        public static ApplicationReviewViewModel viewModel;
        public ApplicationReviewPage()
        {
            //Label Texts on the UI is bound to properties of ReviewAppList defined in the Binding Context View Model. ie; Application Review Model
            viewModel = new ApplicationReviewViewModel();
            BindingContext = new ApplicationReviewViewModel();

            InitializeComponent();
            
           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ItemsSource = viewModel.ReviewAppList;

            if (viewModel.ReviewAppList.Count == 0) //no application submitted
            {
                this.Title = "No applications to review";
            }
            else
            {
                this.Title = "Current Applications";
            }

           
        }
      

        private void ReviewPassengerListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            ((ListView)sender).SelectedItem = null;
        }

        private async void ApproveButton_Clicked(object sender, EventArgs e)
        {
			bool confirm = await DisplayAlert("Confirm", "Are you sure you want to approve this application?", "Yes", "No");
            if (confirm)
            {
               	int applicationid = int.Parse(viewModel.ReviewAppList[Children.IndexOf(CurrentPage)].applicationid);
                //Change Application Status to APPROVED
	            ApplicationController.ApproveApplication(applicationid);
                //Once APPROVED, remove current application from ReviewAppList (list of applications to review that are visible to Manager)
                ApplicationReviewPage.viewModel.ReviewAppList.RemoveAt(Children.IndexOf(CurrentPage));
                if (viewModel.ReviewAppList.Count == 0)
                    HomePage.viewModel.ResetHomePageUI_OnNoApplicationstoReview(); 
                await Navigation.PopAsync();
            }            
        }

        private async void RejectButton_Clicked(object sender, EventArgs e)
        {
            //Comments are REQUIRED  to be submitted at rejection
            //To Be Done : Implement content validation of Comments Content to ensure users aren't sending null/whitespace/single character. Check if at least 15 characters are inputted.
			StackLayout commentsStack = CurrentPage.FindByName<StackLayout>("CommentsStack");
            commentsStack.IsVisible = true;
            

            var commentsContent = commentsStack.FindByName<Editor>("CommentsEditor").Text;
            commentsStack.FindByName<Editor>("CommentsEditor").Focus();



            if (string.IsNullOrEmpty(commentsContent) || string.IsNullOrWhiteSpace(commentsContent))
                await DisplayAlert("Comments Required", "Please enter a brief description of your reason for rejecting this application.", "OK");
            else
            {
				bool confirm = await DisplayAlert("Confirm", "Are you sure you want to reject this application?", "Yes", "No");
                if (confirm)
                {
	                int applicationid = int.Parse(viewModel.ReviewAppList[Children.IndexOf(CurrentPage)].applicationid);
	
	                Comment comment = new Comment
	                {
	                    applicationid = applicationid.ToString(),
	                    commenter = DataHandler.getEmployeeData().name,
	                    comment_by = "MANAGER",
	                    comment_time = DateTime.UtcNow,
	                    comment = commentsContent
	                };
                    //Add Comment to Database
                    //To be done : Implement feature for applicant to view comments on their HomePage as to why application was rejected
                    CommentController.AddComment(comment);

                    //Update Application Status to CANCELLED
                    ApplicationController.RejectApplication(applicationid);
                    //Once REJECTED, remove current application from ReviewAppList (list of applications to review that are visible to Manager)
                    viewModel.ReviewAppList.RemoveAt(Children.IndexOf(CurrentPage));
                    if (viewModel.ReviewAppList.Count == 0)
                        HomePage.viewModel.ResetHomePageUI_OnNoApplicationstoReview();
                    Navigation.PopAsync(); 
				}
            }
        }

        
    }
}
