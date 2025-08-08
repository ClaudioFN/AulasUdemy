using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")] // Produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // /produtos/
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
                return await _context.Produtos.Take(10).AsNoTracking().ToListAsync(); 

        }*/

        // /produtos/
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.Take(10).AsNoTracking().ToList(); // AsNoTracking = impede rastreio do estado dos objetos e armazenamento em cache que sobrecarregue a aplicacao
                                                                                   // Take = limitar a quantidade trazida para a aplicacao
                if (produtos is null)
                {
                    return NotFound("Nenhum Produto encontrado!");
                }
                return produtos;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao obter os Produtos: {ex.Message}" );
            }

        }

        // /produtos/{id}
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _context.Produtos.Take(10).FirstOrDefault(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("O Produto especificado não foi encontrado!");
                }

                return produto;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao obter o Produto especificado {id}: {ex.Message}");
            }

        }

        // /produtos/
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        // /produtos/{id}
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _context.Entry(produto).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao alterar o Produto {id}: {ex.Message}");
            }


        }


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado para DELETE!");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao deletar o Produto {id}: {ex.Message}");
            }

        }
    }
}
