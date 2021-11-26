using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.FirebaseManagement.v1beta1;
using Google.Apis.FirebaseManagement.v1beta1.Data;
using System.IO;

namespace CloudResourceManager
{
    public static class FirebaseManagement
    {
        private static FirebaseManagementService _firebaseManagementService;

        public static void InitializeFirebaseManagement()
        {
            //var buffer = Convert.FromBase64String(ServiceAccountManager.DataKey.PrivateKeyData);
            //Stream stream = new MemoryStream(buffer);
            //var credential = GoogleCredential.FromStream(stream);
            var credential = GoogleCredential.FromFile("private_key.json");
            if (CloudManager.Credential.IsCreateScopedRequired)
            {
                credential = CloudManager.Credential.CreateScoped(FirebaseManagementService.Scope.CloudPlatform);
            }
            _firebaseManagementService = new FirebaseManagementService(
               new BaseClientService.Initializer()
               {
                  
                   HttpClientInitializer = credential,
                   ApplicationName = CloudManager.ApplicationName

               });
        }

        public static void AddFirebase()
        {
            var request = new AddFirebaseRequest();
            var operationFirebase1 = _firebaseManagementService.Projects.AddFirebase(request,
                "projects/" + CloudManager.ProjectId).Execute();
            WaitOperation(operationFirebase1, "Firebase");

        }

        public static void CreateAndroidProject()
        {
            var body = new AndroidApp()
            {
                DisplayName = "GamanetAndroid",
                PackageName = "com.test1.testnotification",
            };
            var operationAndroid1 = _firebaseManagementService.Projects.AndroidApps.Create(body, "projects/" + CloudManager.ProjectId).Execute();
            WaitOperation(operationAndroid1, nameof(AndroidApp));
        }

        public static void CreateiOSProject()
        {
            var body = new IosApp()
            {
                DisplayName = "GamanetIOS",
                BundleId = "com.test1.TestNotification",
                TeamId = "U7H97X23T2",
            };
            var operationIOS1 = _firebaseManagementService.Projects.IosApps.Create(body, "projects/" + CloudManager.ProjectId).Execute();
            WaitOperation(operationIOS1, nameof(IosApp));
        }

        public static AdminSdkConfig GetAdminConfig()
        {
            return _firebaseManagementService.Projects.GetAdminSdkConfig("projects/" + CloudManager.ProjectId + "/adminSdkConfig").Execute();
        }

        public static AndroidAppConfig GetAndroidConfig()
        {
            var listAndroid = _firebaseManagementService.Projects.AndroidApps.List("projects/" + CloudManager.ProjectId + "/androidApps").Execute();
            return _firebaseManagementService.Projects.AndroidApps.GetConfig("projects/-/androidApps/" + listAndroid.Apps[0].AppId + "/config").Execute();
        }

        public static IosAppConfig GetIOSConfig()
        {
            var listIOS = _firebaseManagementService.Projects.IosApps.List("projects/" + CloudManager.ProjectId + "/iosApps").Execute();
            return _firebaseManagementService.Projects.IosApps.GetConfig("projects/-/iosApps/" + listIOS.Apps[0].AppId + "/config").Execute();
        }

        private static void WaitOperation(Operation operation, string name)
        {
            Operation operation2;
            do
            {
                operation2 = _firebaseManagementService.Operations.Get(operation.Name).Execute();
                Console.WriteLine($"Creating - {name}........" + operation2.Done.ToString());
                System.Threading.Thread.Sleep(1000);
            } while (operation2.Done != true);
        }
    }
}
