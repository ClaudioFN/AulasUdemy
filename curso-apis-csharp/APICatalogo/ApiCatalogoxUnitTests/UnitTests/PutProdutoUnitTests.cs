using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests
{
    public class PutProdutoUnitTests : IClassFixture<ProdutosUnitTestController>
    {
        private readonly ProdutosController _controller;
        public PutProdutoUnitTests(ProdutosUnitTestController controller)
        {
            _controller = new ProdutosController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduto_Return_OkResult()
        {
            var prodId = 14;

            var updateProdutoDto = new ProdutoDTO
            {
                ProdutoId = prodId,
                Nome = "Produto Atualizado - Testes",
                Descricao = "Minha Descrição de Produto",
                ImagemUrl = "imageFake.jpg",
                CategoriaId = 2
            };

            var result = await _controller.Put(prodId, updateProdutoDto) as ActionResult<ProdutoDTO>;

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task PutProduto_Return_BadRequest()
        {
            var prodId = 10000;

            var meuProduto = new ProdutoDTO
            {

                ProdutoId = 14,
                Nome = "Produto Atualizado - Testes",
                Descricao = "Minha Descrição de Produto",
                ImagemUrl = "imageFake.jpg",
                CategoriaId = 2
            };

            var data = await _controller.Put(prodId, meuProduto);

            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);
    }
    }
}
