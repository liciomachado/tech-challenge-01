using Microsoft.AspNetCore.Mvc;
using TechChallenge01.Application.ViewModels;

namespace TechChallenge01.Application.Interfaces;

public interface IInsertContactUseCase
{
    Task<PublishResponse> Execute(InsertContactRequest insertContactRequest);
}
