using PortalEmpleo.Domain.Contracts.PostulacionRepository;
using PortalEmpleo.Infraestructure;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.Postulacion;
using PortalEmpleo.Shared.OutDTO.Postulacion;

namespace PortalEmpleo.Domain.Services.PostulacionRepository
{
    public class PostulacionRepository : IPostulacionRepository
    {
        private readonly DBContext _context;

        public PostulacionRepository(DBContext context)
        {
            _context = context;
        }

        public RespuestaDto CrearPostulacion(PostulacionDto postulacion)
        {
            try
            {
                var oferta = _context.OfertaEmpleos
                    .FirstOrDefault(o => o.IdOferta == postulacion.IdOferta && o.Estado == "Activa");

                if (oferta == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Error al postular",
                        "La oferta no existe o no está activa");
                }

                var nuevaPostulacion = new Postulacion
                {
                    IdOferta = postulacion.IdOferta,
                    Nombre = postulacion.Nombre,
                    Email = postulacion.Email,
                    FechaPostulacion = DateTime.Now
                };

                _context.Postulacions.Add(nuevaPostulacion);
                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Postulación exitosa",
                    Detalle = "Su postulación ha sido registrada correctamente",
                    Resultado = ConvertirAPostulacionOutDto(nuevaPostulacion)
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ListarPostulacionesPorOferta(int idOferta, int pagina, int tamanoPagina)
        {
            try
            {
                // Consulta base
                var baseQuery = _context.Postulacions
                    .Where(p => p.IdOferta == idOferta);

                // Materializar el conteo total
                var totalRegistros = baseQuery.Count();
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);

                // Obtener y materializar los resultados paginados
                var postulaciones = baseQuery
                    .OrderByDescending(p => p.FechaPostulacion)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .AsEnumerable() // Materializa los resultados
                    .Select(p => ConvertirAPostulacionOutDto(p))
                    .ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Postulaciones encontradas",
                    Resultado = new
                    {
                        Pagina = pagina,
                        TotalPaginas = totalPaginas,
                        TotalRegistros = totalRegistros,
                        Postulaciones = postulaciones
                    }
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        private PostulacionOutDto ConvertirAPostulacionOutDto(Postulacion postulacion)
        {
            return new PostulacionOutDto
            {
                IdPostulacion = postulacion.IdPostulacion,
                Nombre = postulacion.Nombre,
                Email = postulacion.Email,
                FechaPostulacion = postulacion.FechaPostulacion ?? DateTime.Now
            };
        }
    }
}
