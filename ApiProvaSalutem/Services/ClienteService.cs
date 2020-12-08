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

        public void Save(ClienteDTO obj)
        {
            var existsClient = _clienteRepository.GetAll().ToList().FindAll(x => x.Cnpj == obj.Cnpj);

            if (existsClient.Count == 0 || existsClient == null)
            {
                if (!obj.Cnpj.Contains('.') && !obj.Cnpj.Contains('/') && !obj.Cnpj.Contains('-'))
                {
                    if (obj.Cnpj.Length == 14)
                    {
                        obj.Cnpj = Convert.ToUInt64(obj.Cnpj).ToString(@"00\.000\.000\/0000\-00");

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
                    else
                    {
                        throw new Exception("Erro: CNPJ inválido!");
                    }
                }
                else
                {
                    var cnpj = IsCnpj(obj.Cnpj);

                    if (cnpj == true)
                    {
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
                    else
                    {
                        throw new Exception("Erro: CNPJ inválido!");
                    }
                }
            }
            else
            {
                throw new Exception("Erro: Já existe um cliente com este CNPJ!");
            }
        }

        public void Update(ClienteDTO obj)
        {
            if (!obj.Cnpj.Contains('.') && !obj.Cnpj.Contains('/') && !obj.Cnpj.Contains('-'))
            {
                if (obj.Cnpj.Length == 14)
                {
                    obj.Cnpj = Convert.ToUInt64(obj.Cnpj).ToString(@"00\.000\.000\/0000\-00");

                    var dados = _clienteRepository.GetUnique(obj.IdCliente);
                    dados.Update(
                        obj.IdCliente,
                        obj.Cnpj,
                        obj.RazaoSocial,
                        obj.Latitude,
                        obj.Longitude
                    );
                    _clienteRepository.Update(dados);
                }
                else
                {
                    throw new Exception("Erro: CNPJ inválido!");
                }
            }
            else
            {
                var cnpj = IsCnpj(obj.Cnpj);

                if (cnpj == true)
                {
                    var dados = _clienteRepository.GetUnique(obj.IdCliente);
                    dados.Update(
                        obj.IdCliente,
                        obj.Cnpj,
                        obj.RazaoSocial,
                        obj.Latitude,
                        obj.Longitude
                    );
                    _clienteRepository.Update(dados);
                }
                else
                {
                    throw new Exception("Erro: CNPJ inválido!");
                }
            }

        }

        public IEnumerable<ClienteViewModel> GetById(long id, int skip, int limit)
        {
            return _clienteRepository.GetById(id, skip, limit)
            .Select(x => new ClienteViewModel
            {
                Id = x.Id,
                IdCliente = x.IdCliente,
                Cnpj = x.Cnpj,
                RazaoSocial = x.RazaoSocial,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });
        }

        public void Delete(long idCliente)
        {
            _clienteRepository.Delete(idCliente);
        }

        public IEnumerable<ClienteViewModel> GetAll(int skip = 0, int limit = 50)
        {
            return _clienteRepository.GetAll(skip, limit)
                 .Select(x => new ClienteViewModel
                 {
                     Id = x.Id,
                     IdCliente = x.IdCliente,
                     Cnpj = x.Cnpj,
                     RazaoSocial = x.RazaoSocial,
                     Latitude = x.Latitude,
                     Longitude = x.Longitude
                 });
        }

        public static bool IsCnpj(string cnpj)
        {
            string CNPJ = cnpj.Replace(".", "");
            CNPJ = CNPJ.Replace("/", "");
            CNPJ = CNPJ.Replace("-", "");

            if (CNPJ.Length == 14)
                return true;
            else
                return false;
        }
    }
}