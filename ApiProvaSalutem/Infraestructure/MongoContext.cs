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

        public MongoContext() // construtor Mongo Context
        {
            //define configuração de conexão
            Configuration = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            mongoClient = new MongoClient(Configuration["MongoDB:ConnectionString"]);
            database = mongoClient.GetDatabase(Configuration["MongoDB:Database"]);
        }

        // cria a collection cliente no banco
        public IMongoCollection<Cliente> DSalutem_Cliente
        {
            get
            {
                return database.GetCollection<Cliente>(nameof(Cliente));
            }
        }

        // cria a collection vendedor no banco
        public IMongoCollection<Vendedor> DSalutem_Vendedor
        {
            get
            {
                return database.GetCollection<Vendedor>(nameof(Vendedor));
            }
        }
    }
}