using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.WebApp.Models.ViewModels.JobOffers
{
    public class JobOfferCreateEditViewModel
    {
        public int IdOferta { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [Display(Name = "Título")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        [Display(Name = "Ubicación")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres")]
        public string Ubicacion { get; set; }

        [Display(Name = "Salario")]
        public decimal? Salario { get; set; }

        [Required(ErrorMessage = "El tipo de contrato es requerido")]
        [Display(Name = "Tipo de Contrato")]
        public int IdTipoContrato { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public string Estado { get; set; }
    }

}
