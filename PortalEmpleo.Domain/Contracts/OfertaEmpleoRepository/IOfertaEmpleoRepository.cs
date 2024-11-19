using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;

namespace PortalEmpleo.Domain.Contracts.OfertaEmpleoRepository
{
    public interface IOfertaEmpleoRepository
    {
        RespuestaDto CrearOferta(OfertaEmpleoDto oferta, string idReclutador);
        RespuestaDto ActualizarOferta(int idOferta, OfertaEmpleoDto oferta);
        RespuestaDto EliminarOferta(int idOferta);
        RespuestaDto ObtenerOferta(int idOferta);
        RespuestaDto ListarOfertas(int pagina, int tamanoPagina, string? filtro = null);
        RespuestaDto ListarOfertasReclutador(string idReclutador, int pagina, int tamanoPagina);
    }
}
