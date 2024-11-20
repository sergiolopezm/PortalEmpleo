using Newtonsoft.Json;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.WebApp.Services.Interfaces;
using System.Net.Http.Headers;

namespace PortalEmpleo.WebApp.Services.Implementation
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }

        protected void ConfigureAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var idUsuario = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuario");
            if (!string.IsNullOrEmpty(idUsuario))
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("IdUsuario"))
                {
                    _httpClient.DefaultRequestHeaders.Add("IdUsuario", idUsuario);
                }
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            ConfigureAuthorizationHeader();

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<RespuestaDto>(jsonString);

            if (resultado.Resultado != null)
            {
                // Convertir el resultado al tipo específico solicitado
                var resultadoString = JsonConvert.SerializeObject(resultado.Resultado);
                return JsonConvert.DeserializeObject<T>(resultadoString);
            }

            return default(T);
        }

        public async Task<RespuestaDto> PostAsync<T>(string endpoint, T data)
        {
            ConfigureAuthorizationHeader();

            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RespuestaDto>(jsonString);
        }

        public async Task<RespuestaDto> PutAsync<T>(string endpoint, T data)
        {
            ConfigureAuthorizationHeader();

            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RespuestaDto>(jsonString);
        }

        public async Task<RespuestaDto> DeleteAsync(string endpoint)
        {
            ConfigureAuthorizationHeader();

            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RespuestaDto>(jsonString);
        }
    }
}
