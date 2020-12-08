using Microsoft.AspNetCore.Mvc;
using System;
using ApiProvaSalutem.Services;
using ApiProvaSalutem.DTO;
using System.Collections.Generic;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet()]
        public IEnumerable<ClienteViewModel> GetById(long idCliente, int skip, int limit)
        {
            IEnumerable<ClienteViewModel> clienteViewModel = null;

            clienteViewModel = _clienteService.GetById(idCliente, skip, limit);

            return clienteViewModel;
        }

        [HttpGet("all")]
        public IEnumerable<ClienteViewModel> Get(int skip = 0, int limit = 50)
        {
            return _clienteService.GetAll(skip, limit);
        }

        [HttpPost()]
        public IActionResult Post(ClienteDTO body)
        {
            try
            {
                _clienteService.Save(body);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut()]
        public IActionResult Put(ClienteDTO body)
        {
            try
            {
                _clienteService.Update(body);
                return Ok("Cliente atualizado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete()]
        public IActionResult Delete(long idCliente)
        {
            try
            {
                _clienteService.Delete(idCliente);
                return Ok("Cliente deletado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}