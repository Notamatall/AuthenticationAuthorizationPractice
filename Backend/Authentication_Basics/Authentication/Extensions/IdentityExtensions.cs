using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace API.Authentication.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services) =>
            services.AddIdentityService(o => { });

        public static IServiceCollection AddIdentityService(this IServiceCollection services, Action<IdentityOptions> identityOptions)
        {
            services.AddOptions<IdentityOptions>().Configure(identityOptions);
            services.AddSingleton<IIdentityService, IdentityService>();
            return services;
        }
        public static IServiceCollection WithMsSqlServer(this IServiceCollection services, Action<MsSqlServerOptions> options) =>
               services.WithMsSqlServer<DBUserModel>(options);

        public static IServiceCollection WithMsSqlServer<TUserInformationModel>(this IServiceCollection services, Action<MsSqlServerOptions> options) where TUserInformationModel : IDBUserModel
        {
            services.Configure(options);

            services.AddTransient<IDbUserRepository, MsSqlUserRepository<TUserInformationModel>>();

            return services;
        }

        public static IServiceCollection WithDapper(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IDbConnection, DbConnection>(sp =>
            {
                var connection = new SqlConnection(configuration.GetConnectionString("Db"));
                return connection;
            });


            return services;
        }

    }
}
