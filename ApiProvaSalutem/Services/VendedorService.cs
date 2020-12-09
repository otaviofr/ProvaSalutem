using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ApiProvaSalutem.DTO;
using ApiProvaSalutem.Infraestructure.Repositories;
using ApiProvaSalutem.Model;
using ApiProvaSalutem.ViewModel;
using ClosedXML.Excel;

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
            var existsSeller = _vendedorRepository.GetAll().ToList().FindAll(x => x.IdVendedor == idVendedor);

            if (existsSeller.Count != 0)
            {
                _vendedorRepository.Delete(idVendedor);
            }
            else
            {
                throw new Exception("Erro: Cliente não encontrado!");
            }
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

        public byte[] ExportSeller(long? idVendedor, string? nomeVendedor)
        {
            var sellers = _vendedorRepository.GetAll();

            if (idVendedor != null)
            {
                sellers = _vendedorRepository.GetAll().ToList().FindAll(x => x.IdVendedor == idVendedor);
            }

            if (nomeVendedor != null)
            {
                if (nomeVendedor != null)
                    nomeVendedor = PadronizaString(nomeVendedor);

                sellers = _vendedorRepository.GetAll().ToList().FindAll(x => PadronizaString(x.NomeVendedor).Contains(nomeVendedor));
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Vendedores");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Id Vendedor";
                worksheet.Cell(currentRow, 2).Value = "CPF";
                worksheet.Cell(currentRow, 3).Value = "Nome Vendedor";
                worksheet.Cell(currentRow, 4).Value = "Latitude";
                worksheet.Cell(currentRow, 5).Value = "Longitude";

                foreach (var seller in sellers)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = seller.IdVendedor;
                    worksheet.Cell(currentRow, 2).Value = seller.Cpf;
                    worksheet.Cell(currentRow, 3).Value = seller.NomeVendedor;
                    worksheet.Cell(currentRow, 4).Value = seller.Latitude;
                    worksheet.Cell(currentRow, 5).Value = seller.Longitude;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
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

        public static string PadronizaString(string s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString().ToLower();
        }
    }
}