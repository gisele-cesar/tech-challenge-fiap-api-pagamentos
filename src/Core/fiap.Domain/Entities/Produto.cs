using System;
using System.Text.Json.Serialization;

namespace fiap.Domain.Entities
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public int IdCategoriaProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}
