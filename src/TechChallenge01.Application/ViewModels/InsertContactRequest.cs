﻿using System.ComponentModel.DataAnnotations;

namespace TechChallenge01.Application.ViewModels;

public record InsertContactRequest([Required] string Nome,
    [Required][MinLength(11)][MaxLength(20)] string PhoneNumber,
    [EmailAddress] string Email);
 