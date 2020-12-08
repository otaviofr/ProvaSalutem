using ApiProvaSalutem.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.IO;

namespace ApiProvaSalutem.Infraestructure
{
    public class MongoContext
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase database;
        public IConfigurationRoot Configuration { get; }

        public MongoContext()
        {
            Configuration = new ConfigurationBuilder().

                SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            mongoClient = new MongoClient(Configuration["MongoDB:ConnectionString"]);
            database = mongoClient.GetDatabase(Configuration["MongoDB:Database"]);
        }

        public IMongoCollection<Cliente> DSalutem_Cliente
        {
            get
            {
                return database.GetCollection<Cliente>(nameof(Cliente));
            }
        }
    }
}