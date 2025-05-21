using System.Threading.Tasks;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;

namespace fiap.Application.UseCases
{
    public class PagamentoApplication : IPagamentoApplication
    {
        private readonly ILogger _logger;
        private readonly IPagamentoExternoService _pagamentoExternoService;

        public PagamentoApplication(ILogger logger, IPagamentoExternoService pagamentoExternoService)
        {
            _logger = logger;
            _pagamentoExternoService = pagamentoExternoService;
        }

        public async Task<bool> CriarOrdemPagamento(Pedido pedido)
        {

            _logger.Information($"Criando ordem de pagamento no MP para o pedido id: {pedido.IdPedido}.");
            return await _pagamentoExternoService.CriarOrdemPagamentoExterno(pedido);
        }

        public async Task<object> ConsultarOrdemPagamento(string idOrdemComercial)
        {
            _logger.Information($"Buscando ordem de pagamento no MP do idOrdemComercial: {idOrdemComercial}.");
            return await _pagamentoExternoService.ConsultarOrdemPagamentoExterno(idOrdemComercial);
        }
    }
}
