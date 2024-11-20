using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;

namespace PortalEmpleo.WebApp.Services.Interfaces
{
    public interface IJobOfferService
    {
        Task<RespuestaDto> CreateJobOfferAsync(OfertaEmpleoDto oferta);
        Task<RespuestaDto> UpdateJobOfferAsync(int idOferta, OfertaEmpleoDto oferta);
        Task<RespuestaDto> DeleteJobOfferAsync(int idOferta);
        Task<RespuestaDto> GetJobOfferAsync(int idOferta);
        Task<RespuestaDto> ListJobOffersAsync(int pagina = 1, int tamanoPagina = 10, string filtro = null);
        Task<RespuestaDto> ListRecruiterJobOffersAsync(string idReclutador, int pagina = 1, int tamanoPagina = 10);
    }
}
