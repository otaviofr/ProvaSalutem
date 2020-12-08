using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiProvaSalutem.Model
{
    public class Cliente
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id {get; set;}
        public long IdCliente { get; set; }
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public Cliente(string id, long idCliente, string cnpj, string razaoSocial, string latitude, string longitude)
        {
            Id = id;
            IdCliente = idCliente;
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void Update(string id, long idCliente, string cnpj, string razaoSocial, string latitude, string longitude)
        {
            Id = id;
            IdCliente = idCliente;
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}