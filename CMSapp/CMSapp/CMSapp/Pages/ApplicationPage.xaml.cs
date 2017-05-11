using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CMSapp.Models;
using CMSapp.Controllers;
using CMSapp.Data;

namespace CMSapp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplicationPage : ContentPage
    {
            
        public ApplicationPage()
        {
            InitializeComponent();
        }

        //Toggles visibility of each passenger's entry input fields on teh UI based on if your'e travelling solo or not
        public void TravellingSoloToggled(object sender, ToggledEventArgs e)
        {
            PassengerStack1.IsVisible = !e.Value;
            PassengerStack2.IsVisible = !e.Value;
            PassengerStack3.IsVisible = !e.Value;

            //Escort input field visible only if travelling solo, ie travelling solo is Toggled
            EscortStack.IsVisible = e.Value; 
        }

        //If escort is required and toggled, display option to enter escort name
        public void EscortToggled(object sender, ToggledEventArgs e)
        {
            EscortNameEntry.IsVisible = e.Value;
            
        }

        //Not yet implemented : Form Validation. All fields except Escort Name are required.
        //A Single Passenger has the following related information to be entered for them : EmployeeID, Pickup location, Destination location, In-Time
        //User is allowed to take anywhere between 1-3 passengers. If any detail of a passenger is filled, ALL remaining details are REQUIRED. 
        //If NO details are filled DO NOT add that passenger to passengerlist.
        private async void SubmitButton_OnClick(object sender, EventArgs e)
        {
            List<Passenger> passengerlist = new List<Passenger>();
            //Applicant gets added as passenger with related details as well
            passengerlist.Add(new Passenger
            {
                employeeid = Data.DataHandler.getEmployeeData().employeeid,
                in_time = DateTime.Today + PassengerInTimePicker0.Time,
                pickup = PassengerPickupEntry0.Text,
                destination = PassengerDestinationEntry0.Text
            });
            //Currently only EmployeeID field is checked to be not null . If not null, add all related detailes to passenger list.
            //To be done : Add to passengerlist only if ALL details of that passenger are not null.
            if (!string.IsNullOrEmpty(PassengerEmpIDEntry1.Text))
            {
                passengerlist.Add(new Passenger
                {
                    employeeid = PassengerEmpIDEntry1.Text,
                    in_time = DateTime.Today + PassengerInTimePicker1.Time,
                    pickup = PassengerPickupEntry1.Text,
                    destination = PassengerDestinationEntry1.Text
                });

            }
            if (!string.IsNullOrEmpty(PassengerEmpIDEntry2.Text))
            {
                passengerlist.Add(new Passenger
                {
                    employeeid = PassengerEmpIDEntry2.Text,
                    in_time = DateTime.Today + PassengerInTimePicker2.Time,
                    pickup = PassengerPickupEntry2.Text,
                    destination = PassengerDestinationEntry2.Text
                });

            }
            if (!string.IsNullOrEmpty(PassengerEmpIDEntry3.Text))
            {
                passengerlist.Add(new Passenger
                {
                    employeeid = PassengerEmpIDEntry3.Text,
                    in_time = DateTime.Today + PassengerInTimePicker2.Time,
                    pickup = PassengerPickupEntry3.Text,
                    destination = PassengerDestinationEntry3.Text
                });

            }
            //Implement form validation. Do not send null data to server.
            Models.Application application = new Models.Application();
            application.travel_purpose = TravelPurposeEntry.Text;
            application.roundtrip = RoundtripSwitch.IsToggled;
            application.escort_name = EscortNameEntry.Text;
            application.employeeid = DataHandler.getEmployeeData().employeeid;
            application.managerid = ManagerIDEntry.Text;
            application.manager_name = ManagerNameEntry.Text;
            application.application_status = "PENDING";
            application.project_bpcode = ProjectBPCodeEntry.Text;
            application.submission_datetime = DateTime.UtcNow.ToString("s");
            application.cab_required_time = (DateTime.Today + CabRequiredTimePicker.Time).ToString("s");

            int applicationid = await ApplicationController.AddApplicationAsync(application, passengerlist);
            if (applicationid != -1)
            {
                PassengerController.AddPassengerAsync(passengerlist, applicationid);
                DataHandler.AddHomePageApplication(applicationid, application, passengerlist);
                await Navigation.PopAsync();
            }

           

        }
    }
}