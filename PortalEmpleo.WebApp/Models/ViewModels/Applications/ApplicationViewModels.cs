using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.WebApp.Models.ViewModels.Applications
{
    public class ApplicationListViewModel
    {
        public int Pagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public List<ApplicationViewModel> Postulaciones { get; set; }
        public int IdOferta { get; set; }
    }

    public class ApplicationViewModel
    {
        public int IdPostulacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaPostulacion { get; set; }
    }

    public class CreateApplicationViewModel
    {
        public int IdOferta { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}
