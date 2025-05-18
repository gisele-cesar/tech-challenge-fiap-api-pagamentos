using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;

namespace fiap.Application.UseCases
{
    public class PedidoApplication : IPedidoApplication
    {
        private readonly ILogger _logger;
        private readonly IPedidoService _pedidoService;
        public PedidoApplication(ILogger logger, IPedidoService pedidoService)
        {
            _logger = logger;
            _pedidoService = pedidoService;
        }
        public async Task<bool> AtualizarStatusPedido(string idPedido, string statusPedido, string statusPagamento)
        {
            _logger.Information($"Atualizando status do pedido id: {idPedido}.");
            return await _pedidoService.AtualizarStatusPedido(idPedido, statusPedido, statusPagamento);

        }
    }
}
