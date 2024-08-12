using System.ComponentModel.DataAnnotations;

namespace TechChallenge01.Application.ViewModels;

public record UpdateContactRequest([Required] long Id,
    [Required][MaxLength(255, ErrorMessage = "Tamanho inválido, máximo de 255 caracteres.")] string Name,
    [Required][Length(10, 20, ErrorMessage = "Tamanho inválido, deve ter entre 10 e 20 caracteres.")] string PhoneNumber,
    [EmailAddress(ErrorMessage = "Este endereço de e-mail não é válido.")][MaxLength(50, ErrorMessage = "Tamanho inválido, máximo de 50 caracteres.")] string Email);
