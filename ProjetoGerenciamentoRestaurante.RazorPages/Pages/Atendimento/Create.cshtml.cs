using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Atendimento
{
    public class Create : PageModel
    {
        [BindProperty]
        public AtendimentoModel AtendimentoModel { get; set; } = new();
        public List<MesaModel> MesaList { get; set; } = new();
        public Create(){
        }

        public async Task<IActionResult> OnGetAsync(){
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Mesa";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            MesaList = JsonConvert.DeserializeObject<List<MesaModel>>(content)!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToPage("/Atendimento/Create");
            }

            var httpClient = new HttpClient();
            var atendimentoJson = JsonConvert.SerializeObject(AtendimentoModel);
            var content = new StringContent(atendimentoJson, Encoding.UTF8, "application/json");
            var atendimentoResponse = await httpClient.PostAsync("http://localhost:5171/Atendimento/Create", content);

            if (atendimentoResponse.IsSuccessStatusCode)
            {
                return RedirectToPage("/Atendimento/Index");
            } 
            else 
            {
                TempData["Aviso_Alocar_Mesa"] = "A mesa já está ocupada!!";
                return RedirectToPage("/Atendimento/Create");
            }
        }
        }
}
