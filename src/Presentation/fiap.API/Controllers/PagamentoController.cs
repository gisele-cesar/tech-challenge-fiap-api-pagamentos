


using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace fiap.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IPagamentoApplication _pagamentoApplication;
        public PagamentoController(Serilog.ILogger logger, IPagamentoApplication pagamentoApplication)
        {
            _logger = logger;
            _pagamentoApplication = pagamentoApplication;
        }
        /// <summary>
        /// Criar ordem de pagamento
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns>Retorna ok para confirmação de criação de ordem de pagamento</returns>
        /// <response code = "200">Retorna ok para confirmação de criação de ordem de pagamento</response>
        /// <response code = "400">Se houver erro na busca do pedido para a ordem criada</response>
        /// <response code = "500">Se houver erro de conexão com banco de dados</response>
        [HttpPost("CriarOrdemPagamento")]
        public async Task<IActionResult> CriarOrdemPagamento([FromBody] Pedido pedido)
        {
            _logger.Information($"Criando ordem de pagamento no MP para o pedido id: {pedido.IdPedido}.");
            return Ok(await _pagamentoApplication.CriarOrdemPagamento(pedido));
        }
    }
}
