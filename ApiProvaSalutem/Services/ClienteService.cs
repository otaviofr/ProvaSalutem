using System;
using System.Collections.Generic;
using System.Linq;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.Infraestructure.Repositories;
using ApiProvaSalutem.Model;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public void Save(ClienteDTO obj){
            var dados = new Cliente(
                obj.Id,
                obj.IdCliente,  
                obj.Cnpj,
                obj.RazaoSocial,
                obj.Latitude,
                obj.Longitude
            );
            _clienteRepository.Save(dados);
        }

        public void Update(ClienteDTO obj){
            var dados = _clienteRepository.GetUnique(obj.IdCliente);
            dados.Update(
                obj.Id,
                obj.IdCliente,
                obj.Cnpj,
                obj.RazaoSocial,
                obj.Latitude,
                obj.Longitude
            );
            _clienteRepository.Update(dados);
        }

        public IEnumerable<ClienteViewModel> GetById(long id, int skip, int limit){
            return _clienteRepository.GetById(id, skip, limit)
            .Select(x=> new ClienteViewModel {
                Id = x.Id,
                IdCliente = x.IdCliente,
                Cnpj = x.Cnpj,
                RazaoSocial = x.RazaoSocial,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });
        }
    }
}