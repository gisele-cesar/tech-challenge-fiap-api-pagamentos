using fiap.API.Webhooks;
using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace fiap.Tests.ControllerTests
{
    public class ControllerPagamentoExternoTests
    {
        private readonly Pedido pedido;
        private readonly OrdemPagamentoExternoResponse objPagamento;
        public ControllerPagamentoExternoTests()
        {
            pedido = new Pedido
            {
                Cliente = new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" },
                IdPedido = 1,
                Numero = "123456",
                Produtos = new List<Produto>
                {
                    new Produto { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste" },
                    new Produto { Preco = 2, Nome = "teste2", IdProduto = 2, IdCategoriaProduto = 2, Descricao = "teste2" }
                }
            };
            objPagamento = new OrdemPagamentoExternoResponse
            {
                id = 1,
                status = "teste",
                external_reference = "123",
                preference_id = "123",
                items = new List<ItemResponse>
                {
                    new ItemResponse { category_id = "teste", currency_id = "123", description = "teste", id = "123", picture_url = "...", quantity = 1, title = "teste", unit_price = 123 }
                }
            };
        }

        [Fact]
        public async Task ReceberEventoOrdemCriada_OKAsync()
        {
            var pedidoApplication = new Mock<IPedidoApplication>();
            var pagamentoApplication = new Mock<IPagamentoApplication>();
            var logger = new Mock<Serilog.ILogger>();

            pagamentoApplication
                .Setup(_ => _.ConsultarOrdemPagamento(It.IsAny<string>()))
                .ReturnsAsync(objPagamento);

            var controller = new PagamentoExternoController(logger.Object, pagamentoApplication.Object, pedidoApplication.Object);

            var result = await controller.ReceberEventoOrdemCriada("123", "topic", new { });

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(objPagamento, okResult.Value);
        }

        [Fact]
        public async Task ReceberEventoPagamentoProcessado_OKAsync()
        {
            var pedidoApplication = new Mock<IPedidoApplication>();
            var pagamentoApplication = new Mock<IPagamentoApplication>();
            var logger = new Mock<Serilog.ILogger>();

            pedidoApplication
                .Setup(_ => _.AtualizarStatusPedido("1", "Recebido", "Aprovado"))
                .ReturnsAsync(true);

            var controller = new PagamentoExternoController(logger.Object, pagamentoApplication.Object, pedidoApplication.Object);

            var result = await controller.ReceberEventoPagamentoProcessado(1, "topic", new { });

            Assert.IsType<OkResult>(result);
        }
    }
}
