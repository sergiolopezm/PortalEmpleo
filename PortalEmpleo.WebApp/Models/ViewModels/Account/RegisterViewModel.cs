using System.ComponentModel.DataAnnotations;

namespace PortalEmpleo.WebApp.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [Display(Name = "Usuario")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 100 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 50 caracteres")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Tipo de Usuario")]
        public int RolId { get; set; }
    }
}
