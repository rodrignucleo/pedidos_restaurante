using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Mesa
{
    public class Create : PageModel
    {
        [BindProperty]
        public MesaModel MesaModel { get; set; } = new();
        public Create(){
        }
        public async Task<IActionResult> OnPostAsync(){
            if(!ModelState.IsValid){
                return Page();
            }
            
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Mesa/Create";
            var mesaJson = JsonConvert.SerializeObject(MesaModel);
            var content = new StringContent(mesaJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            
            if(response.IsSuccessStatusCode){
                return RedirectToPage("/Mesa/Index");
            } else {
                return Page();
            }
        }
    }
}