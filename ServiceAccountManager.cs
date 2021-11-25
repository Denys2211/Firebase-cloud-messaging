using System;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;
using System.Threading;
namespace CloudResourceManager
{
    public static  class ServiceAccountManager
    {
        public static IamService Service { get; private set; }
        public static ServiceAccountKey DataKey { get; private set; }
        public static string _ID { get; private set; }

        public static void InitializeIamService()
        {
            CancellationToken canTok = new CancellationToken();
            var credential = Task.Run(async
                () => await GoogleCredential.FromFileAsync("private.json", canTok)
            ).Result;
            if (credential.IsCreateScopedRequired)
            {
                credential = credential.CreateScoped(IamService.Scope.CloudPlatform); ;
            }
            Service = new IamService(new IamService.Initializer
            {
                HttpClientInitializer = credential
            });
        }
        public static void CreateServiceAccount(string projectId, string name, string displayName)
        {

            var request = new CreateServiceAccountRequest
            {

                AccountId = name,
                ServiceAccount = new ServiceAccount
                {
                    DisplayName = displayName
                }
            };
            var serviceAccount = Service.Projects.ServiceAccounts.Create(
                request, "projects/" + projectId).Execute();
            _ID = serviceAccount.ProjectId;
            Console.WriteLine("Created service account: " + serviceAccount.Email);
            EnableServiceAccount(serviceAccount.Email);
            CreateKey(serviceAccount.Email);
            SetAccess(serviceAccount.Email);

        }
        public static void DeleteServiceAccount(string email)
        {
            var credential = GoogleCredential.FromFile("adc.json")
                .CreateScoped(IamService.Scope.CloudPlatform);
            Service = new IamService(new IamService.Initializer
            {
                HttpClientInitializer = credential
            });

            string resource = "projects/-/serviceAccounts/" + email;
            Service.Projects.ServiceAccounts.Delete(resource).Execute();
            Console.WriteLine("Deleted service account: " + email);
        }
        public static void EnableServiceAccount(string email)
        {
            var request = new EnableServiceAccountRequest();

            string resource = "projects/-/serviceAccounts/" + email;
            Service.Projects.ServiceAccounts.Enable(request, resource).Execute();
            Console.WriteLine("Enabled service account: " + email);
        }
        public static void CreateKey(string serviceAccountEmail)
        {
            //var listkey = Service.Projects.ServiceAccounts.Keys.List("projects/-/serviceAccounts/" + serviceAccountEmail);

            DataKey = Service.Projects.ServiceAccounts.Keys.Create(
                new CreateServiceAccountKeyRequest(),
                "projects/-/serviceAccounts/" + serviceAccountEmail)
                .Execute();
            Console.WriteLine("Created key: " + DataKey.Name);
        }
        public static void SetAccess(string serviceAccountEmail)
        {

            //var role = AccessManager.CreateCustomRole(Service, CloudManager.ProjectId);
            //var roleNameShort = AccessManager.ParseRoleName(role);
            // Test GetPolicy
            var policy = AccessManager.GetPolicy(CloudManager.ProjectId, CloudManager.CloudResourceManagerService);

            policy = AccessManager.AddBinding(policy, "roles/firebase.managementServiceAgent", "serviceAccount:"+serviceAccountEmail);
            //Roles:https://cloud.google.com/iam/docs/understanding-roles
            // Test SetPolicy
            AccessManager.SetPolicy(CloudManager.ProjectId, policy);//https://cloud.google.com/iam/docs/overview
        }
    }
}
