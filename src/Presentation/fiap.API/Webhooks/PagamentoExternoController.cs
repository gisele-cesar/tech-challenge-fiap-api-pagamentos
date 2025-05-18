using fiap.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Webhooks
{
    /// <summary>
    /// Controller responsible for handling external payment webhooks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoExternoController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IPagamentoApplication _pagamentoApplication;
        private readonly IPedidoApplication _pedidoApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagamentoExternoController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="pagamentoApplication">The payment application service.</param>
        /// <param name="pedidoApplication">The order application service.</param>
        public PagamentoExternoController(Serilog.ILogger logger, IPagamentoApplication pagamentoApplication, IPedidoApplication pedidoApplication)
        {
            _logger = logger;
            _pagamentoApplication = pagamentoApplication;
            _pedidoApplication = pedidoApplication;
        }

        /// <summary>
        /// Receives an event for a created payment order.
        /// </summary>
        /// <param name="id">The ID of the payment order.</param>
        /// <param name="topic">The topic of the event.</param>
        /// <param name="content">The content of the event.</param>
        /// <returns>Returns OK to confirm receipt of the event.</returns>
        /// <response code="200">Returns OK to confirm receipt of the event.</response>
        /// <response code="400">If there is an error retrieving the order for the created payment order.</response>
        /// <response code="500">If there is a database connection error.</response>
        [HttpPost("ReceberEventoOrdemCriada")]
        public async Task<IActionResult> ReceberEventoOrdemCriada([FromQuery] string id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            _logger.Information($"Recebendo evento de ordem de pagamento criado no MP. IdOrdemComercial: {id}");
            return Ok(await _pagamentoApplication.ConsultarOrdemPagamento(id));
        }

        /// <summary>
        /// Receives an event for a processed payment.
        /// </summary>
        /// <param name="data_id">The ID of the processed payment.</param>
        /// <param name="topic">The topic of the event.</param>
        /// <param name="content">The content of the event.</param>
        /// <returns>Returns OK to confirm receipt of the event.</returns>
        /// <response code="200">Returns OK to confirm receipt of the event.</response>
        /// <response code="400">If there is an error retrieving the orders.</response>
        /// <response code="500">If there is a database connection error.</response>
        [HttpPost("ReceberEventoPagamentoProcessado")]
        public async Task<IActionResult> ReceberEventoPagamentoProcessado([FromQuery] int data_id, [FromQuery] string topic, [FromBody] dynamic content)
        {
            _logger.Information($"Evento 'Pagamento Processado' para o pedido id: {data_id} recebido.");

            var statusPagamento = "Aprovado";
            var statusPedido = "Recebido";

            _logger.Information($"Atualizando status pagamento pedido id: {data_id}.");

            // realizar chamada da API Pedidos para atualizar o status do pedido
            await _pedidoApplication.AtualizarStatusPedido(data_id.ToString(), statusPedido, statusPagamento);

            _logger.Information($"Status pagamento pedido id: {data_id} atualizado com sucesso!");

            return Ok();
        }
    }
}
