using System.ComponentModel.DataAnnotations;

namespace TechChallenge01.Application.ViewModels;

public record DeleteContactRequest([Required] long Id);
    