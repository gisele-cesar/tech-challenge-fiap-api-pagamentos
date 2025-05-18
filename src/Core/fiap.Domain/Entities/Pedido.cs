using System.Collections.Generic;
using System.Linq;

namespace fiap.Domain.Entities
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public Cliente Cliente { get; set; }
        public string Numero { get; set; }
        public string StatusPedido { get; set; }
        public string StatusPagamento { get; set; }
        public decimal ValorTotal
        {
            get { return Produtos?.Select(p => p.Preco).Sum() ?? 0; }
        }
        public List<Produto> Produtos { get; set; }
    }
}
