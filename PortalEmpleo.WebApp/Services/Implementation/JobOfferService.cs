using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;
using PortalEmpleo.WebApp.Services.Interfaces;

namespace PortalEmpleo.WebApp.Services.Implementation
{
    public class JobOfferService : IJobOfferService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogRepository _logRepository;

        public JobOfferService(IApiClient apiClient,  ILogRepository logRepository)
        {
            _apiClient = apiClient;
            _logRepository = logRepository;
        }

        public async Task<RespuestaDto> CreateJobOfferAsync(OfertaEmpleoDto oferta)
        {
            return await _apiClient.PostAsync("api/OfertaEmpleo", oferta);
        }

        public async Task<RespuestaDto> UpdateJobOfferAsync(int idOferta, OfertaEmpleoDto oferta)
        {
            return await _apiClient.PutAsync($"api/OfertaEmpleo/{idOferta}", oferta);
        }

        public async Task<RespuestaDto> DeleteJobOfferAsync(int idOferta)
        {
            return await _apiClient.DeleteAsync($"api/OfertaEmpleo/{idOferta}");
        }

        public async Task<RespuestaDto> GetJobOfferAsync(int idOferta)
        {
            return await _apiClient.GetAsync<RespuestaDto>($"api/OfertaEmpleo/{idOferta}");
        }

        public async Task<RespuestaDto> ListJobOffersAsync(int pagina = 1, int tamanoPagina = 10, string filtro = null)
        {
            string endpoint = $"api/OfertaEmpleo?pagina={pagina}&tamanoPagina={tamanoPagina}";
            if (!string.IsNullOrEmpty(filtro))
            {
                endpoint += $"&filtro={Uri.EscapeDataString(filtro)}";
            }
            return await _apiClient.GetAsync<RespuestaDto>(endpoint);
        }

        public async Task<RespuestaDto> ListRecruiterJobOffersAsync(string idReclutador, int pagina = 1, int tamanoPagina = 10)
        {
            try
            {
                // Corregir la URL 
                string endpoint = $"api/OfertaEmpleo/reclutador/{idReclutador}?pagina={pagina}&tamanoPagina={tamanoPagina}";
                return await _apiClient.GetAsync<RespuestaDto>(endpoint);
            }
            catch (Exception ex)
            {                
                _logRepository.Error(null, null, "Error al obtener las ofertas de empleo del reclutador", ex.Message);
                return RespuestaDto.ErrorInterno();
            }
        }
    }
}
