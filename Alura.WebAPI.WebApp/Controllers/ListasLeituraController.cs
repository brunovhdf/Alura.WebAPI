using System.Linq;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/ListasLeitura")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;
        public ListasLeituraController(IRepository<Livro> repository)
        {
            _repo = repository;
        }
        
        [HttpGet]
        public IActionResult TodasListas()
        {
            var lista = _repo.All.GroupBy(x => x.Lista).ToList();
            if(lista.Count > 0)
            {
                return Ok(lista);
            }
            return NotFound("Nenhum livro cadastrado");
        }

        [HttpGet]
        [Route("{tipo}")]
        public IActionResult ListaEspecifica(TipoListaLeitura tipo)
        {
            var lista = _repo.All.Where(x => x.Lista == tipo).ToList();
            if (lista.Count > 0)
            {
                return Ok(lista);
            }
            return NotFound("Nenhum livro cadastrado na lista " + tipo);
        }
    }
}
