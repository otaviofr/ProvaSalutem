using ApiProvaSalutem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    public interface IClienteRepository
    {
        Task<bool> Save(Cliente obj);
        Cliente GetUnique(long id);
        IEnumerable<Cliente> GetById(long id, int skip, int limit);
        Task<bool> Update(Cliente obj);
    }
}