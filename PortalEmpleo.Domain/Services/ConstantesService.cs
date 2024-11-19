using Microsoft.Extensions.Options;
using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Shared.InDTO;

namespace PortalEmpleo.Domain.Services
{
    public class ConstantesService : IConstantesService
    {
        private readonly UsuarioSettings _settings;

        public ConstantesService(IOptions<UsuarioSettings> settings)
        {
            _settings = settings.Value;
        }

        public string ObtenerPrefijoUsuario(int rolId)
        {
            return rolId switch
            {
                var id when id == _settings.Roles.Admin => _settings.Prefijos.Admin,
                var id when id == _settings.Roles.Reclutador => _settings.Prefijos.Reclutador,
                var id when id == _settings.Roles.Candidato => _settings.Prefijos.Candidato,
                _ => throw new ArgumentException($"RolId no válido: {rolId}")
            };
        }

        public bool EsRolValido(int rolId)
        {
            return ObtenerRolesValidos().Contains(rolId);
        }

        public int[] ObtenerRolesValidos()
        {
            return new[]
            {
            _settings.Roles.Admin,
            _settings.Roles.Reclutador,
            _settings.Roles.Candidato
        };
        }
    }
}
