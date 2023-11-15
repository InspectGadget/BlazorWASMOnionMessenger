
namespace BlazorWASMOnionMessenger.Application.Interfaces.ChatType
{
    public interface IChatTypeService
    {
        Task<IEnumerable<Domain.Entities.ChatType>> GetChatTypesAsync();
    }
}
