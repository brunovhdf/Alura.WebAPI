using System.Linq;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.WebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/Livros")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;
        public LivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult ListaLivros()
        {
            var Livros = _repo.All;
            return Ok(Livros);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound("Livro não encontrado");
            }
            model.ImagemCapa = null;
            return Ok(model);
        }

        [HttpPost]
        public IActionResult Incluir([FromBody] LivroUpload model)
        {
            if(ModelState.IsValid)
            {
                var livro = model.ToLivro();
                _repo.Incluir(livro);
                var url = Url.Action("Recuperar", new { id = livro.Id });
                return Created(url,livro); //201
            }
            return BadRequest("Livro não cadastrado");
        }

        [HttpPut]
        public IActionResult Alterar([FromBody] LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                var livro = model.ToLivro();
                if(model.Capa == null)
                {
                    livro.ImagemCapa = _repo.All.
                        Where(x => x.Id == livro.Id).
                        Select(x => x.ImagemCapa).
                        FirstOrDefault();
                }
                _repo.Alterar(livro);
                return Ok(livro); //200
            }
            return BadRequest("Livro não alterado"); //400
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Excluir(int id)
        {
            var model = _repo.Find(id);
            if(model == null)
            {
                return NotFound("Não encontrado livro com id: " + id); //404
            }
            _repo.Excluir(model);
            return NoContent(); //204
        }
    }
}