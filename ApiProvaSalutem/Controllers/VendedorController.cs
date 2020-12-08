using Microsoft.AspNetCore.Mvc;
using System;
using ApiProvaSalutem.Services;
using ApiProvaSalutem.DTO;
using System.Collections.Generic;
using ApiProvaSalutem.ViewModel;

namespace ApiProvaSalutem.Controllers
{
    [ApiController]
    [Route("vendedor")]
    public class VendedorController : ControllerBase
    {
        private readonly IVendedorService _vendedorService;

        public VendedorController(IVendedorService vendedorService)
        {
            _vendedorService = vendedorService;
        }

        [HttpGet()]
        public IEnumerable<VendedorViewModel> GetById(long idVendedor, int skip, int limit)
        {
            IEnumerable<VendedorViewModel> vendedorViewModel = null;

            vendedorViewModel = _vendedorService.GetById(idVendedor, skip, limit);

            return vendedorViewModel;
        }

        [HttpGet("all")]
        public IEnumerable<VendedorViewModel> Get(int skip = 0, int limit = 50)
        {
            return _vendedorService.GetAll(skip, limit);
        }

        [HttpPost()]
        public IActionResult Post(VendedorDTO body)
        {
            try
            {
                _vendedorService.Save(body);
                return Ok("Vendedor cadastrado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut()]
        public IActionResult Put(VendedorDTO body)
        {
            try
            {
                _vendedorService.Update(body);
                return Ok("Vendedor atualizado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete()]
        public IActionResult Delete(long idVendedor)
        {
            try
            {
                _vendedorService.Delete(idVendedor);
                return Ok("Vendedor deletado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}