//Contains methods that communicate with the REST API
//Methods are used to implement CRUD and other operations on Employee Data
using System;
using System.Threading.Tasks;
using CMSapp.Models;
using RestSharp;
using System.Diagnostics;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using CMSapp.Data;
using Newtonsoft.Json.Linq;

namespace CMSapp.Controllers
{
    class EmployeeController
    {
        public static async Task AddEmployeeAsync(Employee employee, Login login)
        {
            var request = new RestRequest("/register", Method.POST);
            request.AddParameter("employeeid", employee.employeeid);
            request.AddParameter("contact_no", employee.contactno);
            request.AddParameter("department", employee.department);
            request.AddParameter("name", employee.name);
            request.AddParameter("gender", employee.gender);
            request.AddParameter("password", login.password);

            IRestResponse response = new RequestController(request).send();

            dynamic resContent = JsonConvert.DeserializeObject(response.Content);

            string empid = resContent.employeeid;
            string loginid = resContent.loginid;


            Debug.WriteLine("employeeid: " + empid + " , loginid: " + loginid);

            Data.DataHandler.SaveData(employee);
            Data.DataHandler.SaveData(login);
           
        }
        public static Employee GetEmployeeAsync(Login login)
        {
            var request = new RestRequest("/employees/"+login.loginid, Method.GET);
            IRestResponse response = new RequestController(request).send();

            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);

            return employee;
        }
    }
}