using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.WebApp.Models.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
