using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Transaction;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class OracleSessionFactoryHelper
    {
        private static OracleSessionFactoryHelper _instance = new OracleSessionFactoryHelper();
        private static readonly ISessionFactory SessionFactory;

        //HACK:这个地方只初始化一次，会不会有问题，只能产生一个工厂是否能满足需求？
        public ISessionFactory GetSessionFactory()
        {
            return SessionFactory;
        }

        static OracleSessionFactoryHelper()
        {
            var cfg = new Configuration();
            cfg.SessionFactory()
                   .Proxy
                       .DisableValidation()
                       .Through<NHibernate.Bytecode.DefaultProxyFactoryFactory>()
                       .Named("Fluent.SessionFactory")
                       .GenerateStatistics()
                       .Using(EntityMode.Poco)
                       .ParsingHqlThrough<NHibernate.Hql.Ast.ANTLR.ASTQueryTranslatorFactory>()
                   .Integrate
                       .Using<Oracle10gDialect>()
                       .AutoQuoteKeywords()
                       .LogSqlInConsole()
                       .EnableLogFormattedSql()
                   .Connected
                       .Through<DriverConnectionProvider>()
                       .By<OracleDataClientDriver>()
                       .ByAppConfing("ConnectionString")
                   .CreateCommands
                       .Preparing()
                       .WithTimeout(10)
                       .AutoCommentingSql()
                       .WithMaximumDepthOfOuterJoinFetching(11)
                       .WithHqlToSqlSubstitutions("true 1, false 0, yes 'Y', no 'N'");
            SessionFactory = FluentNHibernate.Cfg.Fluently.Configure(cfg)
                   .Database(OracleDataClientConfiguration.Oracle10.ShowSql())
                   .Mappings(m => GetMappingsAssemblies().ToList()
                       .ForEach(o => m.FluentMappings.AddFromAssembly(o))).BuildSessionFactory();
        }

        private static IEnumerable<Assembly> GetMappingsAssemblies()
        {
            string assemblies = System.Configuration.ConfigurationManager.AppSettings["Mappings"];
            return assemblies.Split(',').ToList().Select(Assembly.Load);
        }
    }
}
