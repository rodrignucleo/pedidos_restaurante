using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Categoria
{
    public class Create : PageModel
    {
        [BindProperty]
        public CategoriaModel CategoriaModel { get; set; } = new();
        public Create(){
        }

        public async Task<IActionResult> OnPostAsync(int id){
            if(!ModelState.IsValid){
                return Page();
            }
            
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Categoria/Create";
            var categoriaJson = JsonConvert.SerializeObject(CategoriaModel);
            var content = new StringContent(categoriaJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            
            if(response.IsSuccessStatusCode){
                return RedirectToPage("/Categoria/Index");
            } else {
                return Page();
            }
        }
    }
}