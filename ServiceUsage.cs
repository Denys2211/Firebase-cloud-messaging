using System;
using Google.Apis.Services;
using Google.Apis.ServiceUsage.v1beta1;
using Google.Apis.ServiceUsage.v1beta1.Data;

namespace CloudResourceManager
{
    public static class ServiceUsage //https://googleapis.dev/dotnet/Google.Apis.ServiceUsage.v1/latest/api/Google.Apis.ServiceUsage.v1.ServicesResource.EnableRequest.html
    {
        private static readonly string[] _aPIS = new[]
        {
            "firebase.googleapis.com",
            "fcm.googleapis.com",
            "iam.googleapis.com"
        };
        private static ServiceUsageService _serviceUsageManagerService;

        public static void InitializeServiceUsage()
        {

            _serviceUsageManagerService = new ServiceUsageService(
                new BaseClientService.Initializer()
                {
                    HttpClientInitializer = CloudManager.Credential,
                    ApplicationName = CloudManager.ApplicationName
                }
            );
        }
        public static void BatchEnable()
        {
            for(int i = _aPIS.Length - 1; i >= 0; i--)
                EnableApi(_aPIS[i]);
        }

        public static void EnableApi(string api)
        {
            var operationEnable = _serviceUsageManagerService.Services.Enable(
                new EnableServiceRequest(), "projects/" + CloudManager.ProjectId + "/services/" + api).Execute();
            WaitOperation(operationEnable, api);
            
        }
        public static void WaitOperation(Operation operation, string api)
        {
            Operation operation2;
            do
            {
                operation2 = _serviceUsageManagerService.Operations.Get(operation.Name).Execute();
                Console.WriteLine("Enable API - " + api + "......" + operation2.Done.ToString());
                System.Threading.Thread.Sleep(1000);
            } while (operation2.Done != true);
        }
    }
}
