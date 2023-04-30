using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GarcomController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Get(
            [FromServices] AppDbContext context) => 
                Ok( context.Garcon!.ToList());

        [HttpGet("/{id:int}")]
        public IActionResult GetById([FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var garconModel = context.Garcon!.FirstOrDefault(x => x.GarconId == id);
            if (garconModel == null) {
                return NotFound();
            }

            return Ok(garconModel);
        }

        [HttpPost("/")]
        public IActionResult Post([FromBody] GarconModel garconModel,
            [FromServices] AppDbContext context)
        {
            context.Garcon!.Add(garconModel);
            context.SaveChanges();
            return Created($"/{garconModel.GarconId}", garconModel);
        }

        [HttpPut("/")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] GarconModel garconModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Garcon!.FirstOrDefault(x => x.GarconId == id);
            if (model == null) {
                return NotFound();
            }

            model.Nome = garconModel.Nome;
            model.Sobrenome = garconModel.Sobrenome;
            model.Cpf = garconModel.Cpf;
            model.Telefone = garconModel.Telefone;

            context.Garcon!.Update(model);
            context.SaveChanges();
            return Ok(model);
        }

        [HttpDelete("/")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Garcon!.FirstOrDefault(x => x.GarconId == id);
            if (model == null) {
                return NotFound();
            }

            context.Garcon!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }
    }
}