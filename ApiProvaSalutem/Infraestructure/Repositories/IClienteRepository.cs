using ApiProvaSalutem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> Save(Cliente obj);
        Cliente GetUnique(long id);
        void Delete(long idCliente);
        IEnumerable<Cliente> GetById(long id, int skip, int limit);
        Task<bool> Update(Cliente obj);
        IEnumerable<Cliente> GetAll(int skip = 0, int limit = 50);
    }
}