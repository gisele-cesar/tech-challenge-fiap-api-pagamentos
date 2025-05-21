using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using fiap.Domain.Entities;
using System.Text.Json;
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
                var client = _httpClient.CreateClient("Pedido");

                var response = await client.PutAsync($"http://a27f3383c578d40ae99fb9ceb2cb7cef-262459810.us-east-1.elb.amazonaws.com/api/Pedido/atualizar-status?idPedido={idPedido}&statusPedido={statusPedido}&statusPagamento={statusPagamento}", null);

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
