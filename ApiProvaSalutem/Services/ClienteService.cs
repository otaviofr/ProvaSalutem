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
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public void Save(ClienteDTO obj)
        {
            var cnpjs = obj.Cnpj;

            if (cnpjs.Contains("."))
            {
                cnpjs = cnpjs.Replace(".", "");
                cnpjs = cnpjs.Replace("/", "");
                cnpjs = cnpjs.Replace("-", "");
                cnpjs = Convert.ToUInt64(cnpjs).ToString(@"000\.000\.000\-00");
            }
            else
            {
                cnpjs = Convert.ToUInt64(cnpjs).ToString(@"000\.000\.000\-00");
            }

            var existsClient = _clienteRepository.GetAll().ToList().FindAll(x => x.Cnpj == cnpjs);

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
            var existsClient = _clienteRepository.GetAll().ToList().FindAll(x => x.IdCliente == idCliente);

            if (existsClient.Count != 0)
            {
                _clienteRepository.Delete(idCliente);
            }
            else
            {
                throw new Exception("Erro: Cliente não encontrado!");
            }
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

        public byte[] ExportCostumer(long? idCliente, string? razaoSocial)
        {
            var clients = _clienteRepository.GetAll();

            if (idCliente != null)
            {
                clients = _clienteRepository.GetAll().ToList().FindAll(x => x.IdCliente == idCliente);
            }

            if (razaoSocial != null)
            {
                if (razaoSocial != null)
                    razaoSocial = PadronizaString(razaoSocial);

                clients = _clienteRepository.GetAll().ToList().FindAll(x => PadronizaString(x.RazaoSocial).Contains(razaoSocial));
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Clientes");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Id Cliente";
                worksheet.Cell(currentRow, 2).Value = "CNPJ";
                worksheet.Cell(currentRow, 3).Value = "Razão Social";
                worksheet.Cell(currentRow, 4).Value = "Latitude";
                worksheet.Cell(currentRow, 5).Value = "Longitude";

                foreach (var client in clients)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = client.IdCliente;
                    worksheet.Cell(currentRow, 2).Value = client.Cnpj;
                    worksheet.Cell(currentRow, 3).Value = client.RazaoSocial;
                    worksheet.Cell(currentRow, 4).Value = client.Latitude;
                    worksheet.Cell(currentRow, 5).Value = client.Longitude;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
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