using System;

namespace CMSapp.Models
{
    public class Cab
    {
        public int cabid { get; set; }
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
}
