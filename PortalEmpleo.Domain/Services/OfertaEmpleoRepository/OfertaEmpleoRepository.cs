using Microsoft.EntityFrameworkCore;
using PortalEmpleo.Domain.Contracts.OfertaEmpleoRepository;
using PortalEmpleo.Infraestructure;
using PortalEmpleo.Shared.GeneralDTO;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;
using PortalEmpleo.Shared.OutDTO.OfertaEmpleo;

namespace PortalEmpleo.Domain.Services.OfertaEmpleoRepository
{
    public partial class OfertaEmpleoRepository : IOfertaEmpleoRepository
    {
        private readonly DBContext _context;

        public OfertaEmpleoRepository(DBContext context)
        {
            _context = context;
        }

        public RespuestaDto CrearOferta(OfertaEmpleoDto oferta, string idReclutador)
        {
            try
            {
                var tipoContrato = _context.TiposContratos.Find(oferta.IdTipoContrato);
                if (tipoContrato == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Error al crear oferta",
                        "El tipo de contrato especificado no existe");
                }

                var nuevaOferta = new OfertaEmpleo
                {
                    Titulo = oferta.Titulo,
                    Descripcion = oferta.Descripcion,
                    Ubicacion = oferta.Ubicacion,
                    Salario = oferta.Salario,
                    IdTipoContrato = oferta.IdTipoContrato,
                    FechaPublicacion = DateTime.Now,
                    Estado = "Activa",
                    IdReclutador = idReclutador
                };

                _context.OfertaEmpleos.Add(nuevaOferta);
                _context.SaveChanges();

                // Recargar la oferta con las navegaciones
                var ofertaCompleta = _context.OfertaEmpleos
                    .Include(o => o.IdReclutadorNavigation)
                    .Include(o => o.IdTipoContratoNavigation)
                    .FirstOrDefault(o => o.IdOferta == nuevaOferta.IdOferta);

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Oferta creada",
                    Detalle = "La oferta de empleo se ha creado correctamente",
                    Resultado = ConvertirAOfertaOutDto(ofertaCompleta!)
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ActualizarOferta(int idOferta, OfertaEmpleoDto oferta)
        {
            try
            {
                var ofertaExistente = _context.OfertaEmpleos.Find(idOferta);
                if (ofertaExistente == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Error al actualizar",
                        "La oferta especificada no existe");
                }

                ofertaExistente.Titulo = oferta.Titulo;
                ofertaExistente.Descripcion = oferta.Descripcion;
                ofertaExistente.Ubicacion = oferta.Ubicacion;
                ofertaExistente.Salario = oferta.Salario;
                ofertaExistente.IdTipoContrato = oferta.IdTipoContrato;
                ofertaExistente.FechaActualizacion = DateTime.Now;

                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Oferta actualizada",
                    Detalle = "La oferta de empleo se ha actualizado correctamente",
                    Resultado = ConvertirAOfertaOutDto(ofertaExistente)
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto EliminarOferta(int idOferta)
        {
            try
            {
                var oferta = _context.OfertaEmpleos.Find(idOferta);
                if (oferta == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Error al eliminar",
                        "La oferta especificada no existe");
                }

                oferta.Estado = "Inactiva";
                _context.SaveChanges();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Oferta eliminada",
                    Detalle = "La oferta de empleo se ha eliminado correctamente"
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ObtenerOferta(int idOferta)
        {
            try
            {
                var oferta = _context.OfertaEmpleos
                    .Include(o => o.IdTipoContratoNavigation)
                    .Include(o => o.IdReclutadorNavigation)
                    .FirstOrDefault(o => o.IdOferta == idOferta);

                if (oferta == null)
                {
                    return RespuestaDto.ParametrosIncorrectos(
                        "Error al obtener oferta",
                        "La oferta especificada no existe");
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Oferta encontrada",
                    Resultado = ConvertirAOfertaOutDto(oferta)
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ListarOfertas(int pagina, int tamanoPagina, string? filtro = null)
        {
            try
            {
                // Construir la consulta con proyección directa
                var query = _context.OfertaEmpleos
                    .Include(o => o.IdTipoContratoNavigation)
                    .Include(o => o.IdReclutadorNavigation)
                    .Where(o => o.Estado == "Activa")
                    .Select(o => new OfertaEmpleoOutDto
                    {
                        IdOferta = o.IdOferta,
                        Titulo = o.Titulo,
                        Descripcion = o.Descripcion,
                        Ubicacion = o.Ubicacion,
                        Salario = o.Salario,
                        TipoContrato = o.IdTipoContratoNavigation.Nombre,
                        FechaPublicacion = o.FechaPublicacion ?? DateTime.Now,
                        NombreReclutador = o.IdReclutadorNavigation.Nombre + " " + o.IdReclutadorNavigation.Apellido,
                        Estado = o.Estado ?? "Activa",
                        IdReclutador = o.IdReclutador
                    });

                if (!string.IsNullOrEmpty(filtro))
                {
                    query = query.Where(o =>
                        o.Titulo.Contains(filtro) ||
                        o.Descripcion.Contains(filtro) ||
                        o.Ubicacion.Contains(filtro));
                }

                // Materializar el conteo total
                var totalRegistros = query.Count();
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);

                // Obtener resultados paginados
                var ofertas = query
                    .OrderByDescending(o => o.FechaPublicacion)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Ofertas encontradas",
                    Resultado = new
                    {
                        Pagina = pagina,
                        TotalPaginas = totalPaginas,
                        TotalRegistros = totalRegistros,
                        Ofertas = ofertas
                    }
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        public RespuestaDto ListarOfertasReclutador(string idReclutador, int pagina, int tamanoPagina)
        {
            try
            {
                var query = _context.OfertaEmpleos
                    .Include(o => o.IdTipoContratoNavigation)
                    .Include(o => o.IdReclutadorNavigation)
                    .Where(o => o.IdReclutador == idReclutador);

                var totalRegistros = query.Count();
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);

                var ofertas = query
                    .OrderByDescending(o => o.FechaPublicacion)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .Select(o => ConvertirAOfertaOutDto(o))
                    .ToList();

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Ofertas encontradas",
                    Resultado = new
                    {
                        Pagina = pagina,
                        TotalPaginas = totalPaginas,
                        TotalRegistros = totalRegistros,
                        Ofertas = ofertas
                    }
                };
            }
            catch (Exception ex)
            {
                return RespuestaDto.ErrorInterno();
            }
        }

        private OfertaEmpleoOutDto ConvertirAOfertaOutDto(OfertaEmpleo oferta)
        {
            if (!_context.Entry(oferta).Reference(o => o.IdReclutadorNavigation).IsLoaded)
            {
                _context.Entry(oferta).Reference(o => o.IdReclutadorNavigation).Load();
            }
            if (!_context.Entry(oferta).Reference(o => o.IdTipoContratoNavigation).IsLoaded)
            {
                _context.Entry(oferta).Reference(o => o.IdTipoContratoNavigation).Load();
            }

            return new OfertaEmpleoOutDto
            {
                IdOferta = oferta.IdOferta,
                Titulo = oferta.Titulo,
                Descripcion = oferta.Descripcion,
                Ubicacion = oferta.Ubicacion,
                Salario = oferta.Salario,
                TipoContrato = oferta.IdTipoContratoNavigation?.Nombre ?? "No especificado",
                FechaPublicacion = oferta.FechaPublicacion ?? DateTime.Now,
                NombreReclutador = oferta.IdReclutadorNavigation != null
                    ? $"{oferta.IdReclutadorNavigation.Nombre} {oferta.IdReclutadorNavigation.Apellido}"
                    : "No especificado",
                Estado = oferta.Estado ?? "Activa",
                // Agregamos el ID del reclutador
                IdReclutador = oferta.IdReclutador
            };
        }
    }
}
