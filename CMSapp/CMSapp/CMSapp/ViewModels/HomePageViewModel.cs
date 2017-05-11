using CMSapp.Data;
using CMSapp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CMSapp.ViewModels
{
    public class HomePageApplication
    {
        public string applicationid { get; set; }
        public string submission_datetime { get; set; }
        public string application_status { get; set; }
        public string cab_required_time { get; set; }
    }

    public class HomePagePassenger
    {
        public string employeeid { get; set; }
        public string in_time { get; set; }
        public string pickup { get; set; }
        public string destination { get; set; }

    }
    public class HomePageCab
    {
        public string cab_no { get; set; }
        public string booking_time { get; set; }
        public string expected_arrival_time { get; set; }
        public string arrival_time { get; set; }
        public string departure_time { get; set; }
        public string trip_end_time { get; set; }
        public string driver_name { get; set; }
        public string driver_contact_no { get; set; }
        public int applicationid { get; set; } //REQUIRED
        public string cab_status { get; set; }
    }

    public class HomePageViewModel : INotifyPropertyChanged
    {

        private bool _cabviewisvisible;

        //This Property sets the Visibility of the Cab View on the Employees HomePage. This property should be updated when HomePage UI needs to show Cab Details.
        public bool CabViewIsVisible
        {
            get
            {
                return _cabviewisvisible;
            }
            set
            {
                _cabviewisvisible = value;
                OnPropertyChanged("CabViewIsVisible");
            }
        }

        private bool _applicationview_isvisible;
        //This Property sets the Visibility of the Application View on the Employees HomePage. This property should be updated when HomePage UI needs to show Application Details.
        public bool ApplicationView_IsVisible
        {
            get
            {
                return _applicationview_isvisible;
            }
            set
            {
                _applicationview_isvisible = value;
                OnPropertyChanged("ApplicationView_IsVisible");
            }
        }

        private bool _iscabarrived; 
       
        //This property sets the visibilty of the Start Trip button on the HomePage. Should only be visible when Cab has arrived.
        public bool IsCabArrived
        {
            get
            {
                return _iscabarrived;
            }
            set
            {
                _iscabarrived = value;
                OnPropertyChanged("IsCabArrived");
            }
        }

        private bool _cancelcab_isvisible;
        //This property sets the visibilty of the Cancel Cab button on the HomePage. Should only be visible when Cab has been booked but not yet arrived.
        public bool CancelCab_IsVisible
        {
            get
            {
                return _cancelcab_isvisible;
            }
            set
            {
                _cancelcab_isvisible = value;
                OnPropertyChanged("CancelCab_IsVisible");
            }
        }

        private bool _reviewapplications_isvisible;
        //This property sets the visibilty of the Applications to Review Button button on the Manager's HomePage. Should only be visible when  device has received an application to review.
        // ie. Only managers see this button on receiving an application. Employees should not be able to view it.
        public bool ReviewApplications_IsVisible
        {
            get
            {
                return _reviewapplications_isvisible;
            }
            set
            {
                _reviewapplications_isvisible = value;
                OnPropertyChanged("ReviewApplications_IsVisible");
            }
        }




        //List containing the application information that is displayed on the HomePage.
        //Contains the details of the application that has been submitted by user.
        //Should at all times contain only ONE element ie the currently submitted application. User is disabled from submitting more than one application at a time.
        private ObservableCollection<HomePageApplication> _displayapplist;
        public ObservableCollection<HomePageApplication> DisplayAppList
        {
            get
            {
                return _displayapplist;
            }
            set
            {
                _displayapplist = value;
                OnPropertyChanged("DisplayAppList");
            }
        }
        //List containing the Passeenger information from recently submitted application, that is displayed on the HomePage.
        private ObservableCollection<HomePagePassenger> _displaypassengerlist;
        public ObservableCollection<HomePagePassenger> DisplayPassengerList
        {
            get
            {
                return _displaypassengerlist;
            }
            set
            {
                _displaypassengerlist = value;
                OnPropertyChanged("DisplayPassengerList");
            }
        }


        private ObservableCollection<HomePageCab> _displaycablist;
        public ObservableCollection<HomePageCab> DisplayCabList
        {
            get
            {
                return _displaycablist;
            }
            set
            {
                _displaycablist = value;
                OnPropertyChanged("DisplayCabList");
                
            }
        }




        public HomePageViewModel()
        {
            //Default visibilities of HomePage UI elements on construction. Explicity reset these values appropriately if data is refreshed on app launch
            CancelCab_IsVisible = false;
            IsCabArrived = false;
            CabViewIsVisible = false;
            ApplicationView_IsVisible = false; 
            ReviewApplications_IsVisible = false;
            DisplayAppList = new ObservableCollection<HomePageApplication>();
            

            DisplayPassengerList = new ObservableCollection<HomePagePassenger>();
           
            DisplayCabList = new ObservableCollection<HomePageCab>();
           
           
        }

        //Managers View : Call this function to Make Application Review Button visible
        public void UpdateHomePageUI_OnReceivingReviewApplication()
        {
            ReviewApplications_IsVisible = true;
        }
        public void ResetHomePageUI_OnNoApplicationstoReview()
        {
            ReviewApplications_IsVisible = false;
        }

        //Employees View
        //Call this function to reset UI elements to default on Ending of trip
        public void ResetHomePageUI_OnCabReached()
        {
            CancelCab_IsVisible = false;
            IsCabArrived = false;
            CabViewIsVisible = false;
            ApplicationView_IsVisible = false;
            ReviewApplications_IsVisible = false;

            DisplayAppList.Clear();
            DisplayCabList.Clear();
            DisplayPassengerList.Clear();
            DataHandler.resetCurrentApplication();            
        }
        //To Display app view on submission of app
        public void UpdateHomePageUI_OnAppSubmit()
        {
            ApplicationView_IsVisible = true;
            CabViewIsVisible = false;
            IsCabArrived = false;
            CancelCab_IsVisible = false;

        }
       
        //To display cab details on booking of cab. Called on receiving push notification when cab has been booked.
        public void UpdateHomePageUI_OnCabBooked()
        {
            ApplicationView_IsVisible = false;
            CabViewIsVisible = true;
            IsCabArrived = false;
            CancelCab_IsVisible = true;
        }
        //To display appropriate buttons once cab has arrived. Called on receiving push notification from security when cab has arrived.
        public void UpdateHomePageUI_OnCabArrived()
        {
            ApplicationView_IsVisible = false;
            CabViewIsVisible = true;
            IsCabArrived = true;
            CancelCab_IsVisible = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;

      

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}