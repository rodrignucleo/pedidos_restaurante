using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ProjetoGerenciamentoRestaurante.RazorPages.Data;
using ProjetoGerenciamentoRestaurante.RazorPages.Models;

namespace ProjetoGerenciamentoRestaurante.RazorPages.Pages.Produto
{
    public class Create : PageModel
    {
        [BindProperty]
        public ProdutoModel ProdutoModel { get; set; } = new();
        public List<CategoriaModel> CategoriaList { get; set; } = new();

        public Create(AppDbContext context){
        }

        public async Task<IActionResult> OnGetAsync(){
            var httpClientCategoria = new HttpClient();
            var urlCategoria = "http://localhost:5171/Categoria";
            var requestMessageCategoria = new HttpRequestMessage(HttpMethod.Get, urlCategoria);
            var responseCategoria = await httpClientCategoria.SendAsync(requestMessageCategoria);
            var contentCategoria = await responseCategoria.Content.ReadAsStringAsync();

            CategoriaList = JsonConvert.DeserializeObject<List<CategoriaModel>>(contentCategoria)!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id){
            if(!ModelState.IsValid){
                return Page();
            }
            
            var httpClient = new HttpClient();
            var url = "http://localhost:5171/Produto/Create";
            var produtoJson = JsonConvert.SerializeObject(ProdutoModel);
            var content = new StringContent(produtoJson, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            
            if(response.IsSuccessStatusCode){
                return RedirectToPage("/Produto/Index");
            } else {
                return Page();
            }
        }
    }
}