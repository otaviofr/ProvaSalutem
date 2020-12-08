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

        [HttpPost()]
        public IActionResult Post(ClienteDTO body)
        {
            try
            {
                _clienteService.Save(body);
                return Ok("Customer successfully registered!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet()]
        public IEnumerable<ClienteViewModel> GetById(long idCliente, int skip, int limit)
        {
            IEnumerable<ClienteViewModel> clienteViewModel = null;

            clienteViewModel = _clienteService.GetById(idCliente, skip, limit);

            return clienteViewModel;
        }
    }
}