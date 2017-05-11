//Login Page is set to MainPage of application (inside App.xaml.cs)
using System;
using Xamarin.Forms;
using CMSapp.Models;
using CMSapp.Controllers;
using System.Diagnostics;
using System.Net;
using CMSapp.Data;
using CMSapp.Pages;
using RestSharp;

namespace CMSapp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {			
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void SignUpButton_OnClick(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new RegistrationPage());
        }


        private async void LoginButton_OnClick(object sender, EventArgs e)
        {
            //Validate input first

            Debug.Print("UserIdEntry: " + UserIdEntry.Text);
            Debug.Print("PasswordEntry: " + PasswordEntry.Text);


            if (!string.IsNullOrEmpty(UserIdEntry.Text) && !string.IsNullOrEmpty(PasswordEntry.Text))
            {
                LoginButton.IsEnabled = false;
                LoginIndicator.IsVisible = true;
                LoginIndicator.IsRunning = true;

                ValidateLabel.Text = "";
                Login login = new Login();
                login.loginid = UserIdEntry.Text;
                login.password = PasswordEntry.Text;

                IRestResponse response = await LoginController.getLoginStatus(login);
                HttpStatusCode status = response.StatusCode;


                switch (status)
                {
                    case HttpStatusCode.OK:
                        LoginIndicator.IsVisible = false;
                        LoginIndicator.IsRunning = false;
                        DataHandler.SaveData(login);
                        Console.WriteLine(response.Content);
                        if (response.Content == "false")
                        {
                            Employee employee = EmployeeController.GetEmployeeAsync(login);
                            DataHandler.SaveData(employee);
                            await Navigation.PushModalAsync(new NavigationPage(new HomePage()));
                        }
                        else
                        {
                            await Navigation.PushModalAsync(new NavigationPage(new SecurityHomePage()));
                        }

                        LoginButton.IsEnabled = true;
                        break;
                    case HttpStatusCode.NotFound:
                        LoginIndicator.IsVisible = false;
                        LoginIndicator.IsRunning = false;
                        LoginButton.IsEnabled = true;
                        ValidateLabel.Text = "EmployeeID or password is incorrect";
                        break;
                    case HttpStatusCode.InternalServerError:
                        LoginIndicator.IsVisible = false;
                        LoginIndicator.IsRunning = false;
                        LoginButton.IsEnabled = true;
                        ValidateLabel.Text = "An internal server error occurred.";
                        break;
                }
            }

            else
            {
                if (string.IsNullOrEmpty(UserIdEntry.Text) && string.IsNullOrEmpty(PasswordEntry.Text))
                {
                    ValidateLabel.Text = "Please enter a EmployeeID and Password.";
                }
                else
                {
                    if (string.IsNullOrEmpty(UserIdEntry.Text))
                    {
                        ValidateLabel.Text = "Please enter a EmployeeID.";
                    }
                    else if (string.IsNullOrEmpty(PasswordEntry.Text))
                    {
                        ValidateLabel.Text = "Please enter a password.";
                    }
                }
            }




        }

    }
}