using System;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.CloudResourceManager.v1;
using Google.Apis.Services;

using Data = Google.Apis.CloudResourceManager.v1.Data;
namespace CloudResourceManager
{
    public static class CloudManager
    {
        public static CloudResourceManagerService CloudResourceManagerService { get; private set; }
        public static GoogleCredential Credential { get; set; }
        public static string ProjectId { get; set; }
        public static string ApplicationName { get; private set; } = "gamanetcom";
        public static string DisplayName { get; private set; } = "Testgamanet";


        public static void InitializeCloudManager()
        {

            CloudResourceManagerService = new CloudResourceManagerService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = Credential,
                    ApplicationName = CloudManager.ApplicationName
                }
            );


        }
        public static void GetCredential(string path)
        {
            //Auth https://docs.microsoft.com/en-us/advertising/scripts/examples/authenticating-with-google-services
            var scopes = new string[] {
                CloudResourceManagerService.Scope.CloudPlatform
            };
            Credential = Task.Run(
                () => GoogleCredential.FromFile(path)//file with "client_id", "client_secret", "refresh_token"
            ).Result;

            if (Credential.IsCreateScopedRequired)
            {
                Credential = Credential.CreateScoped(scopes);
            }
        }

        public static void CreateProject()
        {
            //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\apikey.json");
            //string Pathsave = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            Console.WriteLine("1. Create Project");
            Data.Operation operation1 = CloudResourceManagerService.Projects.Create(
                new Data.Project()
                {
                    Name = ProjectId,
                    ProjectId = ProjectId
                }
            ).Execute();
            Console.WriteLine("2. Awaiting Operation Completion");
            Data.Operation operation2;
            do
            {
                operation2 = CloudResourceManagerService.Operations.Get(operation1.Name).Execute();
                Console.WriteLine("Creating........" + operation2.Done.ToString());
                System.Threading.Thread.Sleep(1000);
            } while (operation2.Done != true);
        }
        public static void DeleteProject()
        {
            Console.WriteLine(" Deleting Project");
            var operation3 = CloudResourceManagerService.Projects.Delete(ProjectId).Execute();
        }
    }
}
