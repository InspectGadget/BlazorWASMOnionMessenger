using BlazorWASMOnionMessenger.Domain.Entities;

namespace BlazorWASMOnionMessenger.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
