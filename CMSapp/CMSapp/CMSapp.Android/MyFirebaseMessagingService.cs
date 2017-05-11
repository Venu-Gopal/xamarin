using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using CMSapp.Controllers;
using System.Diagnostics;
using Xamarin.Forms;
using CMSapp.Pages;
using CMSapp;
using CMSapp.Droid;
using CMSapp.Data;
using System.Collections.Generic;
using CMSapp.Models;
using System.Linq;

namespace CMSApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            int applicationId = -1;
            string header = "";
            if(message.Data.Keys.Contains("applicationid"))
            {
                foreach(var item in message.Data.Values)
                {
                    try
                    {
                        applicationId = int.Parse(item);
                    }
                    catch( Exception e)
                    {
                        header = item;
                    }
                }
                CMSapp.Models.Application app = ApplicationController.GetApplication(applicationId);
                List<Passenger> pass = PassengerController.GetPassengers(applicationId);

                if (header == "SECURITY")
                {
                    Console.WriteLine("Got application for security to book cab for");
                    DataHandler.AddSecurityApplication(applicationId, app, pass);
                    //Security calls company and books cab
                    //Enters cab details and presses submit
                }
                else if (header == "MANAGER")
                {
                    Console.WriteLine("Got application for manager to review");
                    //Update unread application count
                    DataHandler.unreadApplications++;
                    if (Xamarin.Forms.Application.Current.Properties.ContainsKey(Constants.unreadApplications))
                    {
                        Xamarin.Forms.Application.Current.Properties[Constants.unreadApplications] = DataHandler.unreadApplications;
                    }
                    SendNotification("You have " + DataHandler.unreadApplications + " new application(s) to review.");

                    //Update the reviewpassengers ui
                    DataHandler.AddReviewApplication(applicationId, app, pass);
                    HomePage.viewModel.UpdateHomePageUI_OnReceivingReviewApplication();
                    DataHandler.saveCurrentReviewApplications(ApplicationReviewPage.viewModel.ReviewAppList.ToList());
                }
                else if (header == "EMPLOYEE")
                {
                    Console.WriteLine("Application approved by manager");
                    CMSapp.Models.Application application = ApplicationController.GetApplication(applicationId);

                    HomePage.viewModel.DisplayAppList.Clear();
                    HomePage.viewModel.DisplayAppList.Add(new CMSapp.ViewModels.HomePageApplication
                    {
                        applicationid = applicationId.ToString(),
                        application_status = application.application_status,
                        cab_required_time = application.cab_required_time,
                        submission_datetime = Convert.ToDateTime(application.submission_datetime).ToString()
                    });
                }
                else if (header == "CAB_BOOKED")
                {
                    Cab cab = CabController.GetCab(applicationId);
                    //Update cab details
                    HomePage.viewModel.DisplayCabList.Clear();
                    HomePage.viewModel.DisplayCabList.Add(new CMSapp.ViewModels.HomePageCab
                    {
                        applicationid = cab.applicationid,
                        expected_arrival_time = cab.expected_arrival_time,
                        booking_time = cab.booking_time,
                        cab_no = cab.cab_no,
                        cab_status = cab.cab_status,
                        driver_contact_no = cab.driver_contact_no,
                        driver_name = cab.driver_name
                    });
                    HomePage.viewModel.UpdateHomePageUI_OnCabBooked();
                }
                else if (header == "CAB_ARRIVED")
                {
                    Cab cab = CabController.GetCab(applicationId);
                    //Update cab details
                    HomePage.viewModel.DisplayCabList.Clear();
                    HomePage.viewModel.DisplayCabList.Add(new CMSapp.ViewModels.HomePageCab
                    {
                        applicationid = cab.applicationid,
                        expected_arrival_time = cab.expected_arrival_time,
                        booking_time = cab.booking_time,
                        cab_no = cab.cab_no,
                        cab_status = cab.cab_status,
                        driver_contact_no = cab.driver_contact_no,
                        driver_name = cab.driver_name
                    });
                    HomePage.viewModel.UpdateHomePageUI_OnCabArrived();
                }
                else Console.WriteLine("Invalid header");
            }
        }

        void SendNotification(string messageBody)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            
            var notificationBuilder = new Notification.Builder(this)
                .SetContentTitle("Application for review!")
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}