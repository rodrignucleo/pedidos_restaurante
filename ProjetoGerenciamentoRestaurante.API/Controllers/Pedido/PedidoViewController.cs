using ProjetoGerenciamentoRestaurante.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoGerenciamentoRestaurante.API.Models;

namespace ProjetoGerenciamentoRestaurante.API.Controllers.Pedido
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class PedidoViewController : ControllerBase
    {
        public List<PedidoViewModel>pedidoViewList { get; set; } = new ();

        [HttpGet]
        [Route("/PedidoView")]

        public IActionResult Get([FromServices] AppDbContext context){
            var pedidoList = context.Pedido!
                            .Include(g => g.Garcon)
                            .Include(a => a.Atendimento)
                            .Include(m => m.Atendimento!.Mesa)
                            .ToList();
            var pedidoProdutoList = context.Pedido_Produto!
                            .Include(p => p.Pedido)
                            .Include(x => x.Produto)
                            .ToList();
            var garconList = context.Garcon!.ToList();

            foreach (var g in garconList){
                PedidoViewModel pedido = new();
                pedido.Garcon = g;
                pedidoViewList.Add(pedido);
            }

            foreach (var p in pedidoList){
                foreach (var v in pedidoViewList) {
                    if(p.GarconId == v.Garcon!.GarconId){
                        v.countPedidos += 1;
                    }
                }
            }

            foreach (var p in pedidoProdutoList){
                foreach (var v in pedidoViewList){
                    if(p.Pedido!.GarconId == v.Garcon!.GarconId){
                        var total = p.Produto!.Preco * p.Quantidade;
                        v.totalVendas += total;
                        v.quantidadeTotal += p.Quantidade;
                    }
                }
            }

            return Ok(pedidoViewList);
        }
    }
}