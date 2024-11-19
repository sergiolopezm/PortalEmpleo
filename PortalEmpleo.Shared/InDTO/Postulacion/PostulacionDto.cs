using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.Shared.InDTO.Postulacion
{
    public class PostulacionDto
    {
        [Required(ErrorMessage = "El ID de la oferta es requerido")]
        public int IdOferta { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Email { get; set; } = null!;
    }
}
