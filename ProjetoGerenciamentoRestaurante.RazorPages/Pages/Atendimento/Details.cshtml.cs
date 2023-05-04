using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Atendimento
{
    public class Details : PageModel
    {
        [BindProperty]
        public AtendimentoModel AtendimentoModel { get; set; } = new();
        public MesaModel MesaModel { get; set; } = new();
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

        public async Task<IActionResult> OnPostAsync(int? id){
            if(!ModelState.IsValid){
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Mesa/Edit/{id}";
            var mesaJson = JsonConvert.SerializeObject(MesaModel);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            requestMessage.Content = new StringContent(mesaJson, Encoding.UTF8, "application/json");
            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return RedirectToPage("/Mesa/Index");
            }
            var httpClientAtendimento = new HttpClient();
            var urlAtendimento = $"http://localhost:5171/Atendimento/Edit/{id}";
            var atendimentoJson = JsonConvert.SerializeObject(AtendimentoModel);

            var requestMessageAtendimento = new HttpRequestMessage(HttpMethod.Put, url);
            requestMessageAtendimento.Content = new StringContent(atendimentoJson, Encoding.UTF8, "application/json");
            var responseAtendimento = await httpClient.SendAsync(requestMessageAtendimento);

            if(!response.IsSuccessStatusCode){
                return RedirectToPage("/Atendimento/Index");
            }
            
            return Page();

            /*if(!ModelState.IsValid){
                return Page();
            }
            var atendimentoToUpdate = await _context.Atendimento!.FindAsync(id);

            if(atendimentoToUpdate == null){
                return NotFound();
            }
            
            if(atendimentoToUpdate.AtendimentoFechado){
                var mesaAtualId = atendimentoToUpdate.MesaId;

                atendimentoToUpdate.AtendimentoFechado = false;
                atendimentoToUpdate.DataSaida = null;

                var mesaAtual = await _context.Mesa!.FindAsync(mesaAtualId);
                mesaAtual!.Status = true;
                mesaAtual.HoraAbertura = DateTime.Now.AddHours(1);
            
                try{
                    _context.Update(mesaAtual);
                    _context.Update(atendimentoToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Atendimento/Index");
                } catch(DbUpdateException){
                    return Page();
                }
            }
            else{
                var mesaAtualId = atendimentoToUpdate.MesaId;

                atendimentoToUpdate.AtendimentoFechado = true;
                atendimentoToUpdate.DataSaida = DateTime.Now.AddHours(3);

                var mesaAtual = await _context.Mesa!.FindAsync(mesaAtualId);
                mesaAtual!.Status = false;
                mesaAtual.HoraAbertura = null;
            
                try{
                    _context.Update(mesaAtual);
                    _context.Update(atendimentoToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Atendimento/Index");
                } catch(DbUpdateException){
                    return Page();
                }
            }*/
        }

    }
}