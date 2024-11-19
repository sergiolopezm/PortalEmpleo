using PortalEmpleo.Infraestructure;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO;

namespace PortalEmpleo.Domain.Contracts
{
    public interface IAccesoRepository
    {
        bool ValidarAcceso(string sitio, string contraseña);
    }  
}