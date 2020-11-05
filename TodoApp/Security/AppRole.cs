using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Security
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole()
        {
            Claims = new List<IdentityRoleClaim<string>>();
        }

        public AppRole(string name) : this()
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
        }

        public override string ToString()
        {
            return Name;
        }

        public List<IdentityRoleClaim<string>> Claims { get; set; }
    }
}
