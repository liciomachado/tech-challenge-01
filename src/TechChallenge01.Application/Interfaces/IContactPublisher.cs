using TechChallenge01.Application.Events;

namespace TechChallenge01.Application.Interfaces
{
    public  interface IContactPublisher
    {
        Task PublishInsertContactAsync(InsertContactEvent message);
        Task PublishUpdateContactAsync(UpdateContactEvent message);
        Task PublishDeleteContactAsync(DeleteContactEvent message);
    }
}
