//Security Specific UI
//This page is displayed once Security consults with the cab company about the feasibilty of the booking adn clicks Approve on SecurityApplicationReviewPage


using CMSapp.Controllers;
using CMSapp.Models;
using RestSharp;
using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CMSapp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FillCabDetailsPage : ContentPage
	{
        public string applicationid;
        public FillCabDetailsPage(string appid)
        {
            InitializeComponent();
            applicationid = appid;
        }

        private async void BookCabButton_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to approve this application?", "Yes", "No");
            if (confirm)
            {
                Cab cab = new Cab
                {
                    cab_no = CabNoEntry.Text,
                    applicationid = int.Parse(applicationid),
                    expected_arrival_time = (DateTime.Today + ExpectedArrivalTimePicker.Time).ToString("o"),
                    driver_name = DriverNameEntry.Text,
                    driver_contact_no = DriverContactEntry.Text,
                    cab_status = "BOOKED",
                    booking_time = DateTime.UtcNow.ToString("s")
                };

                //Send Cab Details here, this cab is being created in database for the first time
                CabController.AddCab(cab);

                //Pop back to Security HomePage
                Navigation.PopAsync();
                Navigation.PopAsync();
            }
        }
    }
}
