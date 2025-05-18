using System.Text;
using fiap.Domain.Interfaces;
using Serilog;

namespace fiap.Services
{
    public class PedidoService: IPedidoService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger _logger;

        public PedidoService(IHttpClientFactory httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<bool> AtualizarStatusPedido(string idPedido, string statusPedido, string statusPagamento)
        {
            try
            {
                var client = _httpClient.CreateClient("Pedidos");
                //var url = $"api/pedidos/atualizar-status?idPedido={Uri.EscapeDataString(idPedido)}&statusPedido={Uri.EscapeDataString(statusPedido)}&statusPagamento={Uri.EscapeDataString(statusPagamento)}";
                var response = await client.PutAsync($"http://abf8a0373974c477c988a709a3916975-1818303021.us-east-1.elb.amazonaws.com/api/pedidos/atualizar-status?idPedido={idPedido}&statusPedido={statusPedido}&statusPagamento={statusPagamento}", null);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.Error($"Erro ao atualizar o status do pedido: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao atualizar o status do pedido: {ex.Message}");
                return false;
            }
        }
    }
}
