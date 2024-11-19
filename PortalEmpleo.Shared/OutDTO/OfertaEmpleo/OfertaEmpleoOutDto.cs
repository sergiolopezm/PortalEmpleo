namespace PortalEmpleo.Shared.OutDTO.OfertaEmpleo
{
    public class OfertaEmpleoOutDto
    {
        public int IdOferta { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public decimal? Salario { get; set; }
        public string TipoContrato { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
        public string NombreReclutador { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string IdReclutador { get; set; } = null!;
    }
}
