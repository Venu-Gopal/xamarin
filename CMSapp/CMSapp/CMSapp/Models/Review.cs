//Data models for information displayed on Manager's 'Applications to Review Page' 
//Information to be displayed on ApplicationReviewPage is handled inside ApplicationReviewViewModel

using System.Collections.ObjectModel;


namespace CMSapp.Models
{
    public class ReviewApplication
    {
        public string applicationid { get; set; }
        public string travel_purpose { get; set; }
        public string submission_datetime { get; set; }
        public string roundtrip { get; set; }
        public string project_bpcode { get; set; }
        public string application_status { get; set; }
        public string cab_required_time { get; set; }
        public ObservableCollection<ReviewPassenger> PassengerList { get; set; }
    }

    public class ReviewPassenger
    {
        public string employeeid { get; set; }
        public string in_time { get; set; }

        public string pickup { get; set; }
        public string destination { get; set; }


    }
}
