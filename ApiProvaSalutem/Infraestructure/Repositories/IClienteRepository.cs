using ApiProvaSalutem.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProvaSalutem.Infraestructure.Repositories
{
    //Interface onde ficam os métodos relacionados do banco de dados que o repositorio do cliente utiliza
    public interface IClienteRepository
    {
        Task<bool> Save(Cliente obj);
        Cliente GetUnique(long id);
        void Delete(long idCliente);
        IEnumerable<Cliente> GetById(long id);
        Task<bool> Update(Cliente obj);
        IEnumerable<Cliente> GetAll(int skip = 0, int limit = 50);
    }
}