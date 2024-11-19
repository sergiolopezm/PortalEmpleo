using System;
using System.Collections.Generic;

namespace PortalEmpleo.Infraestructure;

public partial class Log
{
    public int IdLog { get; set; }

    public string? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Tipo { get; set; }

    public string? Ip { get; set; }

    public string? Accion { get; set; }

    public string? Detalle { get; set; }
}
