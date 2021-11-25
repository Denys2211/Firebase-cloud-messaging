using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.CloudResourceManager.v1;

using Data = Google.Apis.CloudResourceManager.v1.Data;
using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;
using System.Collections.Generic;
using Google.Apis.CloudResourceManager.v1.Data;
using Policy = Google.Apis.CloudResourceManager.v1.Data.Policy;
using System.Linq;
namespace CloudResourceManager
{
    public static class AccessManager
    {
        public static Policy AddMember(Policy policy, string role, string member)
        {
            var binding = policy.Bindings.First(x => x.Role == role);
            binding.Members.Add(member);
            return policy;
        }
        public static Policy RemoveMember(Policy policy, string role, string member)
        {
            try
            {
                var binding = policy.Bindings.First(x => x.Role == role);
                if (binding.Members.Count != 0 && binding.Members.Contains(member))
                {
                    binding.Members.Remove(member);
                }
                if (binding.Members.Count == 0)
                {
                    policy.Bindings.Remove(binding);
                }
                return policy;
            }
            catch (System.InvalidOperationException e)
            {
                System.Diagnostics.Debug.WriteLine("Role does not exist in policy: \n" + e.ToString());
                return policy;
            }
        }
        public static Policy AddBinding(Policy policy, string role, string member)
        {
            var binding = new Data.Binding
            {
                Role = role,
                Members = new List<string> { member }
            };
            policy.Bindings.Add(binding);
            return policy;
        }
        public static Policy GetPolicy(string projectId, CloudResourceManagerService cloudResourceManager)
        {
            var policy = cloudResourceManager.Projects.GetIamPolicy(new GetIamPolicyRequest(),
                projectId).Execute();
            return policy;
        }
        public static Policy SetPolicy(string projectId, Policy policy)
        {
            return CloudManager.CloudResourceManagerService.Projects.SetIamPolicy(new Data.SetIamPolicyRequest
            {
                Policy = policy
            }, projectId).Execute();
        }
        public static Role CreateCustomRole(IamService service, string projectid)
        {
            var role = new Role
            {
                Title = "C# Test Custom Role",
                Description = "Role for AccessTest",
                IncludedPermissions = new List<string>
                {
                    "resourcemanager.projects.get",
                    "firebase.projects.get",
                    "firebase.projects.update",
                },
                Stage = "GA"
            };

            var request = new CreateRoleRequest
            {
                Role = role,
                RoleId = "csharpTestCustomRole" + new Random().Next()
            };

            return service.Projects.Roles.Create(request, "projects/" + projectid).Execute();


        }

        public static string ParseRoleName(Role role)
        {
            var roleNameComponents = role.Name.Split('/');
            var roleNameShort = roleNameComponents[2] + "/" + roleNameComponents[3];
            return roleNameShort;
        }
    }
}
