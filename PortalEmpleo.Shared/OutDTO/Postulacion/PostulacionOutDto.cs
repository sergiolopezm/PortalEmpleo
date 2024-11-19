namespace PortalEmpleo.Shared.OutDTO.Postulacion
{
    public class PostulacionOutDto
    {
        public int IdPostulacion { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime FechaPostulacion { get; set; }
    }
}
