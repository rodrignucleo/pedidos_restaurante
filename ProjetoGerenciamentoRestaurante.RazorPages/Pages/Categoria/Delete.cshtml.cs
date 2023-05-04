using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Categoria
{
    public class Delete : PageModel
    {
        [BindProperty]
        public CategoriaModel CategoriaModel { get; set; } = new();
        public Delete(){
        }

        public async Task<IActionResult> OnGetAsync(int? id){
            if(id == null){
                return NotFound();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Categoria/Details/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            CategoriaModel = JsonConvert.DeserializeObject<CategoriaModel>(content)!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id){
            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Categoria/Delete/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await httpClient.SendAsync(requestMessage);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Categoria/Index");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                return Page();
            }
        }
    }
}