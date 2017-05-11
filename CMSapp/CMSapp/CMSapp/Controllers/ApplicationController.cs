//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Application Data

using CMSapp.Models;
using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CMSapp.Controllers
{
    class ApplicationController
    {
        public static async Task<int> AddApplicationAsync(Application application, List<Passenger> passengerlist)
        {
            var request = new RestRequest("/createApplication", Method.POST);
            request.AddParameter("travel_purpose", application.travel_purpose);
            request.AddParameter("roundtrip", application.roundtrip);
            request.AddParameter("employeeid", application.employeeid);
            request.AddParameter("escort_name", application.escort_name);        
            request.AddParameter("managerid", application.managerid);
            request.AddParameter("manager_name", application.manager_name);
            request.AddParameter("project_bpcode", application.project_bpcode);
            request.AddParameter("submission_datetime", application.submission_datetime);
            request.AddParameter("cab_required_time", application.cab_required_time);
            IRestResponse response = new RequestController(request).send();
            dynamic resContent = JsonConvert.DeserializeObject(response.Content);
            int applicationid = resContent.id;
            if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
            {
                return applicationid;
            }
            return -1;
        }

        public static Application GetApplication(int applicationid)
        {
            var request = new RestRequest("/applications/" + applicationid, Method.GET);
            IRestResponse response = new RequestController(request).send();

            Application application = JsonConvert.DeserializeObject<Application>(response.Content);
            return application;
        }

        public static void updateApplication(int applicationid, Application application)
        {
            var request = new RestRequest("/updateApplication/", Method.POST);
            request.AddParameter("applicationid", applicationid);
            request.AddParameter("travel_purpose", application.travel_purpose);
            request.AddParameter("roundtrip", application.roundtrip);
            request.AddParameter("employeeid", application.employeeid);
            request.AddParameter("escort_name", application.escort_name);
            request.AddParameter("managerid", application.managerid);
            request.AddParameter("manager_name", application.manager_name);
            request.AddParameter("project_bpcode", application.project_bpcode);
            request.AddParameter("submission_datetime", application.submission_datetime);
            request.AddParameter("cab_required_time", application.cab_required_time);

            IRestResponse response = new RequestController(request).send();
        }


        public static void RejectApplication(int applicationid)
        {
            Application application = GetApplication(applicationid);
            var request = new RestRequest("/updateApplication/", Method.POST);
            request.AddParameter("applicationid", applicationid);
            request.AddParameter("travel_purpose", application.travel_purpose);
            request.AddParameter("roundtrip", application.roundtrip);
            request.AddParameter("employeeid", application.employeeid);
            request.AddParameter("escort_name", application.escort_name);
            request.AddParameter("application_status", "REJECTED");
            request.AddParameter("managerid", application.managerid);
            request.AddParameter("manager_name", application.manager_name);
            request.AddParameter("project_bpcode", application.project_bpcode);
            request.AddParameter("submission_datetime", application.submission_datetime);
            request.AddParameter("cab_required_time", application.cab_required_time);
            IRestResponse response = new RequestController(request).send();
        }

        public static void ApproveApplication(int applicationid)
        {
            Application application = GetApplication(applicationid);
            var request = new RestRequest("/updateApplication/", Method.POST);
            request.AddParameter("applicationid", applicationid);
            request.AddParameter("travel_purpose", application.travel_purpose);
            request.AddParameter("roundtrip", application.roundtrip);
            request.AddParameter("employeeid", application.employeeid);
            request.AddParameter("escort_name", application.escort_name);
            request.AddParameter("application_status", "APPROVED");
            request.AddParameter("managerid", application.managerid);
            request.AddParameter("manager_name", application.manager_name);
            request.AddParameter("project_bpcode", application.project_bpcode);
            request.AddParameter("submission_datetime", application.submission_datetime);
            request.AddParameter("cab_required_time", application.cab_required_time);
            IRestResponse response = new RequestController(request).send();
            if(application.application_status != "SECURITY")
            {
                var newRequest = new RestRequest("/security", Method.POST);
                newRequest.AddParameter("applicationid", applicationid);

                IRestResponse newResponse = new RequestController(newRequest).send();
            }
        }
        public static void UpdateStatustoSECURITY(int applicationid)
        {
            Application application = GetApplication(applicationid);
            var request = new RestRequest("/updateApplication/", Method.POST);
            request.AddParameter("applicationid", applicationid);
            request.AddParameter("travel_purpose", application.travel_purpose);
            request.AddParameter("roundtrip", application.roundtrip);
            request.AddParameter("employeeid", application.employeeid);
            request.AddParameter("escort_name", application.escort_name);
            request.AddParameter("application_status", "SECURITY");
            request.AddParameter("managerid", application.managerid);
            request.AddParameter("manager_name", application.manager_name);
            request.AddParameter("project_bpcode", application.project_bpcode);
            request.AddParameter("submission_datetime", application.submission_datetime);
            request.AddParameter("cab_required_time", application.cab_required_time);
            IRestResponse response = new RequestController(request).send();

        }


        public static void SendToSecurity(int applicationid)
        {
            var request = new RestRequest("/security", Method.POST);
            request.AddParameter("applicationid", applicationid);

            IRestResponse response = new RequestController(request).send();
        }
    }
}
