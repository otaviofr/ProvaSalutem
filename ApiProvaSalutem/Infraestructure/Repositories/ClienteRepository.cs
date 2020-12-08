using ApiProvaSalutem.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly MongoContext _mongoContext;

        public ClienteRepository()
        {
            _mongoContext = new MongoContext();
        }

        public async Task<bool> Save(Cliente obj)
        {
            await _mongoContext.DSalutem_Cliente.InsertOneAsync(obj);
            return true;
        }

        public Cliente GetUnique(long id)
        {
            return _mongoContext.DSalutem_Cliente
                .Find(x => x.IdCliente.Equals(id)).FirstOrDefault();
        }

        public async Task<bool> Update(Cliente obj)
        {
            var filter = Builders<Cliente>.Filter.Eq(x => x.IdCliente, obj.IdCliente);
            var update = Builders<Cliente>.Update.
                Set(x => x.IdCliente, obj.IdCliente).
                Set(x => x.Cnpj, obj.Cnpj).
                Set(x => x.RazaoSocial, obj.RazaoSocial).
                Set(x => x.Latitude, obj.Latitude).
                Set(x => x.Longitude, obj.Longitude);

            var result = await _mongoContext.DSalutem_Cliente.
                UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }
        public IEnumerable<Cliente> GetById(long id, int skip, int limit)
        {
            var cliente = _mongoContext.DSalutem_Cliente.Find(x => x.IdCliente.Equals(id)).Skip(skip).Limit(limit).ToList();

            return cliente;
        }

        public void Delete(long idCliente)
        {
            _mongoContext.DSalutem_Cliente.DeleteOne(x => x.IdCliente == idCliente);
        }

        public IEnumerable<Cliente> GetAll(int skip = 0, int limit = 50)
        {
            return _mongoContext.DSalutem_Cliente.Find(x => true).Skip(skip).Limit(limit).ToList();
        }
    }
}