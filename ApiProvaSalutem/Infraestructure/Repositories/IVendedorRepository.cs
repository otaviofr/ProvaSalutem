using ApiProvaSalutem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    public interface IVendedorRepository
    {
        Task<bool> Save(Vendedor obj);
        Vendedor GetUnique(long id);
        void Delete(long idCliente);
        IEnumerable<Vendedor> GetById(long id, int skip, int limit);
        Task<bool> Update(Vendedor obj);
        IEnumerable<Vendedor> GetAll(int skip = 0, int limit = 50);
    }
}