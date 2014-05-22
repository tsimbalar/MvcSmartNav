using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SmartNav.Example.Models
{
    public static class SampleAppRoles
    {
        public const string Admin = "Administrator";
        public const string Marketing = "Marketing";
        public const string Security = "Security";
    }

    public class FakeUserStore : IUserStore<SampleAppUser>, IUserRoleStore<SampleAppUser>
    {
        private static readonly List<SampleAppUser> UsersInternal = new List<SampleAppUser>
                                            {
                                                new SampleAppUser("1", "sa")
                                                        .WithRole(SampleAppRoles.Admin) ,
                                                new SampleAppUser("2", "alice")
                                                        .WithRole(SampleAppRoles.Marketing),
                                                new SampleAppUser("3", "bob")
                                                        .WithRole(SampleAppRoles.Security),
                                                new SampleAppUser("99", "ceo")
                                                        .WithRoles(SampleAppRoles.Marketing, SampleAppRoles.Security)
                                            };

        public static IReadOnlyList<SampleAppUser> Users
        {
            get { return UsersInternal.AsReadOnly(); }
        } 

        public void Dispose()
        {
        }

        public Task CreateAsync(SampleAppUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(SampleAppUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(SampleAppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<SampleAppUser> FindByIdAsync(string userId)
        {
            return Task.FromResult(UsersInternal.SingleOrDefault(u => u.Id == userId));
        }

        public Task<SampleAppUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(UsersInternal.SingleOrDefault(u => u.UserName == userName));
        }


        public Task AddToRoleAsync(SampleAppUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(SampleAppUser user, string role)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(SampleAppUser user)
        {
            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(SampleAppUser user, string role)
        {
            return Task.FromResult(user.Roles.Contains(role));
        }
    }
}