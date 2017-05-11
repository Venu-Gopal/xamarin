//Contains the list (ReviewAppList ) that populate the Manager's UI.
//This list can be updated from anywhere in the app when the UI data needs to be updated. A static instance of this viewModel is created in HomePage.xaml.cs
using CMSapp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CMSapp.ViewModels
{
    public class ApplicationReviewViewModel : INotifyPropertyChanged
    {
        
        private  ObservableCollection<ReviewApplication> _reviewapplist;
        public ObservableCollection<ReviewApplication> ReviewAppList 
        {
            get
            {
                return _reviewapplist;
            }
            set
            {
                _reviewapplist = value;
                OnPropertyChanged("ReviewAppList");
            }
        }

        public ApplicationReviewViewModel()
        {
            ReviewAppList = new ObservableCollection<ReviewApplication>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
    }
}
