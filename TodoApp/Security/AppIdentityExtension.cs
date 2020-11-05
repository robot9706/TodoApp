using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using TodoApp.Data.Model;

namespace TodoApp.Security
{
    public static class AppIdentityExtension
    {
		public static IdentityBuilder AddAppIndentityManager(this IServiceCollection services, Action<IdentityOptions> options = null)
		{
			var builder = services.AddIdentity<User, AppRole>(options ?? (x => { }));

			builder.AddRoleStore<AppRoleStore>()
			.AddUserStore<AppUserStore>()
			.AddUserManager<UserManager<User>>()
			.AddRoleManager<RoleManager<AppRole>>()
			.AddDefaultTokenProviders();

			services.AddSingleton<IRoleStore<AppRole>>(x => new AppRoleStore());
			services.AddSingleton<IUserStore<User>>(x => new AppUserStore());

			return builder;
		}
	}
}
