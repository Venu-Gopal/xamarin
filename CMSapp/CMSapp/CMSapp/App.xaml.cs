using CMSapp.Data;
using CMSapp.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CMSapp
{
	public partial class App : Application
	{
        public App ()
		{
            InitializeComponent();            
			MainPage = new NavigationPage(new LoginPage());

            //Initialize unread applications
            DataHandler.unreadApplications = 0;
            Current.Properties[Constants.unreadApplications] = DataHandler.unreadApplications;
		}

        public static Page GetMainPage()
        {
            return new NavigationPage(new HomePage());
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
