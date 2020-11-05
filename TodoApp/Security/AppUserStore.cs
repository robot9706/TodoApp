using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Data.Collections;
using TodoApp.Data.Model;

namespace TodoApp.Security
{
    public class AppUserStore : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>
    {
        public void Dispose()
        {
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            UserCollection.InsertOne(user);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            bool success = UserCollection.DeleteOne(user);

            return Task.FromResult(success ? IdentityResult.Failed() : IdentityResult.Success);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(UserCollection.FindOneById(new ObjectId(userId)));
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(UserCollection.FindByUsername(normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            return Task.FromResult(UserCollection.FindOneById(user.ID).Username.ToUpper());
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ID.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            User dbUser = UserCollection.FindOneById(user.ID);
            if (dbUser == null)
            {
                return Task.FromResult<string>(user.Username);
            }

            return Task.FromResult(dbUser.Username);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                user.Username = userName;

                return Task.CompletedTask;
            }

            bool success = UserCollection.SetUsername(user.ID, userName);

            return Task.FromResult(success ? IdentityResult.Failed() : IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                return Task.FromResult<string>(null);
            }

            return Task.FromResult(UserCollection.FindOneById(user.ID).PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(!string.IsNullOrEmpty(UserCollection.FindOneById(user.ID).PasswordHash));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                user.PasswordHash = passwordHash;

                return Task.CompletedTask;
            }

            bool success = UserCollection.SetPasswordHash(user.ID, passwordHash);

            return Task.FromResult(success ? IdentityResult.Failed() : IdentityResult.Success);
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            bool success = UserCollection.AddRoleToUser(user.ID, roleName);

            return Task.FromResult(success ? IdentityResult.Failed() : IdentityResult.Success);
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            return Task.FromResult(UserCollection.FindOneById(user.ID).Roles.ToList() as IList<string>);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            string[] userRoles = UserCollection.FindOneById(user.ID).Roles;

            return Task.FromResult(userRoles.Contains(roleName));
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            if (user.ID == ObjectId.Empty)
            {
                throw new ArgumentNullException();
            }

            bool success = UserCollection.RemoveRoleFromUser(user.ID, roleName);

            return Task.FromResult(success ? IdentityResult.Failed() : IdentityResult.Success);
        }
    }
}
