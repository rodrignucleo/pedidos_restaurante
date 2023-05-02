using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesaController : ControllerBase
    {
        [HttpGet]
        [Route("/Mesa")]
        public IActionResult Get(
            [FromServices] AppDbContext context) => 
                Ok( context.Mesa!.ToList());

        [HttpGet("/Mesa/Details/{id:int}")]
        public IActionResult GetById([FromRoute] int id,
            [FromServices] AppDbContext context)
        {
            var mesaModel = context.Mesa!.FirstOrDefault(x => x.MesaId == id);
            if (mesaModel == null) {
                return NotFound();
            }

            return Ok(mesaModel);
        }

        [HttpPost("/Mesa/Create")]
        public IActionResult Post([FromBody] MesaModel mesaModel,
            [FromServices] AppDbContext context)
        {
            context.Mesa!.Add(mesaModel);
            context.SaveChanges();
            return Created($"/{mesaModel.MesaId}", mesaModel);
        }

        [HttpPut("/Mesa/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, [FromBody] MesaModel mesaModel, [FromServices] AppDbContext context)
        {
            var model = context.Mesa!.FirstOrDefault(x => x.MesaId == id);
            if (model == null) {
                return NotFound();
            }

            model.Numero = mesaModel.Numero;
            model.Status = mesaModel.Status;
            model.HoraAbertura = mesaModel.Status ? mesaModel.HoraAbertura : null;

            try{
                context.Mesa!.Update(model);
                context.SaveChanges(); 
                return Ok(model);
            } catch(DbUpdateException){
                return Ok(model);
            }
        }

        [HttpDelete("/Mesa/Delete/{id:int}")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Mesa!.FirstOrDefault(x => x.MesaId == id);
            if (model == null) {
                return NotFound();
            }

            context.Mesa!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }
    }
}