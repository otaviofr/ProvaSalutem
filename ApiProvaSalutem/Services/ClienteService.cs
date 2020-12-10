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
    public class ClienteService : IClienteService // define herança
    {
        private readonly IClienteRepository _clienteRepository; // instancia interface do repositorio cliente

        public ClienteService(IClienteRepository clienteRepository) // construtor ClienteService
        {
            _clienteRepository = clienteRepository;
        }

        // método que salva o cliente no banco de dados
        public void Save(ClienteDTO obj)
        {
            var cnpjs = obj.Cnpj; // pega o cnpj

            //verifica se o usuario informou o cnpj com pontos e realiza formatação correta, e, caso nao tenha informado formata da forma correta
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

            //verifica se ja existe um cliente com esse cnpj
            var existsClient = _clienteRepository.GetAll().ToList().FindAll(x => x.Cnpj == cnpjs);

            //caso não exista, cadastra o cliente
            if (existsClient.Count == 0 || existsClient == null)
            {
                //verifica se o cnpj informado contem pontos
                if (!obj.Cnpj.Contains('.') && !obj.Cnpj.Contains('/') && !obj.Cnpj.Contains('-'))
                {
                    if (obj.Cnpj.Length == 14) // verifica se o cnpj contem a quantidade correta de caracteres
                    {
                        obj.Cnpj = Convert.ToUInt64(obj.Cnpj).ToString(@"00\.000\.000\/0000\-00"); // coloca a máscara correta no cnpj

                        //cria um novo obejto Cliente para ser salvo no banco de dados
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
                else // se o cnpj foi informado com pontos, entra neste laço
                {
                    var cnpj = IsCnpj(obj.Cnpj); // verifica se o cnpj esta em um formato válido

                    if (cnpj == true) // caso o cnpj seja válido realiza o cadastro
                    {
                        //cria um novo obejto Cliente para ser salvo no banco de dados
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

        //metodo que atualiza o cliente
        public void Update(ClienteDTO obj)
        {
            //verifica se o cnpj informado, caso tenha alterado contem pontos
            if (!obj.Cnpj.Contains('.') && !obj.Cnpj.Contains('/') && !obj.Cnpj.Contains('-'))
            {
                if (obj.Cnpj.Length == 14) // verifica se o cnpj tem a quantidade correta de caracteres
                {
                    obj.Cnpj = Convert.ToUInt64(obj.Cnpj).ToString(@"00\.000\.000\/0000\-00"); // coloca a máscara do cnpj

                    //busca cliente a ser atualizado e salva alterações
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
            else // entra neste laço caso o cnpj informado contenha pontos
            {
                var cnpj = IsCnpj(obj.Cnpj); //verifica se o formato do cnpj é válido

                if (cnpj == true) // se for válido entra neste laço e salva a atualização
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

        //método que pega um unico cliente
        public IEnumerable<ClienteViewModel> GetById(long id)
        {
            //faz a seleção do cliente informado no banco de dados e retorna para o usuário
            return _clienteRepository.GetById(id)
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

        // método que deleta um cliente
        public void Delete(long idCliente)
        {
            // verifica se o cliente à excluir existe
            var existsClient = _clienteRepository.GetAll().ToList().FindAll(x => x.IdCliente == idCliente);

            if (existsClient.Count != 0) // caso exista apaga do banco de dados
            {
                _clienteRepository.Delete(idCliente);
            }
            else // caso nao exista retorna erro ao usuário
            {
                throw new Exception("Erro: Cliente não encontrado!");
            }
        }

        //método que retorna todos clientes cadastrados
        public IEnumerable<ClienteViewModel> GetAll(int skip = 0, int limit = 50)
        {
            //faz a seleção de todos clientes no banco e retorna para o usuário
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

        //método que exporta clientes, com filtro ou todos
        public byte[] ExportCostumer(long? idCliente, string? razaoSocial)
        {
            //busca todos clientes da base sem levar em consideração os filtros
            var clients = _clienteRepository.GetAll();

            //caso o filtro por id seja informado, entra neste laço e retorna somente os clientes desejados
            if (idCliente != null)
            {
                clients = _clienteRepository.GetAll().ToList().FindAll(x => x.IdCliente == idCliente);
            }

            //caso o filtro por razao social seja informado, entra neste laço e retorna somente os clientes desejados
            if (razaoSocial != null)
            {
                //padroniza string para busca no banco, ignorando acentos, maiusculas e minusculas
                razaoSocial = PadronizaString(razaoSocial);

                clients = _clienteRepository.GetAll().ToList().FindAll(x => PadronizaString(x.RazaoSocial).Contains(razaoSocial));
            }

            //instancia do XLWorkbook para criar o arquivo excel
            using (var workbook = new XLWorkbook())
            {
                //nome dos dados contidos no arquivo
                var worksheet = workbook.Worksheets.Add("Clientes");
                //define primeira linha do documento
                var currentRow = 1;

                //criação dos titulos das colunas
                worksheet.Cell(currentRow, 1).Value = "Id Cliente";
                worksheet.Cell(currentRow, 2).Value = "CNPJ";
                worksheet.Cell(currentRow, 3).Value = "Razão Social";
                worksheet.Cell(currentRow, 4).Value = "Latitude";
                worksheet.Cell(currentRow, 5).Value = "Longitude";

                //laço para gravação de todos clientes encontrados no documento
                foreach (var client in clients)
                {
                    //para cada cliente cadastrado sera inserido uma nova linha com seus atributos
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = client.IdCliente;
                    worksheet.Cell(currentRow, 2).Value = client.Cnpj;
                    worksheet.Cell(currentRow, 3).Value = client.RazaoSocial;
                    worksheet.Cell(currentRow, 4).Value = client.Latitude;
                    worksheet.Cell(currentRow, 5).Value = client.Longitude;
                }

                //instancia do memory stream
                using (var stream = new MemoryStream())
                {
                    //salva os dados e passa para um array, onde é montado o excel
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        //método que valida o formato e se é válido o cnpj
        private static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /*método para padronização da string, recebe uma string, normaliza e em seguida percorre todos os caracteres
          removendo acentos e letras especiais
          e por ultimo deixa toda string minuscula*/
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