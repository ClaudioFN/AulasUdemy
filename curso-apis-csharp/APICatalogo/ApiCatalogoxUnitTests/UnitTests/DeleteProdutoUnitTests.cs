using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class DeleteProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public DeleteProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProdutoById_Return_OkResult()
        {
            var prodId = 2;

            // Act
            var result = await _controller.Delete(prodId) as ActionResult<ProdutoDTO>;

            // Assert
            result.Should().NotBeNull(); // Verifica se o resultado nao e nulo
            result.Result.Should().BeOfType<OkObjectResult>(); // Verifica se o resultado e OKResult
        }

        [Fact]
        public async Task DeleteProdutoById_Return_NotFound()
        {
            var prodId = 999;

            // Act
            var result = await _controller.Delete(prodId) as ActionResult<ProdutoDTO>;

            // Assert 
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

}
