using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [Route("/Categoria")]
        public IActionResult Get(
            [FromServices] AppDbContext context) => 
                Ok( context.Categoria!.ToList());

        [HttpGet("/Categoria/Details/{id:int}")]
        public IActionResult GetById([FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var categoriaModel = context.Categoria!.FirstOrDefault(x => x.CategoriaId == id);
            if (categoriaModel == null) {
                return NotFound();
            }

            return Ok(categoriaModel);
        }

        [HttpPost("/Categoria/Create")]
        public IActionResult Post([FromBody] CategoriaModel categoriaModel,
            [FromServices] AppDbContext context)
        {
            context.Categoria!.Add(categoriaModel);
            context.SaveChanges();
            return Created($"/{categoriaModel.CategoriaId}", categoriaModel);
        }

        [HttpPut("/Categoria/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] CategoriaModel categoriaModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Categoria!.FirstOrDefault(x => x.CategoriaId == id);
            if (model == null) {
                return NotFound();
            }

            model.Nome = categoriaModel.Nome;
            model.Descricao = categoriaModel.Descricao;

            context.Categoria!.Update(model);
            context.SaveChanges();
            return Ok(model);
        }

        [HttpDelete("/Categoria/Delete/{id:int}")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Categoria!.FirstOrDefault(x => x.CategoriaId == id);
            if (model == null) {
                return NotFound();
            }

            context.Categoria!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }
    }
}