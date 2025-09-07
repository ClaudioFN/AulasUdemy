using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //private readonly ICategoriaInterface _repository;
        // private readonly IRepository<Categoria> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        //private readonly IMeuServico _meuServico;

        public CategoriasController(/*ICategoriaInterface repository , IMeuServico meuServico,*/ IConfiguration configuration
            , ILogger<CategoriasController> logger, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
            //_meuServico = meuServico;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["cahve2"];
            var secao1 = _configuration["secao1:chave2"];
        
            return $"Chave1 = {valor1} \nChave2 = {valor2} \nSeção1 => Chave2 = {secao1}";
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameters categoriasParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);

            return ObterCategorias(categorias);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

            return ObterCategorias(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriaDto = categorias.ToCategoriaDTOList();

            return Ok(categoriaDto);
        }

        /*[HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation(" ======================================GET API CATEGORIAS PRODUTOS======================================");
            //return _context.Categorias.Include(p => p.Produtos).Take(10).ToList(); // Take = limitar a quantidade trazida para a aplicacao
            //return _repository.GetAllE().ToList();
            return _unitOfWork.CategoriaRepository.GetAllAsync().ToList();
        }*/

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            //return _context.Categorias.AsNoTracking().ToList();// AsNoTracking = impede rastreio do estado dos objetos e armazenamento em cache que sobrecarregue a aplicacao
            //var categorias = _repository.GetAllE();
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();

            /*var categoriasDto = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDTO = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
                categoriasDto.Add(categoriaDTO);
            }*/

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            //throw new Exception("Exceção ao retornar detalhes pelo id"); // teste da aula 67
            try
            {
                _logger.LogInformation($" ======================================GET API CATEGORIAS ID = {id} ======================================");

                //var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                //var categoria = _repository.Get(c => c.CategoriaId == id);
                var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    _logger.LogInformation($" ======================================GET API CATEGORIAS ID = {id} NOT FOUND ======================================");

                    return NotFound("Categoria não encontrada!");
                }

                /*var categoriaDTO = new CategoriaDTO()
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };*/
                var categoriaDto = categoria.ToCategoriaDTO();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao tratar sua solicitação Get Especifico = {id} - {ex.Message}!");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest();

            //_context.Categorias.Add(categoria);
            //_context.SaveChanges();
            //var categoriaCriada = _repository.Create(categoria);

            /*var categoria = new Categoria()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };*/
            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();

            /*var novaCategoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };*/
            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    _logger.LogWarning($"Dados Inválidos..");
                    return BadRequest();
                }

                //_context.Entry(categoria).State = EntityState.Modified;
                //_context.SaveChanges();
                //_repository.Update(categoria);

                /*var categoria = new Categoria()
                { 
                    CategoriaId = categoriaDto.CategoriaId,
                    Nome = categoriaDto.Nome,
                    ImagemUrl = categoriaDto.ImagemUrl
                };*/

                var categoria = categoriaDto.ToCategoria();

                var categoriaAtualizada = _unitOfWork.CategoriaRepository.Update(categoria);
                await _unitOfWork.CommitAsync();

                /*var categoriaAtualizadaDto = new CategoriaDTO()
                {
                    CategoriaId = categoriaAtualizada.CategoriaId,
                    Nome = categoriaAtualizada.Nome,
                    ImagemUrl = categoriaAtualizada.ImagemUrl
                };*/
                var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

                return Ok(categoriaAtualizadaDto);
            }
            catch (Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao tratar sua solicitação Put Especifico = {id} - {ex.Message}!");
            }


        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            //var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            //var categoria = _repository.Get(c => c.CategoriaId == id);
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada para DELETE!");
            }

            //_context.Categorias.Remove(categoria);
            //_context.SaveChanges();
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            /*var categoriaExcluidaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaExcluida.CategoriaId,
                Nome = categoriaExcluida.Nome,
                ImagemUrl = categoriaExcluida.ImagemUrl
            };*/

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDto);
        }

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromService([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("SemUsarFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoSemFromService(IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }
    }
}
