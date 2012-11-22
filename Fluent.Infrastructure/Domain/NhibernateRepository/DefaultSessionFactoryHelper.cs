using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.AdoNet;
using NHibernate.Transaction;
using NHibernate.Exceptions;
using System.Reflection;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class DefaultSessionFactoryHelper
    {
        private static readonly ISessionFactory SessionFactory;

        public ISessionFactory GetSessionFactory()
        {
            return SessionFactory;
        }

        static DefaultSessionFactoryHelper()
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
                    .Using<MsSql2008Dialect>()
                    .Connected
                    .ByAppConfing("ConnectionString")
                .BatchingQueries
                    .Through<SqlClientBatchingBatcherFactory>()
                    .Each(15)
                    .LogSqlInConsole()
                    .EnableLogFormattedSql()
                .Transactions
                    .Through<AdoNetTransactionFactory>()
                .CreateCommands
                    .Preparing()
                    .WithTimeout(10)
                    .AutoCommentingSql()
                    .WithMaximumDepthOfOuterJoinFetching(11)
                    .WithHqlToSqlSubstitutions("true 1, false 0, yes 'Y', no 'N'");

            SessionFactory = FluentNHibernate.Cfg.Fluently.Configure(cfg)
                .Database(MsSqlConfiguration.MsSql2008.ShowSql())
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
