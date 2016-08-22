using System.Configuration;
using System.Linq;

namespace TasksManager.Infra.Data
{
    public class MongoConnection
    {
        public string Server { get; private set; }
        public string Database { get; private set; }
        public MongoConnection(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (connectionString != null)
                Split(connectionString.ConnectionString);
        }

        private void Split(string connectionString)
        {
            var connectionSplited = connectionString?.Split(';');

            if (connectionSplited == null)
                return;

            var toServer = connectionSplited.FirstOrDefault(i => i.ToLower().Contains("server="));

            if (toServer != null)
                Server = toServer.ToLower();

            var toDatabase = connectionSplited.FirstOrDefault(i => i.ToLower().Contains("database="));

            if (toDatabase != null)
                Database = toDatabase;
        }

    }
}