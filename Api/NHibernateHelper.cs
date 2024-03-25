using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Reflection;

namespace Api
{
    public class NHibernateHelper
    {
        public static ISessionFactory BuildSessionFactory(IConfiguration configuration)
        {
            var configurationSettings = new Dictionary<string, string>
            {
                { "connection.provider", "NHibernate.Connection.DriverConnectionProvider" },
                { "dialect", "NHibernate.Dialect.MsSql2012Dialect" },
                { "connection.connection_string", configuration.GetConnectionString("Db") },
                { "connection.release_mode", "auto" },
                { "default_schema", "dbo" },
                { "cache.use_query_cache", "false" },
                { "flushmode", "auto" },
                { "show_sql", "false" },
                { "generate_statistics", "true" },
                { "cache.provider_class", "NHibernate.Cache.HashtableCacheProvider" },
                { "cache.use_second_level_cache", "false" },
                { "adonet.batch_size", "10" },
                { "prepare_sql", "true" }
            };

            var cfg = new Configuration();
            cfg.SetProperties(configurationSettings);
            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            return cfg.BuildSessionFactory();
        }
    }
}
