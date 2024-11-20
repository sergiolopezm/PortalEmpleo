using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO;
using PortalEmpleo.Shared.OutDTO;

namespace PortalEmpleo.WebApp.Services.Interfaces
{
    public interface IAuthService
    {
        Task<RespuestaDto> LoginAsync(UsuarioLoginDto loginDto);
        Task<RespuestaDto> RegisterAsync(UsuarioRegistroDto registroDto);
        Task<UsuarioOutDto> GetCurrentUserAsync();
        bool IsAuthenticated();
        void Logout();
    }
}
