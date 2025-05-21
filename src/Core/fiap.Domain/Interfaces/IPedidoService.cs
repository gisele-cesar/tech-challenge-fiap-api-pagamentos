using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fiap.Domain.Interfaces
{
    public interface IPedidoService
    {
        Task<bool> AtualizarStatusPedido(string idPedido, string statusPedido, string statusPagamento);
    }
}
