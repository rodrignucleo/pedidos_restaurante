namespace ProjetoGerenciamentoRestaurante.API.Models
{
    public class PedidoViewModel
    {
        public MesaModel? Mesa { get; set; }
        public GarconModel? Garcon { get; set; }
        public AtendimentoModel? Atendimento { get; set; }
        public double totalVendas { get; set; }
        public int countPedidos { get; set; }
        public double quantidadeTotal { get; set; } 
    }
}