using fiap.Application.UseCases;
using fiap.Domain.Interfaces;
using Moq;
using Serilog;
using Xunit;

namespace fiap.Tests.Application
{
    public class PedidoApplicationTests
    {
        [Fact]
        public async Task AtualizarStatusPedido_DeveRetornarTrue_QuandoAtualizacaoBemSucedida()
        {
            // Configurando o mock do serviço
            var mockPedidoService = new Mock<IPedidoService>();
            mockPedidoService.Setup(s => s.AtualizarStatusPedido(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var mockLogger = new Mock<ILogger>();
            var pedidoApplication = new PedidoApplication(mockLogger.Object, mockPedidoService.Object);

            // Chamada ao método
            var resultado = await pedidoApplication.AtualizarStatusPedido("001", "Em andamento", "Pago");

            // Verificações
            Assert.True(resultado);
            mockPedidoService.Verify(s => s.AtualizarStatusPedido("001", "Em andamento", "Pago"), Times.Once);
        }

        [Fact]
        public async Task AtualizarStatusPedido_DeveLancarExcecao_QuandoServicoFalha()
        {
            // Configurando o mock do serviço para lançar uma exceção
            var mockPedidoService = new Mock<IPedidoService>();
            mockPedidoService.Setup(s => s.AtualizarStatusPedido(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new System.Exception("Erro ao atualizar pedido"));

            var mockLogger = new Mock<ILogger>();
            var pedidoApplication = new PedidoApplication(mockLogger.Object, mockPedidoService.Object);

            // Verificando se a exceção é lançada corretamente
            var ex = await Assert.ThrowsAsync<System.Exception>(() => pedidoApplication.AtualizarStatusPedido("001", "Em andamento", "Pago"));

            Assert.Equal("Erro ao atualizar pedido", ex.Message);
        }
    }

}
