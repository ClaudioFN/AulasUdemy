using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaInterface _repository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        //private readonly IMeuServico _meuServico;

        public CategoriasController(ICategoriaInterface repository /*, IMeuServico meuServico*/, IConfiguration configuration
            , ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _configuration = configuration;
            _logger = logger;
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

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation(" ======================================GET API CATEGORIAS PRODUTOS======================================");
            //return _context.Categorias.Include(p => p.Produtos).Take(10).ToList(); // Take = limitar a quantidade trazida para a aplicacao
            return _repository.GetCategorias().ToList();
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            //return _context.Categorias.AsNoTracking().ToList();// AsNoTracking = impede rastreio do estado dos objetos e armazenamento em cache que sobrecarregue a aplicacao
            var categorias = _repository.GetCategorias();

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            //throw new Exception("Exceção ao retornar detalhes pelo id"); // teste da aula 67
            try
            {
                _logger.LogInformation($" ======================================GET API CATEGORIAS ID = {id} ======================================");

                //var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                var categoria = _repository.GetCategoria(id);

                if (categoria is null)
                {
                    _logger.LogInformation($" ======================================GET API CATEGORIAS ID = {id} NOT FOUND ======================================");

                    return NotFound("Categoria não encontrada!");
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao tratar sua solicitação Get Especifico = {id}!");
            }
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            //_context.Categorias.Add(categoria);
            //_context.SaveChanges();
            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaCriada.CategoriaId }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    _logger.LogWarning($"Dados Inválidos..");
                    return BadRequest();
                }

                //_context.Entry(categoria).State = EntityState.Modified;
                //_context.SaveChanges();
                _repository.Update(categoria);

                return Ok(categoria);
            }
            catch (Exception ex)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao tratar sua solicitação Put Especifico = {id}!");
            }


        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            var categoria = _repository.GetCategoria(id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada para DELETE!");
            }

            //_context.Categorias.Remove(categoria);
            //_context.SaveChanges();
            var categoriaExcluida = _repository.Delete(id);

            return Ok(categoriaExcluida);
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
