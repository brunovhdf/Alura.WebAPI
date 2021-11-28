using System.Linq;
using Alura.ListaLeitura.Persistencia;
using Alura.ListaLeitura.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alura.WebAPI.WebApp.Controllers
{
    public class APILivrosController : Controller
    {
        private readonly IRepository<Livro> _repo;
        public APILivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound("Livro não encontrado");
            }
            model.ImagemCapa = null;
            return Json(model);
        }

        [HttpPost]
        public IActionResult Incluir(LivroUpload model)
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

        [HttpPost]
        public IActionResult Alterar(LivroUpload model)
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
            return BadRequest("Livro não alterado");
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var model = _repo.Find(id);
            if(model == null)
            {
                return NotFound("Não encontrado livro com id: " + id);
            }
            _repo.Excluir(model);
            return NoContent(); //203
        }
    }
}