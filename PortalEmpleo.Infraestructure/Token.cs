using System;
using System.Collections.Generic;

namespace PortalEmpleo.Infraestructure;

public partial class Token
{
    public string IdToken { get; set; } = null!;

    public string? IdUsuario { get; set; }

    public string? Ip { get; set; }

    public DateTime? FechaAutenticacion { get; set; }

    public DateTime? FechaExpiracion { get; set; }

    public string? Observacion { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
