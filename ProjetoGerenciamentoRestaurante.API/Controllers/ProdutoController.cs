using ProjetoGerenciamentoRestaurante.API.Models;
using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjetoGerenciamentoRestaurante.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [Route("/Produto")]
        public IActionResult Get([FromServices] AppDbContext context){
            var produtos = context.Produto!.Include(p => p.Categoria).ToList();
            return Ok(produtos);
        }

        [HttpGet("/Produto/Details/{id:int}")]
        public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var produtoModel = context.Produto!.Include(p => p.Categoria).FirstOrDefault(x => x.ProdutoId == id);
            if (produtoModel == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Id = produtoModel.ProdutoId,
                Nome = produtoModel.Nome,
                Preco = produtoModel.Preco,
                Descricao = produtoModel.Descricao,
                Categoria = new
                {
                    Id = produtoModel.Categoria!.CategoriaId,
                    Nome = produtoModel.Categoria.Nome,
                    Descricao = produtoModel.Categoria.Descricao
                }
            });
        }

        [HttpPost("/Produto/Create")]
        public IActionResult Post([FromBody] ProdutoModel produtoModel,
            [FromServices] AppDbContext context)
        {
            context.Produto!.Add(produtoModel);
            context.SaveChanges();
            return Created($"/{produtoModel.ProdutoId}", produtoModel);
        }

        [HttpPut("/Produto/Edit/{id:int}")]
        public IActionResult Put([FromRoute] int id, 
            [FromBody] ProdutoModel produtoModel,
            [FromServices] AppDbContext context)
        {
            var model = context.Produto!.Include(p => p.Categoria).FirstOrDefault(x => x.ProdutoId == id);
            if (model == null) {
                return NotFound();
            }

            model.Nome = produtoModel.Nome;
            model.Descricao = produtoModel.Descricao;
            model.Preco = produtoModel.Preco;
            model.CategoriaId = produtoModel.CategoriaId;

            context.Produto!.Update(model);
            context.SaveChanges();
            return Ok(model);
        }

        [HttpDelete("/Produto/Delete/{id:int}")]
        public IActionResult Delete([FromRoute] int id, 
            [FromServices] AppDbContext context)
        {
            var model = context.Produto!.FirstOrDefault(x => x.ProdutoId == id);
            if (model == null) {
                return NotFound();
            }

            context.Produto!.Remove(model);
            context.SaveChanges();
            return Ok(model);
        }
    }
}