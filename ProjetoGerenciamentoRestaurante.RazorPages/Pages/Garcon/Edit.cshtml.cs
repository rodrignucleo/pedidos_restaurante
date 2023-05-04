using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Garcon
{
    public class Edit : PageModel
    {
        // private readonly AppDbContext _context;
        [BindProperty]
        public GarconModel GarconModel { get; set; } = new();
        // public Edit(AppDbContext context){
            // _context = context;
        // }
        public Edit(){
        }

        public async Task<IActionResult> OnGetAsync(int? id){
            // if(id == null || _context.Garcon == null){
            //     return NotFound();
            // }

            // var garconModel = await _context.Garcon.FirstOrDefaultAsync(e => e.GarconId == id);
            // if(garconModel == null){
            //     return NotFound();
            // }
            // GarconModel = garconModel;
            // return Page();
            if(id == null){
                return NotFound();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Garcon/Details/{id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            GarconModel = JsonConvert.DeserializeObject<GarconModel>(content)!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id){
            // if(!ModelState.IsValid){
            //     return Page();
            // }

            // var garconToUpdate = await _context.Garcon!.FindAsync(id);

            // if(garconToUpdate == null){
            //     return NotFound();
            // }

            // garconToUpdate.Nome = GarconModel.Nome;
            // garconToUpdate.Sobrenome = GarconModel.Sobrenome;
            // garconToUpdate.Cpf = GarconModel.Cpf;
            // garconToUpdate.Telefone = GarconModel.Telefone;

            // try{
            //     await _context.SaveChangesAsync();
            //     return RedirectToPage("/Garcon/Index");
            // } catch(DbUpdateException){
            //     return Page();
            // }

            if(!ModelState.IsValid){
                return Page();
            }

            var httpClient = new HttpClient();
            var url = $"http://localhost:5171/Garcon/Edit/{id}";
            var garconJson = JsonConvert.SerializeObject(GarconModel);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            requestMessage.Content = new StringContent(garconJson, Encoding.UTF8, "application/json");

            var response = await httpClient.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode){
                return Page();
            }

            return RedirectToPage("/Garcon/Index");
        }
    }
}