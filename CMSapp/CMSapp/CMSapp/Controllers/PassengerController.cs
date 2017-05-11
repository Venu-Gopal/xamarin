//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Passenger Data
using CMSapp.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CMSapp.Controllers
{
    class PassengerController
    {
        public static void AddPassengerAsync( List<Passenger> passengerlist,  int appid)
        {
            var request = new RestRequest("/createPassenger", Method.POST);
            var json = request.JsonSerializer.Serialize((
                new
                {
                    applicationid = appid,
                    passengers = passengerlist
                }));

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

            IRestResponse response = new RequestController(request).send();
        }

        public static List<Passenger> GetPassengers(int applicationid)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(Constants.RestBaseUri);
            client.Authenticator = new NtlmAuthenticator();

            var request = new RestRequest("/passengers/?where={\"applicationid\":{\"contains\":"+ applicationid + "}}", Method.GET);
            IRestResponse response = new RequestController(request).send();

            List<Passenger> passengers = JsonConvert.DeserializeObject<List<Passenger>>(response.Content);
            return passengers;
        }
    }
}
