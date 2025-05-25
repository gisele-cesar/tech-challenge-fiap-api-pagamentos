using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using fiap.Services;
using Serilog;
using Moq.Protected;

namespace fiap.Tests.Service
{

    public class PedidoServiceTests
    {
        [Fact]
        public async Task AtualizarStatusPedido_DeveRetornarTrue_QuandoRequisicaoBemSucedida()
        {
            // Criando um HttpMessageHandler falso para simular a resposta HTTP
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected()
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

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(_ => _.CreateClient("Pedido")).Returns(httpClient);

            var loggerMock = new Mock<ILogger>();
            var service = new PedidoService(httpClientFactoryMock.Object, loggerMock.Object);

            var resultado = await service.AtualizarStatusPedido("001", "Em andamento", "Pago");

            Assert.True(resultado);
        }

        [Fact]
        public async Task AtualizarStatusPedido_DeveRetornarFalse_QuandoRequisicaoFalha()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock.Protected()
               .Setup<Task<HttpResponseMessage>>(
                   "SendAsync",
                   ItExpr.IsAny<HttpRequestMessage>(),
                   ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.InternalServerError
               });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(_ => _.CreateClient("Pedido")).Returns(httpClient);

            var loggerMock = new Mock<ILogger>();
            var service = new PedidoService(httpClientFactoryMock.Object, loggerMock.Object);

            var resultado = await service.AtualizarStatusPedido("001", "Em andamento", "Pago");

            // Verificações
            Assert.False(resultado);
        }
    }
}
