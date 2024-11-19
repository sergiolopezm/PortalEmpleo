using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.Shared.InDTO.OfertaEmpleo
{
    public class OfertaEmpleoDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La ubicación es requerida")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres")]
        public string Ubicacion { get; set; } = null!;

        public decimal? Salario { get; set; }

        [Required(ErrorMessage = "El tipo de contrato es requerido")]
        public int IdTipoContrato { get; set; }
    }
}
