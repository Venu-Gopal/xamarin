using CMSapp.Controllers;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSapp.Models
{
    public class Application
    {
        public string travel_purpose { get; set; }
        public string submission_datetime { get; set; }
        public bool roundtrip { get; set; }
        public string escort_name { get; set; }
        public string employeeid { get; set; }
        public string managerid { get; set; }
        public string manager_name { get; set; }
        public string project_bpcode { get; set; }
        public string application_status { get; set; }
        public string cab_required_time { get; set; }
    }
}
