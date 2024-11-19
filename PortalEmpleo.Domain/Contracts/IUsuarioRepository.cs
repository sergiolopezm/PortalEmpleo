using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO;

namespace PortalEmpleo.Domain.Contracts
{
    public interface IUsuarioRepository
    {
        RespuestaDto AutenticarUsuario(UsuarioLoginDto args);
        RespuestaDto RegistrarUsuario(UsuarioRegistroDto args);
    }
}