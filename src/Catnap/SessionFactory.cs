using System;
using Catnap.Database;
using Catnap.Mapping;

namespace Catnap
{
    public class SessionFactory : ISessionFactory
    {
        private static ISessionFactory current;

        private readonly IDbAdapter dbAdapter;
        private readonly string connectionString;
        private readonly IDomainMap domainMap;

        public SessionFactory(string connectionString, IDbAdapter dbAdapter, IDomainMap domainMap)
        {
            this.connectionString = connectionString;
            this.dbAdapter = dbAdapter;
            this.domainMap = domainMap;
        }

        public static ISessionFactory Current
        {
            get
            {
                if (current == null)
                {
                    throw new InvalidOperationException("SessionFactory not initialized.");
                }
                return current;
            }
        }

        public IDomainMap DomainMap
        {
            get { return domainMap; }
        }

        public IDbAdapter DbAdapter
        {
            get { return dbAdapter; }
        }

        internal static void SetCurrent(ISessionFactory instance)
        {
            current = instance;
        }

        public ISession New()
        {
            return new Session(DomainMap, connectionString, DbAdapter);
        }

        //TODO: This does not belong here
        public string FormatParameterName(string name)
        {
            const string defaultSqlParameterPrefix = "@";
            return DbAdapter == null
                ? name.StartsWith(defaultSqlParameterPrefix)
                    ? name
                    : defaultSqlParameterPrefix + name
                : DbAdapter.FormatParameterName(name);
        }
    }
}