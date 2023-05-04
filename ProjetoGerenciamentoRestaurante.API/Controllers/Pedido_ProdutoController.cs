using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
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
/*
        [HttpGet("/Pedido_Produto/Details/{id:int}")]
        public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var pedido_ProdutoModel = context.Pedido_Produto!.Include(p => p.Pedido)
                    .ThenInclude(p => p!.Garcon)
                .Include(p => p.Pedido)
                    .ThenInclude(p => p!.Atendimento)
                        .ThenInclude(a => a!.Mesa)
                .Include(p => p.Produto)
                .Where(e => e.Pedido!.Atendimento!.AtendimentoId == id)
                .FirstOrDefault(x => x.PedidoProdutoId == id);
            if (pedido_ProdutoModel == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Id = pedido_ProdutoModel.PedidoProdutoId,
                PedidoId = pedido_ProdutoModel.PedidoId,
                ProdutoId = pedido_ProdutoModel.ProdutoId,
                Quantidade = pedido_ProdutoModel.Quantidade,
                Pedido = new
                {
                    Id = pedido_ProdutoModel.Pedido!.PedidoId,
                    AtendimentoId = pedido_ProdutoModel.Pedido.AtendimentoId,
                    GarconId = pedido_ProdutoModel.Pedido.GarconId,
                },
                Garcon = new
                {
                    Id = pedido_ProdutoModel.Pedido.Garcon!.GarconId,
                    Nome = pedido_ProdutoModel.Pedido.Garcon!.Nome,
                    Sobrenome = pedido_ProdutoModel.Pedido.Garcon!.Sobrenome
                },
                Produto = new
                {
                    Id = pedido_ProdutoModel.Produto!.ProdutoId,
                    Nome = pedido_ProdutoModel.Produto.Nome,
                    Descricao = pedido_ProdutoModel.Produto.Descricao,
                    Preco = pedido_ProdutoModel.Produto.Preco
                }
            });
        }

        [HttpPost("/Pedido_Produto/Create")]
        public IActionResult Post([FromBody] Pedido_ProdutoModel pedido_ProdutoModel,
            [FromServices] AppDbContext context)
        {
            context.Pedido_Produto!.Add(pedido_ProdutoModel);
            context.SaveChanges();
            return Created($"/{pedido_ProdutoModel.Pedido_ProdutoId}", pedido_ProdutoModel);
        }

        [HttpPut("/Pedido_Produto/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] Pedido_ProdutoModel pedido_ProdutoModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Pedido_Produto!.Include(p => p.Categoria).FirstOrDefault(x => x.Pedido_ProdutoId == id);
            if (model == null) {
                return NotFound();
            }

            model.Nome = pedido_ProdutoModel.Nome;
            model.Descricao = pedido_ProdutoModel.Descricao;
            model.Preco = pedido_ProdutoModel.Preco;
            model.CategoriaId = pedido_ProdutoModel.CategoriaId;

            context.Pedido_Produto!.Update(model);
            context.SaveChanges();
            return Ok(model);
        }

        [HttpDelete("/Pedido_Produto/Delete/{id:int}")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Pedido_Produto!.FirstOrDefault(x => x.Pedido_ProdutoId == id);
            if (model == null) {
                return NotFound();
            }

            context.Pedido_Produto!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }*/
    }
}