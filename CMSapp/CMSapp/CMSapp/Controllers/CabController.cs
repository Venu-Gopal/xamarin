//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Cab Data
using CMSapp.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CMSapp.Controllers
{
    class CabController
    {
        public static Cab GetCab(int applicationid)
        {
            var request = new RestRequest("/cabs?applicationid=" + applicationid, Method.GET);
            IRestResponse response = new RequestController(request).send();

            List<Cab> cabs = JsonConvert.DeserializeObject<List<Cab>>(response.Content);
            if (cabs.Count == 0)
                return null;
            return cabs[0];
        }

        public static void AddCab(Cab cab)
        {
            var request = new RestRequest("/cabs/create", Method.POST);
            request.AddParameter("cab_no", cab.cab_no);
            request.AddParameter("applicationid", cab.applicationid);
            request.AddParameter("driver_name", cab.driver_name);
            request.AddParameter("driver_contact_no", cab.driver_contact_no);
            request.AddParameter("expected_arrival_time", cab.expected_arrival_time);
            request.AddParameter("booking_time", cab.booking_time);
            request.AddParameter("cab_status", cab.cab_status);
            IRestResponse response = new RequestController(request).send();
        }

        public static void UpdateCab(Cab cab)
        {
            var request = new RestRequest("/cabs/" + cab.cabid, Method.PUT);
            request.AddParameter("cab_no", cab.cab_no);
            request.AddParameter("applicationid", cab.applicationid);
            request.AddParameter("driver_name", cab.driver_name);
            request.AddParameter("driver_contact_no", cab.driver_contact_no);
            request.AddParameter("expected_arrival_time", cab.expected_arrival_time);
            request.AddParameter("booking_time", cab.booking_time);
            request.AddParameter("cab_status", cab.cab_status);

            IRestResponse response = new RequestController(request).send();
        }
    }
}
