using System;
using System.Collections.Generic;

namespace PortalEmpleo.Infraestructure;

public partial class TiposContrato
{
    public int IdTipoContrato { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<OfertaEmpleo> OfertaEmpleos { get; set; } = new List<OfertaEmpleo>();
}
