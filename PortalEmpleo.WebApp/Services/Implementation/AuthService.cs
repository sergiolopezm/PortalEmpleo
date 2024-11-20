using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO;
using PortalEmpleo.Shared.OutDTO;
using PortalEmpleo.WebApp.Services.Interfaces;

namespace PortalEmpleo.WebApp.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthService(
            IApiClient apiClient,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<RespuestaDto> LoginAsync(UsuarioLoginDto loginDto)
        {
            try
            {
                // Agregar IP del cliente
                loginDto.Ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

                // Llamar a la API de login
                var response = await _apiClient.PostAsync("api/Auth/login", loginDto);

                if (response.Exito && response.Resultado != null)
                {
                    // Convertir el resultado a JObject primero
                    var jObject = response.Resultado as Newtonsoft.Json.Linq.JObject;

                    // Deserializar correctamente a UsuarioOutDto
                    var usuario = jObject?.ToObject<UsuarioOutDto>();

                    if (usuario != null)
                    {
                        // Guardar token en sesión
                        _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", usuario.Token);
                        _httpContextAccessor.HttpContext?.Session.SetString("IdUsuario", usuario.IdUsuario);

                        // Guardar información del usuario en sesión
                        _httpContextAccessor.HttpContext?.Session.SetString("UserName", usuario.NombreUsuario);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserRole", usuario.Rol);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public async Task<RespuestaDto> RegisterAsync(UsuarioRegistroDto registroDto)
        {
            try
            {
                return await _apiClient.PostAsync("api/Auth/registro", registroDto);
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public async Task<UsuarioOutDto> GetCurrentUserAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.Session.GetString("UserName");
            var userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("IdUsuario");

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userRole) || string.IsNullOrEmpty(userId))
                return null;

            return new UsuarioOutDto
            {
                IdUsuario = userId,
                NombreUsuario = userName,
                Rol = userRole
            };
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.Session.GetString("JwtToken"));
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}
