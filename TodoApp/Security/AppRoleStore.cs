using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace TodoApp.Security
{
    public class AppRoleStore : IRoleClaimStore<AppRole>, IQueryableRoleStore<AppRole>
    {
        private List<AppRole> roles = new List<AppRole>()
        {
            new AppRole("ROOT")
        };

        public IQueryable<AppRole> Roles => roles.AsQueryable();

        public Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));

            //var found = roles.FirstOrDefault(x => x.NormalizedName == role.NormalizedName);

            //if (found == null) roles.Add(role);

            //return IdentityResult.Success;

            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));

            ////await _collection.ReplaceOneAsync(x => x.Id == role.Id, role, cancellationToken: cancellationToken).ConfigureAwait(false);

            //return IdentityResult.Success;

            throw new NotImplementedException();
        }

        public IdentityResult Delete(AppRole role, CancellationToken cancellationToken)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));

            //roles.Remove(role);

            //return IdentityResult.Success;

            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id.ToString());
        }

        public string GetRoleName(AppRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return (roles.FirstOrDefault(x => x.Id == role.Id))?.Name ?? role.Name;
        }

        public void SetRoleName(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));
            //if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

            //role.Name = roleName;

            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(AppRole role, string normalizedRoleName, CancellationToken cancellationToken)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));
            //if (string.IsNullOrEmpty(normalizedRoleName)) throw new ArgumentNullException(nameof(normalizedRoleName));

            //role.NormalizedName = normalizedRoleName;

            throw new NotImplementedException();
        }

        public Task<AppRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(roleId)) throw new ArgumentNullException(nameof(roleId));

            return Task.FromResult(roles.FirstOrDefault(x => x.Id == Convert.ToInt32(roleId)));
        }

        public Task<AppRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedRoleName)) throw new ArgumentNullException(nameof(normalizedRoleName));

            return Task.FromResult(roles.FirstOrDefault(x => x.NormalizedName == normalizedRoleName));
        }

        public Task<IList<Claim>> GetClaimsAsync(AppRole role, CancellationToken cancellationToken = default)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            cancellationToken.ThrowIfCancellationRequested();

            var dbRole = roles.FirstOrDefault(x => x.Id == role.Id);

            return Task.FromResult(dbRole.Claims.Select(e => new Claim(e.ClaimType, e.ClaimValue)).ToList() as IList<Claim>);
        }

        public async Task AddClaimAsync(AppRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));
            //if (claim == null) throw new ArgumentNullException(nameof(claim));

            //cancellationToken.ThrowIfCancellationRequested();

            //var currentClaim = role.Claims.FirstOrDefault(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value);

            //if (currentClaim == null)
            //{
            //    var identityRoleClaim = new IdentityRoleClaim<string>()
            //    {
            //        ClaimType = claim.Type,
            //        ClaimValue = claim.Value
            //    };

            //    role.Claims.Add(identityRoleClaim);

            //    //await _collection.UpdateOneAsync(x => x.Id == role.Id, Builders<TRole>.Update.Set(x => x.Claims, role.Claims), cancellationToken: cancellationToken).ConfigureAwait(false);
            //}

            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(AppRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            //if (role == null) throw new ArgumentNullException(nameof(role));
            //if (claim == null) throw new ArgumentNullException(nameof(claim));

            //cancellationToken.ThrowIfCancellationRequested();

            //role.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            //return Task.CompletedTask;

            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            //roles.Remove(role);
            //return Task.FromResult(IdentityResult.Success);

            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            //role.Name = roleName;
            //return Task.CompletedTask;

            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
