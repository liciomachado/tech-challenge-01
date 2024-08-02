using System.Threading.Tasks;

namespace TechChallenge01.Domain.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();


    }
}