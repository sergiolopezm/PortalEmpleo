using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.Domain.Contracts.OfertaEmpleoRepository;
using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Domain.Contracts.PostulacionRepository;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.Postulacion;
using PortalEmpleo.Shared.OutDTO.OfertaEmpleo;
using PortalEmpleo.Shared.OutDTO.Postulacion;
using PortalEmpleo.WebApi.Attributes;

namespace PortalEmpleo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogAttribute))]
    public class PostulacionController : ControllerBase
    {
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IOfertaEmpleoRepository _ofertaRepository;
        private readonly ILogRepository _logRepository;

        public PostulacionController(
            IPostulacionRepository postulacionRepository,
            IOfertaEmpleoRepository ofertaRepository,
            ILogRepository logRepository)
        {
            _postulacionRepository = postulacionRepository;
            _ofertaRepository = ofertaRepository;
            _logRepository = logRepository;
        }

        [HttpPost]
        [ServiceFilter(typeof(AccesoAttribute))]
        [ValidarModelo]
        public IActionResult CrearPostulacion([FromBody] PostulacionDto postulacion)
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            // Primero obtenemos los detalles de la oferta
            var ofertaResultado = _ofertaRepository.ObtenerOferta(postulacion.IdOferta);
            if (!ofertaResultado.Exito)
            {
                _logRepository.Error(null, ip, "Crear Postulación",
                    $"Error al obtener detalles de la oferta {postulacion.IdOferta}: {ofertaResultado.Detalle}");
                return StatusCode(400, ofertaResultado);
            }

            var oferta = (OfertaEmpleoOutDto)ofertaResultado.Resultado!;
            var resultado = _postulacionRepository.CrearPostulacion(postulacion);

            if (resultado.Exito)
            {
                var postulacionCreada = (PostulacionOutDto)resultado.Resultado!;
                string detalleLog = $"El candidato {postulacion.Nombre} ({postulacion.Email}) " +
                                  $"se postuló a la oferta '{oferta.Titulo}' (ID: {oferta.IdOferta}) " +
                                  $"publicada por el reclutador {oferta.NombreReclutador} " +
                                  $"el {DateTime.Now:dd/MM/yyyy HH:mm:ss}. " +
                                  $"ID de Postulación generado: {postulacionCreada.IdPostulacion}";

                _logRepository.Accion(null, ip, "Crear Postulación", detalleLog);

                // Log adicional para el reclutador
                string detalleLogReclutador = $"Nueva postulación recibida para la oferta '{oferta.Titulo}' " +
                                            $"del candidato {postulacion.Nombre} ({postulacion.Email}) " +
                                            $"el {DateTime.Now:dd/MM/yyyy HH:mm:ss}. " +
                                            $"ID de Postulación: {postulacionCreada.IdPostulacion}";

                _logRepository.Info(oferta.IdReclutador, ip, "Nueva Postulación Recibida", detalleLogReclutador);
            }
            else
            {
                string detalleError = $"Error en la postulación de {postulacion.Nombre} ({postulacion.Email}) " +
                                    $"para la oferta ID {postulacion.IdOferta}: {resultado.Detalle}";

                _logRepository.Error(null, ip, "Error en Postulación", detalleError);
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [HttpGet("oferta/{idOferta}")]
        [ServiceFilter(typeof(AutorizacionJwtAttribute))]
        public IActionResult ListarPostulacionesPorOferta(int idOferta, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
        {
            string idReclutador = HttpContext.Request.Headers["IdUsuario"].ToString();
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";

            // Primero verificamos la oferta
            var ofertaResultado = _ofertaRepository.ObtenerOferta(idOferta);
            if (!ofertaResultado.Exito)
            {
                _logRepository.Error(idReclutador, ip, "Listar Postulaciones",
                    $"Error al verificar oferta {idOferta}: {ofertaResultado.Detalle}");
                return StatusCode(400, ofertaResultado);
            }

            var oferta = (OfertaEmpleoOutDto)ofertaResultado.Resultado!;

            // Verificamos que el reclutador sea el dueño de la oferta
            if (oferta.IdReclutador != idReclutador)
            {
                string detalleError = $"El reclutador {idReclutador} intentó acceder a postulaciones " +
                                    $"de la oferta {idOferta} que pertenece a otro reclutador";
                _logRepository.Error(idReclutador, ip, "Acceso No Autorizado", detalleError);

                return StatusCode(401, RespuestaDto.ParametrosIncorrectos(
                    "Acceso denegado",
                    "No tiene autorización para ver las postulaciones de esta oferta"));
            }

            var resultado = _postulacionRepository.ListarPostulacionesPorOferta(idOferta, pagina, tamanoPagina);

            if (resultado.Exito)
            {
                string detalleLog = $"El reclutador {oferta.NombreReclutador} (ID: {idReclutador}) " +
                                  $"consultó las postulaciones de la oferta '{oferta.Titulo}' (ID: {idOferta}). " +
                                  $"Página: {pagina}, Tamaño: {tamanoPagina}. " +
                                  $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                _logRepository.Accion(idReclutador, ip, "Consultar Postulaciones", detalleLog);
            }
            else
            {
                string detalleError = $"Error al consultar postulaciones de la oferta {idOferta}: {resultado.Detalle}";
                _logRepository.Error(idReclutador, ip, "Error al Listar Postulaciones", detalleError);
            }

            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }
    }
}
