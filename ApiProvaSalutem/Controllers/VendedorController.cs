using Microsoft.AspNetCore.Mvc;
using System;
using ApiProvaSalutem.Services;
using ApiProvaSalutem.DTO;
using System.Collections.Generic;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Controllers
{
    [ApiController]
    [Route("vendedor")] // define rota pricipal
    public class VendedorController : ControllerBase // define herança
    {
        private readonly IVendedorService _vendedorService; // instancia a interface VendedorService

        public VendedorController(IVendedorService vendedorService) // construtor VendedorController
        {
            _vendedorService = vendedorService;
        }

        [HttpGet()] // define tipo do serviço HTTP
        //método que busca um único vendedor
        public IEnumerable<VendedorViewModel> GetById(long idVendedor) // recebe como parametro o id do vendedor desejado
        {
            IEnumerable<VendedorViewModel> vendedorViewModel = null; // cria um objeto null para receber os dados buscados no banco

            vendedorViewModel = _vendedorService.GetById(idVendedor); // busca e atribui resultado obtido do banco a variável

            return vendedorViewModel; // retorna dados para o usuário
        }

        [HttpGet("all")] // define tipo do serviço HTTP e complemento da rota
        //método que buscae todos vendedores
        public IEnumerable<VendedorViewModel> Get(int skip = 0, int limit = 50) // os parametros skip e limit definem paginação com padrão 0 e 50 respectivamente
        {
            return _vendedorService.GetAll(skip, limit); // retorna dados para o usuário
        }

        [HttpPost()] // define tipo do serviço HTTP
        //método que cadastra vendedor
        public IActionResult Post(VendedorDTO body) // recebe como parametro um objeto do tipo Vendedor
        {
            try
            {
                _vendedorService.Save(body); // cadastra vendedor
                return Ok("Vendedor cadastrado com sucesso!"); // retorna mensagem de sucesso ao usuario
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // retorna mensagem de erro ao usuario
            }
        }

        [HttpPut()] // define tipo do serviço HTTP
        // método de atualização do vendedor
        public IActionResult Put(VendedorDTO body) // recebe como parametro um objeto do tipo Vendedor
        {
            try
            {
                _vendedorService.Update(body); // atualiza vendedor
                return Ok("Vendedor atualizado com sucesso!"); // retorna mensagem de sucesso ao usuario
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // retorna mensagem de erro ao usuario
            }
        }

        [HttpDelete()] // define tipo do serviço HTTP
        // método que deleta um vendedor
        public IActionResult Delete(long idVendedor) // recebe como parametro o id do vendedor
        {
            try
            {
                _vendedorService.Delete(idVendedor); // deleta vendedor
                return Ok("Vendedor deletado com sucesso!"); // retorna mensagem de sucesso ao usuário
            }
            catch (Exception e)
            {
                return NotFound(e.Message); // retorna mensagem de erro ao usuário
            }
        }

        [HttpPost("export-all")] // define tipo do serviço HTTP e complemento da rota
        public IActionResult ExportSeller(long? idVendedor, string? nomeVendedor) // recebe como parametro dados do filtro, caso informado
        {
            var content = _vendedorService.ExportSeller(idVendedor, nomeVendedor); // monta documento para download
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Sellers.xlsx"); // retorna o documento para ser feito o download
        }
    }
}