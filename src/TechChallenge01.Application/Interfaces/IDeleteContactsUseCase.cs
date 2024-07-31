namespace TechChallenge01.Application.Interfaces;

public interface IDeleteContactsUseCase
{
    Task Delete(long Id);
}