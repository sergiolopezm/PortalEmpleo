using PortalEmpleo.Shared.GeneralDTO;

namespace PortalEmpleo.WebApp.Services.Interfaces
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<RespuestaDto> PostAsync<T>(string endpoint, T data);
        Task<RespuestaDto> PutAsync<T>(string endpoint, T data);
        Task<RespuestaDto> DeleteAsync(string endpoint);
    }
}
