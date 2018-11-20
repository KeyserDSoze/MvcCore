using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.AuthenticationProvider
{
    public class DapperUsersTable
    {
        private readonly string connection;
        public DapperUsersTable(string connection)
        {
            this.connection = connection;
        }

        #region createuser
        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            int rows = 1;
            if (rows > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }
        #endregion

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            int rows = 1;

            if (rows > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}." });
        }


        public async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return null;
        }


        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return null;
        }
    }
}
