using ApiProvaSalutem.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    public class VendedorRepository : IVendedorRepository
    {
        private readonly MongoContext _mongoContext;

        public VendedorRepository()
        {
            _mongoContext = new MongoContext();
        }

        public async Task<bool> Save(Vendedor obj)
        {
            await _mongoContext.DSalutem_Vendedor.InsertOneAsync(obj);
            return true;
        }

        public Vendedor GetUnique(long id)
        {
            return _mongoContext.DSalutem_Vendedor
                .Find(x => x.IdVendedor.Equals(id)).FirstOrDefault();
        }

        public async Task<bool> Update(Vendedor obj)
        {
            var filter = Builders<Vendedor>.Filter.Eq(x => x.IdVendedor, obj.IdVendedor);
            var update = Builders<Vendedor>.Update.
                Set(x => x.IdVendedor, obj.IdVendedor).
                Set(x => x.Cpf, obj.Cpf).
                Set(x => x.NomeVendedor, obj.NomeVendedor).
                Set(x => x.Latitude, obj.Latitude).
                Set(x => x.Longitude, obj.Longitude);

            var result = await _mongoContext.DSalutem_Vendedor.
                UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        public IEnumerable<Vendedor> GetById(long id, int skip, int limit)
        {
            var vendedor = _mongoContext.DSalutem_Vendedor.Find(x => x.IdVendedor.Equals(id)).Skip(skip).Limit(limit).ToList();

            return vendedor;
        }

        public void Delete(long idVendedor)
        {
            _mongoContext.DSalutem_Vendedor.DeleteOne(x => x.IdVendedor == idVendedor);
        }

        public IEnumerable<Vendedor> GetAll(int skip = 0, int limit = 50)
        {
            return _mongoContext.DSalutem_Vendedor.Find(x => true).Skip(skip).Limit(limit).ToList();
        }
    }
}