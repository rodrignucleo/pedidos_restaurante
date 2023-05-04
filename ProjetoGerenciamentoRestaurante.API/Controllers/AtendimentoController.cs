using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtendimentoController : ControllerBase
    {
        [HttpGet]
        [Route("/Atendimento")]
        public IActionResult Get([FromServices] AppDbContext context){
            var atendimentos = context.Atendimento!.Include(p => p.Mesa).ToList();
            return Ok(atendimentos);
        }
        
        [HttpGet("/Atendimento/Details/{id:int}")]
        public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var atendimentoModel = context.Atendimento!.Include(p => p.Mesa).FirstOrDefault(x => x.AtendimentoId == id);
            if (atendimentoModel == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                AtendimentoId = atendimentoModel.AtendimentoId,
                AtendimentoFechado = atendimentoModel.AtendimentoFechado,
                DataSaida = atendimentoModel.DataSaida,
                MesaId = atendimentoModel.MesaId,
                Mesa = new
                {
                    MesaId = atendimentoModel.Mesa!.MesaId,
                    Numero = atendimentoModel.Mesa.Numero,
                    HoraAbertura = atendimentoModel.Mesa.HoraAbertura
                }
            });
        }
/*
        [HttpPost("/Atendimento/Create")]
        public IActionResult Post([FromBody] AtendimentoModel atendimentoModel,
            [FromServices] AppDbContext context)
        {
            context.Atendimento!.Add(atendimentoModel);
            context.SaveChanges();
            return Created($"/{atendimentoModel.AtendimentoId}", atendimentoModel);
        }
*/
        [HttpPut("/Atendimento/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] AtendimentoModel atendimentoModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Atendimento!.Include(p => p.Mesa).FirstOrDefault(x => x.AtendimentoId == id);
            if (model == null) {
                return NotFound();
            }

            model.MesaId = atendimentoModel.MesaId;

            context.Atendimento!.Update(model);
            context.SaveChanges();
            return Ok(model);
        }
/*
        [HttpDelete("/Atendimento/Delete/{id:int}")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Atendimento!.FirstOrDefault(x => x.AtendimentoId == id);
            if (model == null) {
                return NotFound();
            }

            context.Atendimento!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }*/
    }
}