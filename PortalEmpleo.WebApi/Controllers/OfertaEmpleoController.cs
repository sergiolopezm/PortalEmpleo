using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Domain.Contracts.OfertaEmpleoRepository;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;
using PortalEmpleo.Shared.OutDTO.OfertaEmpleo;
using PortalEmpleo.WebApi.Attributes;

namespace PortalEmpleo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]
    public class OfertaEmpleoController : ControllerBase
    {
        private readonly IOfertaEmpleoRepository _ofertaRepository;
        private readonly ILogRepository _logRepository;

        public OfertaEmpleoController(
            IOfertaEmpleoRepository ofertaRepository,
            ILogRepository logRepository)
        {
            _ofertaRepository = ofertaRepository;
            _logRepository = logRepository;
        }

        [HttpPost]
        [ServiceFilter(typeof(AutorizacionJwtAttribute))]
        [ValidarModelo]
        public IActionResult CrearOferta([FromBody] OfertaEmpleoDto oferta)
        {
            string idReclutador = HttpContext.Request.Headers["IdUsuario"].ToString();
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            var resultado = _ofertaRepository.CrearOferta(oferta, idReclutador);

            if (resultado.Exito)
            {
                var ofertaCreada = (OfertaEmpleoOutDto)resultado.Resultado!;
                string detalleLog = $"El reclutador {ofertaCreada.NombreReclutador} (ID: {idReclutador}) " +
                                  $"publicó la oferta de empleo '{ofertaCreada.Titulo}' (ID: {ofertaCreada.IdOferta}) " +
                                  $"el {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                _logRepository.Accion(idReclutador, ip, "Crear Oferta Empleo", detalleLog);
            }
            else
            {
                _logRepository.Error(idReclutador, ip, "Crear Oferta Empleo",
                    $"Error al crear oferta: {resultado.Detalle}");
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [HttpPut("{idOferta}")]
        [ServiceFilter(typeof(AutorizacionJwtAttribute))]
        [ValidarModelo]
        public IActionResult ActualizarOferta(int idOferta, [FromBody] OfertaEmpleoDto oferta)
        {
            string idReclutador = HttpContext.Request.Headers["IdUsuario"].ToString();
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            var resultado = _ofertaRepository.ActualizarOferta(idOferta, oferta);

            if (resultado.Exito)
            {
                var ofertaActualizada = (OfertaEmpleoOutDto)resultado.Resultado!;
                string detalleLog = $"El reclutador {ofertaActualizada.NombreReclutador} (ID: {idReclutador}) " +
                                  $"actualizó la oferta de empleo '{ofertaActualizada.Titulo}' (ID: {idOferta}) " +
                                  $"el {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                _logRepository.Accion(idReclutador, ip, "Actualizar Oferta Empleo", detalleLog);
            }
            else
            {
                _logRepository.Error(idReclutador, ip, "Actualizar Oferta Empleo",
                    $"Error al actualizar oferta {idOferta}: {resultado.Detalle}");
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [HttpDelete("{idOferta}")]
        [ServiceFilter(typeof(AutorizacionJwtAttribute))]
        public IActionResult EliminarOferta(int idOferta)
        {
            string idReclutador = HttpContext.Request.Headers["IdUsuario"].ToString();
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            var resultado = _ofertaRepository.EliminarOferta(idOferta);

            if (resultado.Exito)
            {
                string detalleLog = $"El reclutador con ID {idReclutador} eliminó/inactivó la oferta " +
                                  $"de empleo ID {idOferta} el {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                _logRepository.Accion(idReclutador, ip, "Eliminar Oferta Empleo", detalleLog);
            }
            else
            {
                _logRepository.Error(idReclutador, ip, "Eliminar Oferta Empleo",
                    $"Error al eliminar oferta {idOferta}: {resultado.Detalle}");
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [HttpGet("{idOferta}")]
        [ServiceFilter(typeof(AccesoAttribute))]
        public IActionResult ObtenerOferta(int idOferta)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            var resultado = _ofertaRepository.ObtenerOferta(idOferta);

            if (resultado.Exito)
            {
                var ofertaConsultada = (OfertaEmpleoOutDto)resultado.Resultado!;
                string detalleLog = $"Se consultó la oferta de empleo '{ofertaConsultada.Titulo}' (ID: {idOferta}) " +
                                  $"el {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                _logRepository.Info(null, ip, "Consultar Oferta Empleo", detalleLog);
            }
            else
            {
                _logRepository.Info(null, ip, "Consultar Oferta Empleo",
                    $"Error al consultar oferta {idOferta}: {resultado.Detalle}");
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [HttpGet]
        [ServiceFilter(typeof(AccesoAttribute))]
        public IActionResult ListarOfertas([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10, [FromQuery] string? filtro = null)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            var resultado = _ofertaRepository.ListarOfertas(pagina, tamanoPagina, filtro);

            string detalleLog = resultado.Exito
                ? $"Se consultó el listado de ofertas. Página: {pagina}, Tamaño: {tamanoPagina}" +
                  (filtro != null ? $", Filtro: {filtro}" : "") +
                  $" el {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
                : $"Error al listar ofertas: {resultado.Detalle}";

            _logRepository.Info(null, ip, "Listar Ofertas Empleo", detalleLog);

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }
    }
}
