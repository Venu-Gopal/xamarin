using CMSapp.Controllers;
using CMSapp.Models;
using CMSapp.Pages;
using CMSapp.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CMSapp.Data
{
    class DataHandler
    {
        public static int unreadApplications = 0;
        
        public static void AddReviewApplication(int applicationid, Application application, List<Passenger> passengers)
        {

            ObservableCollection<ReviewPassenger> newList = new ObservableCollection<ReviewPassenger>();
            foreach (var item in passengers)
            {
                newList.Add(new ReviewPassenger { employeeid = item.employeeid, in_time = Convert.ToDateTime(item.in_time).ToString("s"), pickup = item.pickup, destination = item.destination });
            }

            ApplicationReviewPage.viewModel.ReviewAppList.Add(new ReviewApplication
            {
                applicationid = applicationid.ToString(),
                travel_purpose = application.travel_purpose,
                submission_datetime = Convert.ToDateTime(application.submission_datetime).ToString(),
                roundtrip = application.roundtrip.ToString(),
                project_bpcode = application.project_bpcode,
                application_status = application.application_status.ToString() == "SECURITY" ? "Sent to Security" : application.application_status.ToString(),
                cab_required_time = Convert.ToDateTime(application.cab_required_time).ToShortTimeString(),
                PassengerList = newList
            });
        }

        public static void AddSecurityApplication(int applicationid, Application application, List<Passenger> passengers)
        {
            ObservableCollection<SecurityPassenger> newList = new ObservableCollection<SecurityPassenger>();
            foreach (var item in passengers)
            {
                newList.Add(new SecurityPassenger { employeeid = item.employeeid, in_time = item.in_time.ToString(), pickup = item.pickup, destination = item.destination });
            }

            SecurityHomePage.viewModel.SecurityAppList.Add(new SecurityApplication
            {
                applicationid = applicationid.ToString(),
                application_status = application.application_status,
                cab_required_time = application.cab_required_time,
                roundtrip = application.roundtrip.ToString(),
                submission_datetime = Convert.ToDateTime(application.submission_datetime).ToString("s"),
                SecurityPassengerList = newList
            });
        }

        public static void AddHomePageApplication(int applicationid, Application application, List<Passenger> passengers)
        {
            HomePage.viewModel.DisplayAppList.Clear();
            HomePage.viewModel.DisplayPassengerList.Clear();
            
            foreach (var item in passengers)
            {
                HomePage.viewModel.DisplayPassengerList.Add(new HomePagePassenger { employeeid = item.employeeid, in_time = Convert.ToDateTime(item.in_time).ToString("s"), pickup = item.pickup, destination = item.destination });
            }

            HomePage.viewModel.DisplayAppList.Add(
                new HomePageApplication { applicationid = applicationid.ToString(), application_status = application.application_status, submission_datetime = Convert.ToDateTime(application.submission_datetime).ToString(),
                    cab_required_time = Convert.ToDateTime(application.cab_required_time).ToString() });

            HomePage.viewModel.UpdateHomePageUI_OnAppSubmit();
        }

        public static void UpdateCabDetailsForCurrentApp(Cab cab)
        {
            HomePage.viewModel.DisplayCabList.Add(
                new HomePageCab {
                    
                    cab_no = cab.cab_no,
                    booking_time = cab.booking_time,
                    expected_arrival_time = cab.expected_arrival_time,
                    arrival_time = cab.arrival_time,
                    departure_time = cab.departure_time,
                    trip_end_time = cab.trip_end_time,
                    driver_name = cab.driver_name,
                    driver_contact_no = cab.driver_contact_no,
                    applicationid = cab.applicationid,
                    cab_status = cab.cab_status
                });
        }

        public static void SaveData(Login login)
        {
            Xamarin.Forms.Application.Current.Properties["loginid"] = login.loginid;
            Xamarin.Forms.Application.Current.Properties["password"] = login.password;
        }

        public static void SaveData(Employee employee)
        {
            Xamarin.Forms.Application.Current.Properties["EmployeeData"] = employee;
        }

        public static Login getLoginData()
        {
            if(Xamarin.Forms.Application.Current.Properties.ContainsKey("loginid") && Xamarin.Forms.Application.Current.Properties.ContainsKey("password"))
            {
                return new Login
                {
                    loginid = Xamarin.Forms.Application.Current.Properties["loginid"].ToString(),
                    password = Xamarin.Forms.Application.Current.Properties["password"].ToString()
                };
            }
            return null;
        }

        public static Employee getEmployeeData()
        {
            Employee emp = null;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("loginid") && Xamarin.Forms.Application.Current.Properties.ContainsKey("password"))
            {
                if(Xamarin.Forms.Application.Current.Properties.ContainsKey("EmployeeData"))
                    emp =  (Employee) Xamarin.Forms.Application.Current.Properties["EmployeeData"];
            }
            return emp;
        }

        public static void saveFirebaseToken(string firebaseToken)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "firebase.txt");
            File.WriteAllText(filePath, firebaseToken);

        }

        public static string getFirebaseToken()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "firebase.txt");

            if (File.Exists(filePath))
            {
                string firebaseAuth = File.ReadAllText(filePath);
                return firebaseAuth;
            }
            return null;
        }

        public static void refreshApplicationData()
        {
            Application app = null;
            int applicationid = getCurrentApplication();
            if(applicationid != -1)
                app = ApplicationController.GetApplication(applicationid);
            //Get application and passengers
            if (app != null)
            {
                app.submission_datetime = Convert.ToDateTime(app.submission_datetime).ToString();
                app.cab_required_time = Convert.ToDateTime(app.cab_required_time).ToString();
                List<Passenger> passengerList = PassengerController.GetPassengers(applicationid);
                HomePage.viewModel.DisplayAppList.Clear();
                HomePage.viewModel.DisplayAppList.Add(new HomePageApplication
                {
                    applicationid = applicationid.ToString(),
                    application_status = app.application_status,
                    cab_required_time = app.cab_required_time,
                    submission_datetime = app.submission_datetime
                });
                foreach (Passenger pass in passengerList)
                {
                    HomePage.viewModel.DisplayPassengerList.Add(new HomePagePassenger
                    {
                        employeeid = pass.employeeid,
                        destination = pass.destination,
                        in_time = Convert.ToDateTime(pass.in_time).ToString(),
                        pickup = pass.pickup
                    });
                }
                Cab cab = CabController.GetCab(applicationid);
                if(cab == null)
                {
                    HomePage.viewModel.UpdateHomePageUI_OnAppSubmit();
                }
                else
                {
                    if(cab.cab_status == "ARRIVED")
                    {
                        HomePage.viewModel.UpdateHomePageUI_OnCabArrived();
                    }
                    else
                    {
                        HomePage.viewModel.UpdateHomePageUI_OnCabBooked();
                    }
                }
            }
        }

        public static void refreshManagerData(Employee employee)
        {
            List<int> rAppList = getCurrentReviewApplications();
            if (rAppList != null)
            {
                ApplicationReviewPage.viewModel.ReviewAppList.Clear();
                foreach (int appid in rAppList)
                {
                    Application app = ApplicationController.GetApplication(appid);
                    List<Passenger> passList = PassengerController.GetPassengers(appid);
                    AddReviewApplication(appid, app, passList);
                }
            }
        }

        public static void saveCurrentApplication(int applicationid)
        {
            Xamarin.Forms.Application.Current.Properties["currentApplication"] = applicationid;
        }

        public static int getCurrentApplication()
        {
            if(Xamarin.Forms.Application.Current.Properties.ContainsKey("currentApplication"))
                return (int) Xamarin.Forms.Application.Current.Properties["currentApplication"];
            return -1;
        }

        public static void resetCurrentApplication()
        {
            Xamarin.Forms.Application.Current.Properties.Remove("currentApplication");
        }

        public static void saveCurrentReviewApplications(List<ReviewApplication> appList)
        {
            List<int> listToSave = new List<int>();
            foreach(var rApp in appList)
            {
                listToSave.Add(int.Parse(rApp.applicationid));
            }
            string listToSaveString = JsonConvert.SerializeObject(listToSave);
            Xamarin.Forms.Application.Current.Properties["currentReviewApplications"] = listToSaveString;
        }

        public static List<int> getCurrentReviewApplications()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("currentReviewApplications"))
                return JsonConvert.DeserializeObject<List<int>>((string) Xamarin.Forms.Application.Current.Properties["currentReviewApplications"]);
            return null;
        }
    }
}