using System.Collections.Generic;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Services
{
    public interface IClienteService
    {
        void Save(ClienteDTO obj);
        void Update(ClienteDTO obj);
        IEnumerable<ClienteViewModel> GetById(long id, int skip, int limit);
    }
}