using System;
using System.Collections.Generic;

namespace PortalEmpleo.Infraestructure;

public partial class Postulacion
{
    public int IdPostulacion { get; set; }

    public int IdOferta { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? FechaPostulacion { get; set; }

    public virtual OfertaEmpleo IdOfertaNavigation { get; set; } = null!;
}
