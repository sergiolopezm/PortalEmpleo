using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.Postulacion;

namespace PortalEmpleo.WebApp.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<RespuestaDto> CreateApplicationAsync(PostulacionDto postulacion);
        Task<RespuestaDto> ListApplicationsForOfferAsync(int idOferta, int pagina, int tamanoPagina);
    }
}
