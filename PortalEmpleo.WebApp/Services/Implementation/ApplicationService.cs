using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.Postulacion;
using PortalEmpleo.WebApp.Services.Interfaces;

namespace PortalEmpleo.WebApp.Services.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApiClient _apiClient;

        public ApplicationService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<RespuestaDto> CreateApplicationAsync(PostulacionDto postulacion)
        {
            return await _apiClient.PostAsync("api/Postulacion", postulacion);
        }

        public async Task<RespuestaDto> ListApplicationsForOfferAsync(int idOferta, int pagina, int tamanoPagina)
        {
            return await _apiClient.GetAsync<RespuestaDto>(
                $"api/Postulacion/oferta/{idOferta}?pagina={pagina}&tamanoPagina={tamanoPagina}");
        }
    }
}
