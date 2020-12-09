using Microsoft.AspNetCore.Mvc;
using System;
using ApiProvaSalutem.Services;
using ApiProvaSalutem.DTO;
using System.Collections.Generic;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Controllers
{
    [ApiController]
    [Route("cliente")] // define rota pricipal
    public class ClienteController : ControllerBase // define herança
    {
        private readonly IClienteService _clienteService; // instancia a interface ClienteService

        public ClienteController(IClienteService clienteService) // construtor ClienteController
        {
            _clienteService = clienteService;
        }

        [HttpGet()] // define tipo do serviço HTTP
        //método que busca um único cliente
        public IEnumerable<ClienteViewModel> GetById(long idCliente) // recebe como parametro o id do cliente desejado
        {
            IEnumerable<ClienteViewModel> clienteViewModel = null; // cria um objeto null para receber os dados buscados no banco

            clienteViewModel = _clienteService.GetById(idCliente); // busca e atribui resultado obtido do banco a variável

            return clienteViewModel; // retorna dados para o usuário
        }

        [HttpGet("all")] // define tipo do serviço HTTP e complemento da rota
        //método que buscae todos clientes
        public IEnumerable<ClienteViewModel> Get(int skip = 0, int limit = 50) // os parametros skip e limit definem paginação com padrão 0 e 50 respectivamente
        {
            return _clienteService.GetAll(skip, limit); // retorna dados para o usuário
        }

        [HttpPost()] // define tipo do serviço HTTP
        //método que cadastra cliente
        public IActionResult Post(ClienteDTO body) // recebe como parametro um objeto do tipo Cliente
        {
            try
            {
                _clienteService.Save(body); // cadastra cliente
                return Ok("Cliente cadastrado com sucesso!"); // retorna mensagem de sucesso ao usuario
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // retorna mensagem de erro ao usuario
            }
        }

        [HttpPut()] // define tipo do serviço HTTP
        // método de atualização do cliente
        public IActionResult Put(ClienteDTO body) // recebe como parametro um objeto do tipo Cliente
        {
            try
            {
                _clienteService.Update(body); // atualiza cliente
                return Ok("Cliente atualizado com sucesso!"); // retorna mensagem de sucesso ao usuario
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); // retorna mensagem de erro ao usuario
            }
        }

        [HttpDelete()] // define tipo do serviço HTTP
        // método que deleta um cliente
        public IActionResult Delete(long idCliente) // recebe como parametro o id do cliente
        {
            try
            {
                _clienteService.Delete(idCliente); // deleta cliente
                return Ok("Cliente deletado com sucesso!"); // retorna mensagem de sucesso ao usuário
            }
            catch (Exception e)
            {
                return NotFound(e.Message); // retorna mensagem de erro ao usuário
            }
        }

        [HttpPost("export-all")] // define tipo do serviço HTTP e complemento da rota
        public IActionResult ExportCostumer(long? idCliente, string razaoSocial) // recebe como parametro dados do filtro, caso informado
        {
            var content = _clienteService.ExportCostumer(idCliente, razaoSocial); // monta documento para download
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Costumers.xlsx"); // retorna o documento para ser feito o download
        }
    }
}