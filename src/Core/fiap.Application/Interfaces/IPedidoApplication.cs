using System.Threading.Tasks;

namespace fiap.Application.Interfaces
{
    public interface IPedidoApplication
    {
        Task<bool> AtualizarStatusPedido(string idPedido, string statusPedido, string statusPagamento);
    }
}
