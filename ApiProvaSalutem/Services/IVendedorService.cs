using System.Collections.Generic;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Services
{
    public interface IVendedorService
    {
        void Save(VendedorDTO obj);
        void Update(VendedorDTO obj);
        void Delete(long idVendedor);
        IEnumerable<VendedorViewModel> GetAll(int skip = 0, int limit = 50);
        IEnumerable<VendedorViewModel> GetById(long id, int skip, int limit);
        byte[] ExportSeller(long? idVendedor, string? nomeVendedor);
    }
}