using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Mesa
{
    public class Edit : PageModel
    {
        [BindProperty]
        public MesaModel MesaModel { get; set; } = new();
        public Edit(){
        }

        public async Task<IActionResult> OnGetAsync(int? id){
            if(id == null){
                return NotFound();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Mesa/Details/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            MesaModel = JsonConvert.DeserializeObject<MesaModel>(content)!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id){
            if(!ModelState.IsValid){
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Mesa/Edit/{id}";
            var mesaJson = JsonConvert.SerializeObject(MesaModel);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            requestMessage.Content = new StringContent(mesaJson, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if(MesaModel.HoraAbertura == null){
                TempData["Mensagem"] = "Insira a Hora de Abertura da Mesa!!";
                return Page();
            }

            if(!response.IsSuccessStatusCode){
                return Page();
            }

            return RedirectToPage("/Mesa/Index");
        }
    }
}