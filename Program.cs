using System;
using CloudResourceManager;

namespace CloudResourceManagers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CloudManager.GetCredential("private.json");
            Console.WriteLine("Write ID project :");
            var value = Console.ReadLine();
            CloudManager.ProjectId = $"gamanet-{value}";
            for (; ; )
            {
                Console.WriteLine("--------Choice:" +
                    "\n1 - Create project" +
                    "\n2 - Delete project" +
                    "\n3 - Create Service Account" +
                    "\n4 - Add Firebase" +
                    "\n5 - Add Android project" +
                    "\n6 - Add iOS project");
            switch(Console.ReadLine())
                {
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
                            FirebaseManagement.InitializeFirebaseManagement();
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
