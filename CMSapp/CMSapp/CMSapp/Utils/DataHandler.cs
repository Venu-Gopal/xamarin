using CMSapp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace CMSapp.Data
{
    class DataHandler
    {

        public static int unreadApplications = 0;
		public static void AddReviewApplication(Application app, List<Passenger> passengers)
        {
            ((HomePage)Xamarin.Forms.Application.Current.MainPage).AddReviewApp(app, passengers);
        }

        public static void SaveData(Login login)
        {
            string jsonString = JsonConvert.SerializeObject(login);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "login.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Print("File deleted: " + filePath);
                
            }
            var file = File.Open(filePath, FileMode.Create, FileAccess.Write);
            var strm = new StreamWriter(file);
            strm.Write(jsonString);
            Debug.Print("Save JSON String: " + jsonString);
            Debug.Print("File created" + filePath);
        }

        public static void SaveData(Employee employee)
        {
            string jsonString = JsonConvert.SerializeObject(employee);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "employee.json");

            // serialize JSON to a string and then write string to a file
            File.WriteAllText(filePath, jsonString);

            using(StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, employee);
            }
        }
        //public static void SaveData(Application application)
        //{
        //    string jsonString = JsonConvert.SerializeObject(application);
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    string filePath = Path.Combine(path, "application.json");

        //    // serialize JSON to a string and then write string to a file
        //    File.WriteAllText(filePath, jsonString);

        //    using (StreamWriter file = File.CreateText(filePath))
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        serializer.Serialize(file, application);
        //    }
        //}



        public static Login getLoginData()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "login.json");

            if(File.Exists(filePath))
            {
                var file = File.Open(filePath, FileMode.Open, FileAccess.Read);
                var strm = new StreamReader(file);
                {
                    string jsonString =  strm.ReadToEnd();
                    return JsonConvert.DeserializeObject<Login>(jsonString);
                }
            }
            return null;
        }

        public static Employee getEmployeeData()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "employee.json");

            if (File.Exists(filePath))
            {
                Debug.Print("FIle exists");
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (Employee)serializer.Deserialize(file, typeof(Employee));
                }
            }
            return null;
        }
        //public static Application getApplicationData()
        //{
        //    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    string filePath = Path.Combine(path, "application.json");

        //    if (File.Exists(filePath))
        //    {
        //        Debug.Print("FIle exists");
        //        using (StreamReader file = File.OpenText(filePath))
        //        {
        //            JsonSerializer serializer = new JsonSerializer();
        //            return (Application)serializer.Deserialize(file, typeof(Employee));
        //        }
        //    }
        //    return null;
        //}


        public static void saveFirebaseToken(string firebaseToken)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "firebase.txt");
            File.WriteAllText(filePath, firebaseToken);
            
        }

        public static string getFirebaseToken()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string filePath = Path.Combine(path, "firebase.txt");

            if (File.Exists(filePath))
            {
                string firebaseAuth = File.ReadAllText(filePath);
                return firebaseAuth;
            }
            return null;
        }
    }
}
