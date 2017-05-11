using CMSapp.Controllers;
using CMSapp.Data;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMSapp.PushNotification
{
    class PushNotificationHandler
    {
        public static void saveFcmID(string fcmId)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(Constants.RestBaseUri);
            client.Authenticator = new NtlmAuthenticator();

            var request = new RestRequest("/login", Method.POST);
            request.AddParameter("loginid", Data.DataHandler.getLoginData().loginid);
            request.AddParameter("fcmid", fcmId);


            IRestResponse response = new RequestController(request).send();
        }
    }
}
