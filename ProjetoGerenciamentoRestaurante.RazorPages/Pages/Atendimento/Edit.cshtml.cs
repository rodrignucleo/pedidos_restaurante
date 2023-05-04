using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Atendimento
{
    public class Edit : PageModel
    {
        [BindProperty]
        public AtendimentoModel AtendimentoModel { get; set; } = new();
        public MesaModel MesaModel { get; set; } = new();
        public List<MesaModel> MesaList { get; set; } = new();
        public Edit(){
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
            
            var httpClientMesa = new HttpClient();
            var urlMesa = "http://localhost:5171/Mesa";
            var requestMessageMesa = new HttpRequestMessage(HttpMethod.Get, urlMesa);
            var responseMesa = await httpClientMesa.SendAsync(requestMessageMesa);
            var contentMesa = await responseMesa.Content.ReadAsStringAsync();

            MesaList = JsonConvert.DeserializeObject<List<MesaModel>>(contentMesa)!;

            return Page();
        }

/*
        public async Task<IActionResult> OnPostAsync(int id){
            if(!ModelState.IsValid){
                return Page();
            }
            var atendimentoToUpdate = await _context.Atendimento!.FindAsync(id);

            if(atendimentoToUpdate == null){
                return NotFound();
            }
            
            var mesaAntigaId = atendimentoToUpdate.MesaId;

            atendimentoToUpdate.MesaId = AtendimentoModel.MesaId;

            var mesaAntiga = await _context.Mesa!.FindAsync(mesaAntigaId);
            mesaAntiga!.Status = false;
            mesaAntiga.HoraAbertura = null;

            var mesaNova = await _context.Mesa!.FindAsync(AtendimentoModel.MesaId);
            mesaNova!.Status = true;
            mesaNova.HoraAbertura = DateTime.Now.AddHours(2);

        
            try{
                bool mesaOcupada = await _context.Mesa!.AnyAsync(m => m.MesaId == AtendimentoModel.MesaId && m.Status);
                if (mesaOcupada) {
                    // ModelState.AddModelError(string.Empty, "A mesa já está ocupada!");
                    TempData["Mensagem"] = "A mesa já está ocupada!!";
                    return RedirectToPage("/Atendimento/Create");
                }
                _context.Update(mesaAntiga);
                _context.Update(mesaNova);
                _context.Update(atendimentoToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("/Atendimento/Index");
            } catch(DbUpdateException){
                return Page();
            }
        }*/
    }
}