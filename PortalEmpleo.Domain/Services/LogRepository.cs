using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Infraestructure;

namespace PortalEmpleo.Domain.Services
{
    public class LogRepository : ILogRepository
    {
        private readonly DBContext _context;

        public LogRepository(DBContext context)
        {
            _context = context;
        }

        public void Accion(string? idUsuario, string? ip, string? accion, string? detalle)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, ip, accion, detalle, "200");
        }

        public void Error(string? idUsuario, string? ip, string? accion, string? error)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, ip, accion, error, "500");
        }

        public void Info(string? idUsuario, string? ip, string? accion, string? detalle)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, ip, accion, detalle, "400");
        }

        public void Log(string? idUsuario, string? ip, string? accion, string? detalle, string tipo)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, ip, accion, detalle, tipo);
        }

        private async Task GuardarLogAsync(string? idUsuario, string? ip, string? accion, string? detalle, string tipo)
        {
            var log = new Log
            {
                Fecha = DateTime.Now,
                IdUsuario = idUsuario,
                Ip = ip,
                Accion = accion,
                Tipo = tipo,
                Detalle = detalle
            };

            _context.ChangeTracker.Clear();
            await _context.Logs.AddAsync(log);
            _context.SaveChanges();
        }
    }
}