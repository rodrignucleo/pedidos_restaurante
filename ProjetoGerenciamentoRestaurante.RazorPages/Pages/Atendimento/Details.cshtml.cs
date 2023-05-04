using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Data;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Atendimento
{
    public class Details : PageModel
    {
        [BindProperty]
        public AtendimentoModel AtendimentoModel { get; set; } = new();
        public List<Pedido_ProdutoModel> Pedido_ProdutoList { get; set; } = new();
        
        public Details(){
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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
            // AtendimentoModel = JsonConvert.DeserializeObject<dynamic>(contentAtendimento)!;
            AtendimentoModel = JsonConvert.DeserializeObject<AtendimentoModel>(contentAtendimento)!;
            

            // Obter informações dos pedidos
            var httpClientPedido = new HttpClient();
            var urlPedido = $"http://localhost:5171/Pedido_Produto/{id}";
            var responsePedido = await httpClientPedido.GetAsync(urlPedido);

            if (!responsePedido.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var contentPedido = await responsePedido.Content.ReadAsStringAsync();
            var pedido_ProdutoList = JsonConvert.DeserializeObject<List<Pedido_ProdutoModel>>(contentPedido);


            Pedido_ProdutoList = pedido_ProdutoList!;

            return Page();
        }

      /*  public async Task<IActionResult> OnGetAsync(int? id){
            if(id == null){
                return NotFound();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Atendimento/Details/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            AtendimentoModel = JsonConvert.DeserializeObject<AtendimentoModel>(content)!;

            var httpClientPedido = new HttpClient();
            var urlPedido = $"http://localhost:5171/Pedido_Produto/{id}";
            var requestMessagePedido = new HttpRequestMessage(HttpMethod.Get, urlPedido);
            var responsePedido = await httpClient.SendAsync(requestMessagePedido);
            var contentPedido = await response.Content.ReadAsStringAsync();

            Pedido_ProdutoList = JsonConvert.DeserializeObject<List<Pedido_ProdutoModel>>(contentPedido)!;

            return Page();
        }
*/
        // public async Task<IActionResult> OnPostAsync(int? id){
        //     if(!ModelState.IsValid){
        //         return Page();
        //     }
        //     var atendimentoToUpdate = await _context.Atendimento!.FindAsync(id);

        //     if(atendimentoToUpdate == null){
        //         return NotFound();
        //     }
            
        //     if(atendimentoToUpdate.AtendimentoFechado){
        //         var mesaAtualId = atendimentoToUpdate.MesaId;

        //         atendimentoToUpdate.AtendimentoFechado = false;
        //         atendimentoToUpdate.DataSaida = null;

        //         var mesaAtual = await _context.Mesa!.FindAsync(mesaAtualId);
        //         mesaAtual!.Status = true;
        //         mesaAtual.HoraAbertura = DateTime.Now.AddHours(1);
            
        //         try{
        //             _context.Update(mesaAtual);
        //             _context.Update(atendimentoToUpdate);
        //             await _context.SaveChangesAsync();
        //             return RedirectToPage("/Atendimento/Index");
        //         } catch(DbUpdateException){
        //             return Page();
        //         }
        //     }
        //     else{
        //         var mesaAtualId = atendimentoToUpdate.MesaId;

        //         atendimentoToUpdate.AtendimentoFechado = true;
        //         atendimentoToUpdate.DataSaida = DateTime.Now.AddHours(3);

        //         var mesaAtual = await _context.Mesa!.FindAsync(mesaAtualId);
        //         mesaAtual!.Status = false;
        //         mesaAtual.HoraAbertura = null;
            
        //         try{
        //             _context.Update(mesaAtual);
        //             _context.Update(atendimentoToUpdate);
        //             await _context.SaveChangesAsync();
        //             return RedirectToPage("/Atendimento/Index");
        //         } catch(DbUpdateException){
        //             return Page();
        //         }
        //     }
        // }

    }
}