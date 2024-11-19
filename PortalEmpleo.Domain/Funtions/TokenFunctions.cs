using Microsoft.IdentityModel.Tokens;
using PortalEmpleo.Infraestructure;
using PortalEmpleo.Shared.GeneralDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PortalEmpleo.Domain.Services
{
    public partial class TokenRepository 
    {
        private string CrearTokenUsuario(Usuario usuario, string ip)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("Random", new Random().Next(100000000, 1000000000).ToString()));
            claims.AddClaim(new Claim("IdUsuario", usuario.IdUsuario));
            claims.AddClaim(new Claim("Usuario", usuario.NombreUsuario));
            claims.AddClaim(new Claim("Nombre", usuario.Nombre));
            claims.AddClaim(new Claim("Apellido", usuario.Apellido));
            claims.AddClaim(new Claim("Rol", usuario.Rol.Nombre));
            claims.AddClaim(new Claim("Ip", ip));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(_keyBytes),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(_tiempoExpiracion),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(tokenConfig);
        }

        private void GuardarTokenBD(string idToken, Usuario usuario, string ip)
        {
            Token token = new Token
            {
                IdToken = idToken,
                IdUsuario = usuario.IdUsuario,
                Ip = ip,
                FechaAutenticacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(_tiempoExpiracionBD),
            };


            _context.Tokens.Add(token);
            _context.SaveChanges();

        }

        private void RemoverTokensExpirados(Usuario usuario)
        {
            Token TokenUsuarioYaAutenticado = _context.Tokens.Where(t => t.IdUsuario == usuario.IdUsuario && t.FechaExpiracion > DateTime.Now).FirstOrDefault()!;
            List<Token> TokensUsuarioExpirado = _context.Tokens.Where(u => u.IdUsuario == usuario.IdUsuario && u.FechaExpiracion < DateTime.Now).ToList();

            if (TokensUsuarioExpirado != null)
            {
                List<TokenExpirado> tokensExpirados = ConvertirListJwtUsuarioExpiradoAListJwtUsuario(TokensUsuarioExpirado);
                _context.TokenExpirados.AddRange(tokensExpirados);
                _context.Tokens.RemoveRange(TokensUsuarioExpirado);
            }

            if (TokenUsuarioYaAutenticado != null)
            {
                TokenUsuarioYaAutenticado.Observacion = "La sesión ha caducado debido a que el usuario ha ingresado desde otro equipo";
                TokenUsuarioYaAutenticado.FechaExpiracion = DateTime.Now;
            }

        }

        private ValidoDTO ValidarTokenEnSistema(string idToken, string idUsuario, string ip)
        {
            if (idToken == null || idUsuario == null || ip == null)
            {
                return ValidoDTO.Invalido("La información referente a la sesión de usuario esta incompleta");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_keyBytes),
                ValidateIssuer = false, // Puedes configurar esto según tus necesidades
                ValidateAudience = false, // Puedes configurar esto según tus necesidades
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Sin margen de tiempo para la expiración
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(idToken, validationParameters, out _);

                string claimIdUsuario = principal.FindFirst("IdUsuario")!.Value;
                string claimIp = principal.FindFirst("Ip")!.Value;
                if (idUsuario != claimIdUsuario || ip != claimIp)
                {
                    return ValidoDTO.Invalido("La información referente a la sesión de usuario es incorrecta");
                }

                return ValidoDTO.Valido();
            }
            catch
            {
                return ValidoDTO.Invalido("La sesión de usuario no se encuentra activa o no existe en el sistema. Por favor, inicie sesión");
            }
        }

        private ValidoDTO ValidarTokenEnBD(string idToken)
        {
            Token token = _context.Tokens.FirstOrDefault(t => t.IdToken == idToken)!;
            int i = idToken.Length;
            if (token == null)
            {
                return ValidoDTO.Invalido("La sesión de usuario no se encuentra activa. Por favor, inicie sesión");
            }

            if (token.FechaExpiracion < DateTime.Now)
            {
                if (token.Observacion == null || token.Observacion == "")
                {
                    return ValidoDTO.Invalido("La sesión de usuario a caducado por tiempo de inactividad. Por favor, inicie sesión nuevamente");
                }
                return ValidoDTO.Invalido(token.Observacion);
            }

            return ValidoDTO.Valido();
        }

        private List<TokenExpirado> ConvertirListJwtUsuarioExpiradoAListJwtUsuario(List<Token> tokens)
        {
            List<TokenExpirado> TokenUsuarioExpirados = new List<TokenExpirado>();
            foreach (Token token in tokens)
            {
                TokenUsuarioExpirados.Add(new TokenExpirado
                {
                    IdToken = token.IdToken,
                    IdUsuario = token.IdUsuario,
                    Ip = token.Ip,
                    FechaAutenticacion = token.FechaAutenticacion,
                    FechaExpiracion = token.FechaExpiracion,
                    Observacion = token.Observacion
                });
            }
            return TokenUsuarioExpirados;
        }
    }
}