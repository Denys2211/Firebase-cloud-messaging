using System;
using CloudResourceManager;
using Google.Apis.FirebaseManagement.v1beta1.Data;

namespace CloudResourceManagers
{
    public class Program
    {
        private static Google.Apis.FirebaseManagement.v1beta1.Data.AdminSdkConfig _cfAdmin;
        private static Google.Apis.FirebaseManagement.v1beta1.Data.AndroidAppConfig _cfAndroid;
        private static IosAppConfig _cfIos;

        public static void Main(string[] args)
        {
            CloudManager.GetCredential("private.json");
            FirebaseManagement.InitializeFirebaseManagement();
            bool i = true;
            while (i)
            {
                Console.WriteLine("--------Choice:" +
                    "\n0 - Write ID project" +
                    "\n1 - Create project" +
                    "\n2 - Delete project" +
                    "\n3 - Create Service Account" +
                    "\n4 - Add Firebase" +
                    "\n5 - Add Android project" +
                    "\n6 - Add iOS project" +
                    "\n7 - Get Admin config" +
                    "\n8 - Get Android config" +
                    "\n9 - Get iOS config" +
                    "\n10 - Exit");
            switch(Console.ReadLine())
                {
                    case "0":
                        {
                            Console.Write("Write ID project : ");
                            var value = Console.ReadLine();
                            CloudManager.ProjectId = $"gamanet-{value}";
                            break;
                        }
                    case "1":
                        {
                            CloudManager.InitializeCloudManager();
                            CloudManager.CreateProject();
                            ServiceUsage.InitializeServiceUsage();
                            ServiceUsage.BatchEnable();
                            break;
                        }
                    case "2":
                        {
                            CloudManager.InitializeCloudManager();
                            CloudManager.DeleteProject();
                            break;
                        }
                    case "3":
                        {
                            ServiceAccountManager.InitializeIamService();
                            ServiceAccountManager.CreateServiceAccount(CloudManager.ProjectId, CloudManager.ApplicationName, CloudManager.DisplayName);
                            break;
                        }
                    case "4":
                        {
                            var account = ServiceAccountManager.GetFirebaseServiceAccount();
                            if (account != null)
                                ServiceAccountManager.CreateKey(account.Email);
                            FirebaseManagement.AddFirebase();
                            break;
                        }
                    case "5":
                        {
                            FirebaseManagement.CreateAndroidProject();
                            break;
                        }
                    case "6":
                        {
                            FirebaseManagement.CreateiOSProject();
                            break;
                        }
                    case "7":
                        {
                           
                            _cfAdmin = FirebaseManagement.GetAdminConfig();
                            break;
                        }
                    case "8":
                        {
                            _cfAndroid = FirebaseManagement.GetAndroidConfig();
                            break;
                        }
                    case "9":
                        {
                            _cfIos = FirebaseManagement.GetIOSConfig();
                            break;
                        }
                    case "10":
                        {
                            i = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Not correct");
                            break;
                        }
                }
            }
        }
    }
}
