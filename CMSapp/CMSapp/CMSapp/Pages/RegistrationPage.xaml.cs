using System;
using CMSapp.Models;
using CMSapp.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CMSapp.Data;

namespace CMSapp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class RegistrationPage : ContentPage
	{
        public RegistrationPage()
        {
            InitializeComponent();
        }

        //TO BE DONE : Form Validation. All  fields are REQUIRED and cannot be null.
        private async void SignUp_OnClick_Register(object sender, EventArgs e)
        {
            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                PasswordMismatchLabel.IsVisible = !PasswordMismatchLabel.IsVisible;
            }
            else
            {
                Employee employee = new Employee();
                Login login = new Login();

                employee.employeeid = EmployeeIDEntry.Text;
                employee.name = NameEntry.Text;
                employee.department = DepartmentEntry.Text;
                employee.contactno = ContactNoEntry.Text;
                employee.gender = GenderPicker.Items[GenderPicker.SelectedIndex];

                login.loginid = EmployeeIDEntry.Text;
                login.password = PasswordEntry.Text;
                
                EmployeeController.AddEmployeeAsync(employee, login);


                await Navigation.PushModalAsync(new Pages.HomePage());
            }
        }
            
	}
}
