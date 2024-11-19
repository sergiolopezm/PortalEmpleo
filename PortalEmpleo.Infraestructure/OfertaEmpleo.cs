using System;
using System.Collections.Generic;

namespace PortalEmpleo.Infraestructure;

public partial class OfertaEmpleo
{
    public int IdOferta { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Ubicacion { get; set; } = null!;

    public decimal? Salario { get; set; }

    public int IdTipoContrato { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Estado { get; set; }

    public string IdReclutador { get; set; } = null!;

    public virtual Usuario IdReclutadorNavigation { get; set; } = null!;

    public virtual TiposContrato IdTipoContratoNavigation { get; set; } = null!;

    public virtual ICollection<Postulacion> Postulacions { get; set; } = new List<Postulacion>();
}
