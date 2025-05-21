using System.Threading.Tasks;
using fiap.Domain.Entities;

namespace fiap.Application.Interfaces
{
    public interface IPagamentoApplication
    {
        Task<bool> CriarOrdemPagamento(Pedido pedido);
        Task<object> ConsultarOrdemPagamento(string idOrdemComercial);
    }
}
