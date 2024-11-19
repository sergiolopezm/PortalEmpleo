using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.Postulacion;

namespace PortalEmpleo.Domain.Contracts.PostulacionRepository
{
    public interface IPostulacionRepository
    {
        RespuestaDto CrearPostulacion(PostulacionDto postulacion);
        RespuestaDto ListarPostulacionesPorOferta(int idOferta, int pagina, int tamanoPagina);
    }
}
