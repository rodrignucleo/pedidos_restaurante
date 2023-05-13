using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoGerenciamentoRestaurante.API.Models;

namespace ProjetoGerenciamentoRestaurante.API.Controllers.Pedido
{
    [ApiController]
    [Route("api/[controller]")]
    public class Pedido_ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route("/Pedido_Produto/{id:int}")]
        public IActionResult Get([FromRoute] int id, [FromServices] AppDbContext context){
            var pedido_Produtos = context.Pedido_Produto!
                .Include(p => p.Pedido)
                    .ThenInclude(p => p!.Garcon)
                .Include(p => p.Pedido)
                    .ThenInclude(p => p!.Atendimento)
                        .ThenInclude(a => a!.Mesa)
                .Include(p => p.Produto)
                .Where(e => e.Pedido!.Atendimento!.AtendimentoId  == id)
            .ToList();

            return Ok(pedido_Produtos);
        }

        [HttpPut("/Pedido_Produto/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] AtendimentoModel atendimentoModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Atendimento!.Include(p => p.Mesa).FirstOrDefault(x => x.AtendimentoId == id);

            if (model == null) {
                return NotFound();
            }
            if(model.Mesa!.Status && model.MesaId != model.Mesa.MesaId){
                return RedirectToPage("/Atendimento/Details/"+id);
            }
            else{
                if(model.AtendimentoFechado){
                    model.DataSaida = null;
                    model.AtendimentoFechado = false;
                    model.Mesa!.Status = true;
                    model.Mesa.HoraAbertura = DateTime.Now.AddHours(1);
                }
                else{
                    model.DataSaida = DateTime.Now;
                    model.AtendimentoFechado = true;
                    model.Mesa!.Status = false;
                    model.Mesa!.HoraAbertura = null;
                }
                
                context.Atendimento!.Update(model);
                context.SaveChanges();
                
                return Ok(model);
            }
        }

        [HttpPost("/Pedido/Create")]
        public IActionResult Post([FromBody] PedidoModel pedidoModel,
            [FromServices] AppDbContext context)
        {
            context.Pedido!.Add(pedidoModel);
            context.SaveChanges();
            return Created($"/{pedidoModel.PedidoId}", pedidoModel);
        }

        [HttpPost("/Pedido_Produto/Create/{id:int}")]
        public IActionResult Post([FromRoute] int id, [FromBody] Pedido_ProdutoModel pedido_ProdutoModel,
            [FromServices] AppDbContext context)
        {
            pedido_ProdutoModel.PedidoId = id;
            context.Pedido_Produto!.Add(pedido_ProdutoModel);
            context.SaveChanges();
            return Created($"/{pedido_ProdutoModel.PedidoProdutoId}", pedido_ProdutoModel);
        }
    }
}