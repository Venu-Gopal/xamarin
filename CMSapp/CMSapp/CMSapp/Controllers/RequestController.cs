using RestSharp;
using RestSharp.Authenticators;
using System;

namespace CMSapp.Controllers
{
    class RequestController
    {
        private RestClient client;
        RestRequest request;

        public RequestController(RestRequest request)
        {
            client = new RestClient();
            client.BaseUrl = new Uri("http://192.168.1.250:1337");
            client.Authenticator = new NtlmAuthenticator();
            this.request = request;
            request.AddHeader("Content-Type", "application/json");
            request.RequestFormat = DataFormat.Json;
        }

        public IRestResponse send()
        {
            
            IRestResponse response = client.Execute(request);
            return response;
        }

    }
}
