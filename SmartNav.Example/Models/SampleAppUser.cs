using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace SmartNav.Example.Models
{
    // You can add profile data for the user by adding more properties to your SampleAppUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class SampleAppUser : IUser
    {
        private readonly List<string> _roles;

        public SampleAppUser(string id, string userName)
        {
            if (id == null) throw new ArgumentNullException("id");
            if (userName == null) throw new ArgumentNullException("userName");
            Id = id;
            _roles = new List<string>();
            UserName = userName;
        }

        public static SampleAppUser New(string userName)
        {
            return new SampleAppUser("id", userName)
                   {
                       Id = null
                   };
        }

        public string Id { get; private set; }
        public string UserName { get; set; }

        public IList<string> Roles
        {
            get { return _roles; }
        }

        public SampleAppUser WithRole(string role)
        {
            _roles.Add(role);
            return  this;
        }

        public SampleAppUser WithRoles(params string[] roles)
        {
            _roles.AddRange(roles);
            return this;
        }
    }
}