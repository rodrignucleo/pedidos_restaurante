using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Categoria
{
    public class Index : PageModel
    {
        public List<CategoriaModel> CategoriaList { get; set; } = new();
        public Index()
        {
        }

        public async Task<IActionResult> OnGetAsync(){
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Categoria";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            CategoriaList = JsonConvert.DeserializeObject<List<CategoriaModel>>(content)!;
            
            return Page();
        }
    }
}