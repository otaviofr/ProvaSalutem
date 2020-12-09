using System.Collections.Generic;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Services
{
    public interface IClienteService
    {
        void Save(ClienteDTO obj);
        void Update(ClienteDTO obj);
        void Delete(long idCliente);
        IEnumerable<ClienteViewModel> GetAll(int skip = 0, int limit = 50);
        IEnumerable<ClienteViewModel> GetById(long id, int skip, int limit);
        byte[] ExportCostumer(long? idCliente, string? razaoSocial);
    }
}