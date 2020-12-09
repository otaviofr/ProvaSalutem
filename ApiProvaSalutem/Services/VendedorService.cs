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
    public class VendedorService : IVendedorService // define herança
    {
        private readonly IVendedorRepository _vendedorRepository; // instancia interface do repositorio vendedor

        public VendedorService(IVendedorRepository vendedorRepository) //construtor VendedorService
        {
            _vendedorRepository = vendedorRepository;
        }

        // método que salva o vendedor no banco de dados
        public void Save(VendedorDTO obj)
        {
            var cpf = obj.Cpf; // pega o cpf

            //verifica se o usuario informou o cpf com pontos e realiza formatação correta, e, caso nao tenha informado formata da forma correta
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

            //verifica se ja existe um vendedor com esse cpf
            var existsSeller = _vendedorRepository.GetAll().ToList().FindAll(x => x.Cpf == cpf);

            //caso não exista, cadastra o vendedor
            if (existsSeller.Count == 0 || existsSeller == null)
            {
                //verifica se o cpf informado contem pontos
                if (!obj.Cpf.Contains('.') && !obj.Cpf.Contains('-'))
                {
                    if (obj.Cpf.Length == 11) // verifica se o cpf contem a quantidade correta de caracteres
                    {
                        obj.Cpf = Convert.ToUInt64(obj.Cpf).ToString(@"000\.000\.000\-00"); // coloca a máscara correta no cpf

                        //cria um novo obejto Vendedor para ser salvo no banco de dados
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
                else // se o cpf foi informado com pontos, entra neste laço
                {
                    var cnpj = IsCpf(obj.Cpf); // verifica se o cpf esta em um formato válido

                    if (cnpj == true) // caso o cpf seja válido realiza o cadastro
                    {
                        //cria um novo obejto Vendedor para ser salvo no banco de dados
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

        //metodo que atualiza o vendedor
        public void Update(VendedorDTO obj)
        {
            //verifica se o cpf informado, caso tenha alterado contem pontos
            if (!obj.Cpf.Contains('.') && !obj.Cpf.Contains('-'))
            {
                if (obj.Cpf.Length == 11) // verifica se o cpf tem a quantidade correta de caracteres
                {
                    obj.Cpf = Convert.ToUInt64(obj.Cpf).ToString(@"000\.000\.000\-00"); // coloca a máscara do cpf

                    //busca vendedor a ser atualizado e salva alterações
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
            else // entra neste laço caso o cpf informado contenha pontos
            {
                var cnpj = IsCpf(obj.Cpf); //verifica se o formato do cpf é válido

                if (cnpj == true) // se for válido entra neste laço e salva a atualização
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

        //método que pega um unico vendedor
        public IEnumerable<VendedorViewModel> GetById(long id)
        {
            //faz a seleção do vendedor informado no banco e retorna para o usuário
            return _vendedorRepository.GetById(id)
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

        // método que deleta um vendedor
        public void Delete(long idVendedor)
        {
            // verifica se o vendedor à excluir existe
            var existsSeller = _vendedorRepository.GetAll().ToList().FindAll(x => x.IdVendedor == idVendedor);

            if (existsSeller.Count != 0) // caso exista apaga do banco de dados
            {
                _vendedorRepository.Delete(idVendedor);
            }
            else // caso nao exista retorna erro ao usuário
            {
                throw new Exception("Erro: Cliente não encontrado!");
            }
        }

        //método que retorna todos vendedores cadastrados
        public IEnumerable<VendedorViewModel> GetAll(int skip = 0, int limit = 50)
        {
            //faz a seleção de todos vendedores no banco e retorna para o usuário
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

        //método que exporta vendedores, com filtro ou todos
        public byte[] ExportSeller(long? idVendedor, string? nomeVendedor)
        {
            //busca todos vendedores da base sem levar em consideração os filtros
            var sellers = _vendedorRepository.GetAll();

            //caso o filtro por id seja informado, entra neste laço e retorna somente os vendedores desejados
            if (idVendedor != null)
            {
                sellers = _vendedorRepository.GetAll().ToList().FindAll(x => x.IdVendedor == idVendedor);
            }

            //caso o filtro por nome seja informado, entra neste laço e retorna somente os vendedores desejados
            if (nomeVendedor != null)
            {
                //padroniza string para busca no banco, ignorando acentos, maiusculas e minusculas
                nomeVendedor = PadronizaString(nomeVendedor);

                sellers = _vendedorRepository.GetAll().ToList().FindAll(x => PadronizaString(x.NomeVendedor).Contains(nomeVendedor));
            }

            //instancia do XLWorkbook para criar o arquivo excel
            using (var workbook = new XLWorkbook())
            {
                //nome dos dados contidos no arquivo
                var worksheet = workbook.Worksheets.Add("Vendedores");
                //define primeira linha do documento
                var currentRow = 1;

                //criação dos titulos das colunas
                worksheet.Cell(currentRow, 1).Value = "Id Vendedor";
                worksheet.Cell(currentRow, 2).Value = "CPF";
                worksheet.Cell(currentRow, 3).Value = "Nome Vendedor";
                worksheet.Cell(currentRow, 4).Value = "Latitude";
                worksheet.Cell(currentRow, 5).Value = "Longitude";

                //laço para gravação de todos vendedores encontrados no documento
                foreach (var seller in sellers)
                {
                    //para cada vendedor cadastrado sera inserido uma nova linha com seus atributos
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = seller.IdVendedor;
                    worksheet.Cell(currentRow, 2).Value = seller.Cpf;
                    worksheet.Cell(currentRow, 3).Value = seller.NomeVendedor;
                    worksheet.Cell(currentRow, 4).Value = seller.Latitude;
                    worksheet.Cell(currentRow, 5).Value = seller.Longitude;
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

        //método que valida o formato do cpf
        public static bool IsCpf(string cpf)
        {
            //recebe o cpf informado, e caso contenha algum ponto, é removido para ser inserido a máscara correta
            string CPF = cpf.Replace(".", "");
            CPF = CPF.Replace("-", "");

            // verifica se o cpf contem a quantidade correta de caracteres
            if (CPF.Length == 11)
                return true;
            else //caso não tenha é retornado erro ao usuário
                return false;
        }

        /*método para padronização da string, recebe uma string, normaliza e em seguida percorre todos os caracteres
          removendo acentos e letras especiais,
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