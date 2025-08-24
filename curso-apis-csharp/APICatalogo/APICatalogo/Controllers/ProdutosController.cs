using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")] // Produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        //private readonly IProdutoRepository _produtoRepository;
        //private readonly IRepository<Produto> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(/*IProdutoRepository produtoRepository, IRepository<Produto> repository,*/ IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_produtoRepository = produtoRepository;
            //_repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult <IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
        {
            //var produtos = _produtoRepository.GetProdutosPorCategoria(id);
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

            if(produtos is null)
            {
                return NotFound();
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        // /produtos/
        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2()
        {
                return await _context.Produtos.Take(10).AsNoTracking().ToListAsync(); 

        }*/

        // /produtos/
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                //var produtos = _context.Produtos.Take(10).AsNoTracking().ToList(); // AsNoTracking = impede rastreio do estado dos objetos e armazenamento em cache que sobrecarregue a aplicacao
                                                                                   // Take = limitar a quantidade trazida para a aplicacao
                //var produtos = _repository.GetAllE();
                var produtos = _unitOfWork.ProdutoRepository.GetAllE();
                if (produtos is null)
                {
                    return NotFound("Nenhum Produto encontrado!");
                }

                var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

                return Ok(produtosDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao obter os Produtos: {ex.Message}" );
            }

        }

        // /produtos/{id}
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            try
            {
                //var produto = _repository.Get(c => c.ProdutoId == id);
                var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("O Produto especificado não foi encontrado!");
                }

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao obter o Produto especificado {id}: {ex.Message}");
            }

        }

        // /produtos/
        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest();

            //var novoProduto = _repository.Create(produto);

            var produto = _mapper.Map<Produto>(produtoDto);
            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
        }

        [HttpPatch("{id}/UpdateParcial")]
        public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
        {
            if(patchProdutoDTO is null || id <= 0)
            {
                return BadRequest();
            }

            var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);

            if(produto is null)
            {
                return NotFound();
            }

            var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

            patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(produtoUpdateRequest))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(produtoUpdateRequest, produto);
            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();


            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
        }

        // /produtos/{id}
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest();
                }

                //_context.Entry(produto).State = EntityState.Modified;
                //_context.SaveChanges();
                //bool atualizado = _repository.Update(produto);
                //var produtoAtualizado = _repository.Update(produto);

                var produto = _mapper.Map<Produto>(produtoDto);
                var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
                _unitOfWork.Commit();

                var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);

                if(produtoAtualizadoDto is not null)
                {
                    return Ok(produtoAtualizadoDto);
                } else
                {
                    return StatusCode(500, $"Falha ao atualizar o produto de ID = {id}");
                }
                    
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao alterar o Produto {id}: {ex.Message}");
            }


        }


        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            try
            {
                /*var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado para DELETE!");
                }

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);*/

                //bool deletado = _repository.Delete(id);
                //var produto = _repository.Get(c => c.ProdutoId == id);
                var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);

                if (produto is null)
                    return StatusCode(500, $"Falha ao excluir o produto de ID = {id}");

                //var deletado = _repository.Delete(produto);
                var deletado = _unitOfWork.ProdutoRepository.Delete(produto);
                _unitOfWork.Commit();

                var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(deletado);

                return Ok($"Produto de ID = {id} foi excluído!");
                /*if (deletado)
                {
                    return Ok($"Produto de ID = {id} foi excluído!");
                }
                else
                {
                    return StatusCode(500, $"Falha ao excluir o produto de ID = {id}");
                }*/
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao deletar o Produto {id}: {ex.Message}");
            }

        }
    }
}
