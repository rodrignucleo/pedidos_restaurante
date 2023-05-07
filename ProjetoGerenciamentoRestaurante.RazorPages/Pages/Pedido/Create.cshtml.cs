using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Pedido
{
    public class Create : PageModel
    {
        public AtendimentoModel AtendimentoModel { get; set; } = new();
        [BindProperty]
        public PedidoModel PedidoModel { get; set; } = new();

        [BindProperty]
        public Pedido_ProdutoModel Pedido_ProdutoModel { get; set; } = new();
        public List<GarconModel> GarconList { get; set; } = new();
        public List<ProdutoModel> ProdutoList { get; set; } = new();
        // public List<Pedido_ProdutoModel> Pedido_ProdutoList { get; set; } = new();
        
        public Create(){
        }

        public async Task<IActionResult> OnGetAsync(int? id){
            if (id == null)
            {
                return NotFound();
            }

            // Obter informações do atendimento
            var httpClientAtendimento = new HttpClient();
            var urlAtendimento = $"http://localhost:5171/Atendimento/Details/{id}";
            var responseAtendimento = await httpClientAtendimento.GetAsync(urlAtendimento);

            if (!responseAtendimento.IsSuccessStatusCode)
            {
                return NotFound();
            }
            var contentAtendimento = await responseAtendimento.Content.ReadAsStringAsync();
            AtendimentoModel = JsonConvert.DeserializeObject<AtendimentoModel>(contentAtendimento)!;
            
            // Pedido_ProdutoList = await _context.Pedido_Produto!.ToListAsync();
            
            var httpClientGarcon = new HttpClient();
            var urlGarcon = "http://localhost:5171/Garcon";
            var requestMessageGarcon = new HttpRequestMessage(HttpMethod.Get, urlGarcon);
            var responseGarcon = await httpClientGarcon.SendAsync(requestMessageGarcon);
            var contentGarcon = await responseGarcon.Content.ReadAsStringAsync();

            GarconList = JsonConvert.DeserializeObject<List<GarconModel>>(contentGarcon)!;

            var httpClientProduto = new HttpClient();
            var urlProduto = "http://localhost:5171/Produto";
            var requestMessageProduto = new HttpRequestMessage(HttpMethod.Get, urlProduto);
            var responseProduto = await httpClientProduto.SendAsync(requestMessageProduto);
            var contentProduto = await responseProduto.Content.ReadAsStringAsync();

            ProdutoList = JsonConvert.DeserializeObject<List<ProdutoModel>>(contentProduto)!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id){
            if(!ModelState.IsValid){
                return RedirectToAction("/Pedido/Create/"+id);
            }
            
            var httpClientPedido = new HttpClient();
            var urlPedido = "http://localhost:5171/Pedido/Create";
            var produtoJsonPedido = JsonConvert.SerializeObject(PedidoModel);
            var contentPedido = new StringContent(produtoJsonPedido, Encoding.UTF8, "application/json");
            var responsePedido = await httpClientPedido.PostAsync(urlPedido, contentPedido);

            if(responsePedido.IsSuccessStatusCode){
                string responseContent = await responsePedido.Content.ReadAsStringAsync();
                PedidoModel pedido = JsonConvert.DeserializeObject<PedidoModel>(responseContent)!;
                int pedidoId = pedido.PedidoId;

                var httpClientPedido_Produto = new HttpClient();
                var urlPedido_Produto = $"http://localhost:5171/Pedido_Produto/Create/{pedidoId}";
                var produtoJsonPedido_Produto = JsonConvert.SerializeObject(Pedido_ProdutoModel);

                var contentPedido_Produto = new StringContent(produtoJsonPedido_Produto, Encoding.UTF8, "application/json");
                var responsePedido_Produto = await httpClientPedido_Produto.PostAsync(urlPedido_Produto, contentPedido_Produto);
                if(responsePedido_Produto.IsSuccessStatusCode){
                    return Redirect("/Atendimento/Details/"+id);
                }
                else{
                    return Redirect("/");
                }
            } else {
                return Redirect("/Atendimento");
            }
/*
            try{
                _context.Pedido!.Add(PedidoModel);

                await _context.SaveChangesAsync();

                Pedido_ProdutoModel.PedidoId = PedidoModel.PedidoId;
                _context.Pedido_Produto!.Add(Pedido_ProdutoModel);

                await _context.SaveChangesAsync();
                return RedirectToPage("/Atendimento/Index");
                
            } catch(DbUpdateException){
                return RedirectToAction("/Pedido/Create/"+id);
            }*/
        }
    }
}