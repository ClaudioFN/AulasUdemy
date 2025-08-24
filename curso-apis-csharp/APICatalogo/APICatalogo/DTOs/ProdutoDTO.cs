﻿using APICatalogo.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório!")]
    [StringLength(80, ErrorMessage = "O nome deve ter entre 5 e 80 caracteres!", MinimumLength = 5)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
    public string? Descricao { get; set; }
    [Required]
    [Range(1, 100000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set; }
    [Required]
    [StringLength(300, MinimumLength = 5)]
    public string? ImagemUrl { get; set; }
    public int CategoriaId { get; set; }
}
