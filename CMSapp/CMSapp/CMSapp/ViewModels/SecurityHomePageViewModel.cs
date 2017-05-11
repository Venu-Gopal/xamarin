using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace CMSapp.ViewModels
{
    public class SecurityPassenger
    {
        public string employeeid { get; set; }
        public string in_time { get; set; }
        public string pickup { get; set; }
        public string destination { get; set; }

    }
    public class SecurityApplication
    {
        public string applicationid { get; set; }
        public string submission_datetime { get; set; }
        public string application_status { get; set; }
        public string roundtrip { get; set; }
        public string cab_required_time { get; set; }
        public ObservableCollection<SecurityPassenger> SecurityPassengerList { get; set; }
    }

    public class SecurityHomePageViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<SecurityApplication> _securityapplist;
        public ObservableCollection<SecurityApplication> SecurityAppList
        {
            get
            {
                return _securityapplist;
            }
            set
            {
                _securityapplist = value;
                OnPropertyChanged("SecurityAppList");
            }
        }

        private ObservableCollection<SecurityPassenger> _securitypassengerlist;
        public ObservableCollection<SecurityPassenger> SecurityPassengerList
        {
            get
            {
                return _securitypassengerlist;
            }
            set
            {
                _securitypassengerlist = value;
                OnPropertyChanged("SecurityPassengerList");
            }
        }

        public SecurityHomePageViewModel()
        {
            SecurityAppList = new ObservableCollection<SecurityApplication>();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }   
}
