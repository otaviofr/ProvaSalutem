using System;
using System.Collections.Generic;
using System.Linq;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.Infraestructure.Repositories;
using ApiProvaSalutem.Model;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Services
{
    public class VendedorService : IVendedorService
    {
        private readonly IVendedorRepository _vendedorRepository;

        public VendedorService(IVendedorRepository vendedorRepository)
        {
            _vendedorRepository = vendedorRepository;
        }

        public void Save(VendedorDTO obj)
        {
            var cpf = obj.Cpf;

            if (cpf.Contains("."))
            {
                cpf = cpf.Replace(".", "");
                cpf = cpf.Replace("-", "");
                cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
            }
            else
            {
                cpf = Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
            }

            var existsSeller = _vendedorRepository.GetAll().ToList().FindAll(x => x.Cpf == cpf);

            if (existsSeller.Count == 0 || existsSeller == null)
            {
                if (!obj.Cpf.Contains('.') && !obj.Cpf.Contains('-'))
                {
                    if (obj.Cpf.Length == 11)
                    {
                        obj.Cpf = Convert.ToUInt64(obj.Cpf).ToString(@"000\.000\.000\-00");

                        var dados = new Vendedor(
                            obj.Id,
                            obj.IdVendedor,
                            obj.Cpf,
                            obj.NomeVendedor,
                            obj.Latitude,
                            obj.Longitude
                        );
                        _vendedorRepository.Save(dados);
                    }
                    else
                    {
                        throw new Exception("Erro: CPF inválido!");
                    }
                }
                else
                {
                    var cnpj = IsCpf(obj.Cpf);

                    if (cnpj == true)
                    {
                        var dados = new Vendedor(
                            obj.Id,
                            obj.IdVendedor,
                            obj.Cpf,
                            obj.NomeVendedor,
                            obj.Latitude,
                            obj.Longitude
                        );
                        _vendedorRepository.Save(dados);
                    }
                    else
                    {
                        throw new Exception("Erro: CPF inválido!");
                    }
                }
            }
            else
            {
                throw new Exception("Erro: Já existe um vendedor com este CPF!");
            }
        }

        public void Update(VendedorDTO obj)
        {
            if (!obj.Cpf.Contains('.') && !obj.Cpf.Contains('-'))
            {
                if (obj.Cpf.Length == 11)
                {
                    obj.Cpf = Convert.ToUInt64(obj.Cpf).ToString(@"00\.000\.000\/0000\-00");

                    var dados = _vendedorRepository.GetUnique(obj.IdVendedor);
                    dados.Update(
                        obj.IdVendedor,
                        obj.Cpf,
                        obj.NomeVendedor,
                        obj.Latitude,
                        obj.Longitude
                    );
                    _vendedorRepository.Update(dados);
                }
                else
                {
                    throw new Exception("Erro: CPF inválido!");
                }
            }
            else
            {
                var cnpj = IsCpf(obj.Cpf);

                if (cnpj == true)
                {
                    var dados = _vendedorRepository.GetUnique(obj.IdVendedor);
                    dados.Update(
                        obj.IdVendedor,
                        obj.Cpf,
                        obj.NomeVendedor,
                        obj.Latitude,
                        obj.Longitude
                    );
                    _vendedorRepository.Update(dados);
                }
                else
                {
                    throw new Exception("Erro: CPF inválido!");
                }
            }

        }

        public IEnumerable<VendedorViewModel> GetById(long id, int skip, int limit)
        {
            return _vendedorRepository.GetById(id, skip, limit)
            .Select(x => new VendedorViewModel
            {
                Id = x.Id,
                IdVendedor = x.IdVendedor,
                Cpf = x.Cpf,
                NomeVendedor = x.NomeVendedor,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            });
        }

        public void Delete(long idVendedor)
        {
            _vendedorRepository.Delete(idVendedor);
        }

        public IEnumerable<VendedorViewModel> GetAll(int skip = 0, int limit = 50)
        {
            return _vendedorRepository.GetAll(skip, limit)
                 .Select(x => new VendedorViewModel
                 {
                     Id = x.Id,
                     IdVendedor = x.IdVendedor,
                     Cpf = x.Cpf,
                     NomeVendedor = x.NomeVendedor,
                     Latitude = x.Latitude,
                     Longitude = x.Longitude
                 });
        }

        public static bool IsCpf(string cpf)
        {
            string CPF = cpf.Replace(".", "");
            CPF = CPF.Replace("-", "");

            if (CPF.Length == 11)
                return true;
            else
                return false;
        }
    }
}