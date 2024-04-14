using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Linq;

namespace API.Authentication
{
    public class MsSqlServerOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SystemObjectsDbSchema { get; set; } = "dbo";
        public Func<MsSqlServerOptions, IDbConnection> GetConnection { get; set; } = (options) =>
        {
            return new SqlConnection(options.ConnectionString);
        };
    }

    public interface IDbUserRepository
    {
        IDBUserModel GetUserInformation(string username);
        bool AreValidCredentials(string username, string password);
    }

    public class MsSqlUserRepository<UserModel> : IDbUserRepository where UserModel : IDBUserModel
    {
        protected readonly MsSqlServerOptions options;
        protected readonly string userExistsSqlQuery;
        protected readonly string userInformationSqlQuery;
        protected readonly IServiceProvider serviceProvider;

        public MsSqlUserRepository(IOptions<MsSqlServerOptions> optionsAccessor, IServiceProvider serviceProvider)
        {
            options = optionsAccessor.Value;
            this.serviceProvider = serviceProvider;

            if (string.IsNullOrEmpty(options.ConnectionString))
                throw new ArgumentException($"{nameof(options.ConnectionString)} cannot be null or empty");
            if (string.IsNullOrEmpty(options.SystemObjectsDbSchema))
                throw new ArgumentException($"{nameof(options.SystemObjectsDbSchema)} cannot be null or empty");

            #region Query Creator

            userExistsSqlQuery = "";

            #endregion
        }

        protected virtual string GetPasswordSalt(string username)
        {
            return username.ToLowerInvariant();
        }

        public virtual bool AreValidCredentials(string username, string password)
        {
            return true;
        }

        public IDBUserModel GetUserInformation(string username)
        {
            var sql = $"select * from users_tb where username = @username";
            IDBUserModel? user = null;
            using (var connection = options.GetConnection(options))
            {
                user = connection.Query<DBUserModel>(sql, new { username })
                    .FirstOrDefault();
            }

            return user;
        }
    }

}
