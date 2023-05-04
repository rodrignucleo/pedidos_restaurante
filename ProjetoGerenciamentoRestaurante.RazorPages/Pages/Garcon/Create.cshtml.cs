using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Garcon
{
    public class Create : PageModel
    {
        // private readonly AppDbContext _context;
        [BindProperty]
        public GarconModel GarconModel { get; set; } = new();
        // public Create(AppDbContext context){
        //     _context = context;
        // }
        
        public Create(){
        }

        public async Task<IActionResult> OnPostAsync(int id){
            // if(!ModelState.IsValid){
            //     return Page();
            // }
            // try{
            //     _context.Add(GarconModel);
            //     await _context.SaveChangesAsync();
            //     return RedirectToPage("/Garcon/Index");
            // } catch(DbUpdateException){
            //     return Page();
            // }
                if(!ModelState.IsValid){
                    return Page();
                }
                
                var httpClient = new HttpClient();
                var url = "http://localhost:5171/Garcon/Create";
                var garconJson = JsonConvert.SerializeObject(GarconModel);
                var content = new StringContent(garconJson, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                
                if(response.IsSuccessStatusCode){
                    return RedirectToPage("/Garcon/Index");
                } else {
                    return Page();
                }
        }
    }
}