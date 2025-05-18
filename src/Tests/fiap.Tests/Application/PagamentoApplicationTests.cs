using fiap.API.Controllers;
using fiap.Application.Interfaces;
using fiap.Application.UseCases;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace fiap.Tests.Application
{
    public class PagamentoApplicationTests
    {
        private readonly Pedido pedido;
        private readonly OrdemPagamentoExternoResponse objPagamento;
        public PagamentoApplicationTests()
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
            //objPagamento = new OrdemPagamentoExternoResponse
            //{
            //    additional_info = "teste",
            //    application_id = Guid.NewGuid(),
            //    cancelled = false,
            //    client_id = "123",
            //    collector = new Collector { email = "teste@teste.com", id = 1, nickname = "teste" },
            //    date_created = DateTime.Now,
            //    external_reference = "123",
            //    id = 1,
            //    is_test = false,
            //    last_updated = DateTime.Now,
            //    marketplace = "teste",
            //    order_status = "teste",
            //    items = new List<ItemResponse>() { new ItemResponse { category_id = "teste" , currency_id = "123" , description = "teste" , id = "123" , picture_url = "..." ,
            // quantity = 1 , title = "teste" , unit_price = 123} },
            //    notification_url = "...",
            //    paid_amount = 1,
            //    payer = new object { },
            //    payments = new List<object> { },
            //    payouts = new List<object> { },
            //    preference_id = "123",
            //    refunded_amount = 10,
            //    shipments = new List<object> { },
            //    shipping_cost = 10,
            //    site_id = "123",
            //    sponsor_id = new object { },
            //    status = "teste",
            //    total_amount = 10
            //};

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
        public async Task CriarOrdemPagamento_OkAsync()
        {
            var pagamentoExternoService = new Mock<IPagamentoExternoService>();
            var logger = new Mock<Serilog.ILogger>();

            pagamentoExternoService.Setup(_ => _.CriarOrdemPagamentoExterno(pedido)).ReturnsAsync(true);

            var app = new PagamentoApplication(logger.Object, pagamentoExternoService.Object);
            var result = await app.CriarOrdemPagamento(pedido);

            Assert.True(result);
        }

        [Fact]
        public async Task ConsultarOrdemPagamento_OkAsync()
        {
            var pagamentoExternoService = new Mock<IPagamentoExternoService>();
            var logger = new Mock<Serilog.ILogger>();

            pagamentoExternoService.Setup(_ => _.ConsultarOrdemPagamentoExterno(It.IsAny<string>())).ReturnsAsync(objPagamento);

            var app = new PagamentoApplication(logger.Object, pagamentoExternoService.Object);
            var result = await app.ConsultarOrdemPagamento("123");

            Assert.NotNull(result);
            Assert.IsType<OrdemPagamentoExternoResponse>(result);
        }
    }
}
