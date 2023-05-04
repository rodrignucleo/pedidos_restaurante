using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Atendimento
{
    public class Index : PageModel
    {
        public List<AtendimentoModel> AtendimentoList { get; set; } = new();
        public Index(){
        }

        public async Task<IActionResult> OnGetAsync(){
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Atendimento";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            AtendimentoList = JsonConvert.DeserializeObject<List<AtendimentoModel>>(content)!;
            
            return Page();
        }
    }
}