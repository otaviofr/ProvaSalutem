using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiProvaSalutem.Model
{
    //Classe modelo do vendedor
    public class Vendedor
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}
        public long IdVendedor { get; set; }
        public string Cpf { get; set; }
        public string NomeVendedor { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Vendedor(string id, long idVendedor, string cpf, string nomeVendedor, string latitude, string longitude)
        {
            Id = id;
            IdVendedor = idVendedor;
            Cpf = cpf;
            NomeVendedor = nomeVendedor;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void Update(long idVendedor, string cpf, string nomeVendedor, string latitude, string longitude)
        {
            IdVendedor = idVendedor;
            Cpf = cpf;
            NomeVendedor = nomeVendedor;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}