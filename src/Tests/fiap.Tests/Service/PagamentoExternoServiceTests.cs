using System.Net;
using System.Text;
using System.Text.Json;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using fiap.Services;
using Moq;
using Moq.Protected;
using Serilog;
using Xunit;

namespace fiap.Tests.Service
{
    public class PagamentoExternoServiceTests
    {
        private readonly Pedido pedido;
        public PagamentoExternoServiceTests()
        {
            pedido = new Pedido
            {
                Cliente = new Cliente { Cpf = "12345678910", Email = "teste@teste.com", Id = 1, Nome = "Joao da Silva" },
                IdPedido = "1",
                Numero = "123456",
                Produtos = new List<Produto>
            {
                new Produto { Preco = 1, Nome = "teste", IdProduto = 1, IdCategoriaProduto = 1, Descricao = "teste" },
                new Produto { Preco = 2, Nome = "teste2", IdProduto = 2, IdCategoriaProduto = 2, Descricao = "teste2" }
            }
            };
        }

        [Fact]
        public async Task CriarOrdemPagamentoExterno_Test()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var secretManagerService = new Mock<ISecretManagerService>();
            var logger = new Mock<ILogger>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"RequestId\": \"1\", \"IdTeste\":\"123\"}")
                });

            var client = new HttpClient(mockMessageHandler.Object);
            httpClientFactory.Setup(x => x.CreateClient("MercadoPago")).Returns(client);

            secretManagerService.Setup(_ => _.ObterSecret<SecretMercadoPago>(It.IsAny<string>()))
                .ReturnsAsync(new SecretMercadoPago { AccessToken = "XX", ExternalPosId = "xx", UserId = "xx" });

            var service = new PagamentoExternoService(httpClientFactory.Object, secretManagerService.Object, logger.Object);
            var result = await service.CriarOrdemPagamentoExterno(pedido);

            Assert.True(result);

        }

        [Fact]
        public async Task ConsultarOrdemPagamentoExterno_Test()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var secretManagerService = new Mock<ISecretManagerService>();
            var logger = new Mock<ILogger>();

            var obj = new OrdemPagamentoExternoResponse
            {
                additional_info = "teste",
                application_id = Guid.NewGuid(),
                cancelled = false,
                client_id = "123",
                collector = new Collector { email = "teste@teste.com", id = 1, nickname = "teste" },
                date_created = DateTime.Now,
                external_reference = "123",
                id = 1,
                is_test = false,
                last_updated = DateTime.Now,
                marketplace = "teste",
                order_status = "teste",
                items = new List<ItemResponse>
            {
                new ItemResponse { category_id = "teste", currency_id = "123", description = "teste", id = "123", picture_url = "...", quantity = 1, title = "teste", unit_price = 123 }
            },
                notification_url = "...",
                paid_amount = 1,
                payer = new object(),
                payments = new List<object>(),
                payouts = new List<object>(),
                preference_id = "123",
                refunded_amount = 10,
                shipments = new List<object>(),
                shipping_cost = 10,
                site_id = "123",
                sponsor_id = new object(),
                status = "teste",
                total_amount = 10
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json")
                });

            var client = new HttpClient(mockMessageHandler.Object);
            httpClientFactory.Setup(x => x.CreateClient("MercadoPago")).Returns(client);

            secretManagerService.Setup(_ => _.ObterSecret<SecretMercadoPago>(It.IsAny<string>()))
                .ReturnsAsync(new SecretMercadoPago { AccessToken = "XX", ExternalPosId = "xx", UserId = "xx" });

            var service = new PagamentoExternoService(httpClientFactory.Object, secretManagerService.Object, logger.Object);
            var result = await service.ConsultarOrdemPagamentoExterno("teste");

            Assert.NotNull(result);
            Assert.IsType<OrdemPagamentoExternoResponse>(result);

        }
    }
}
