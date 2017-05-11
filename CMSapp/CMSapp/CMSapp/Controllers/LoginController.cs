//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Login Details
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CMSapp.Models;
using CMSapp.Controllers;
using static CMSapp.Constants;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using CMSapp.Data;
using CMSApp.Droid;

namespace CMSapp.Controllers
{
    class LoginController
    {
        public async static  Task<IRestResponse> getLoginStatus(Login login)
        {
            var request = new RestRequest("/login", Method.POST);
            request.AddParameter("loginid", login.loginid);
            request.AddParameter("password", login.password);
            string firebaseToken = DataHandler.getFirebaseToken();
            if (!string.IsNullOrEmpty(firebaseToken))
            {
                request.AddParameter("fcmid", firebaseToken);
            }

            //local storage : replaces previous login 


            IRestResponse response = new RequestController(request).send();

            return response;
        }
    }
}
