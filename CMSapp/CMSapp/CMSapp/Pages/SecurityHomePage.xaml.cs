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
    public partial class SecurityHomePage
    {
        public static SecurityHomePageViewModel viewModel;

        public SecurityHomePage()
        {
            viewModel = new SecurityHomePageViewModel();
            InitializeComponent();
            BindingContext = this;
            SecurityAppListView.ItemsSource = viewModel.SecurityAppList;
        }









        private async void SecurityAppListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                // don't do anything if we just de-selected the row
                return;
            }
            else
            {
                SecurityApplication application = e.SelectedItem as SecurityApplication;
                ((ListView)sender).SelectedItem = null;
                SecurityApplicationReviewPage reviewpage= new SecurityApplicationReviewPage(application.applicationid);

                reviewpage.BindingContext = application;
                await Navigation.PushAsync(reviewpage);
            }
        }
    }
}
