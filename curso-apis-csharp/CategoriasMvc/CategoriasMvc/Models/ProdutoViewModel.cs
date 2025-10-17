using System.ComponentModel.DataAnnotations;

namespace CategoriasMvc.Models;

public class ProdutoViewModel
{
    public int ProdutoId { get; set; }
    [Required(ErrorMessage = "O nome do Produto é obrigatório!")]
    public string? Nome { get; set; }
    [Required(ErrorMessage = "A descrição do Produto é obrigatório!")]
    public string? Descricao { get; set; }
    [Required(ErrorMessage = "O preço do Produto é obrigatório!")]
    public decimal Preco { get; set; }
    [Required(ErrorMessage = "A imagem do Produto é obrigatório!")]
    [Display(Name = "Caminho da Imagem")]
    public string? ImagemUrl { get; set; }

    [Display(Name = "Categoria do Produto")]
    public int CategoriaId { get; set; }
}
