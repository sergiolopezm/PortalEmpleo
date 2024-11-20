using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.Shared.InDTO.Postulacion;
using PortalEmpleo.WebApp.Models.ViewModels.Applications;
using PortalEmpleo.WebApp.Services.Interfaces;

namespace PortalEmpleo.WebApp.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IAuthService _authService;

        public ApplicationController(
            IApplicationService applicationService,
            IAuthService authService)
        {
            _applicationService = applicationService;
            _authService = authService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(CreateApplicationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { exito = false, detalle = "Datos inválidos" });
            }

            var postulacion = new PostulacionDto
            {
                IdOferta = model.IdOferta,
                Nombre = model.Nombre,
                Email = model.Email
            };

            var resultado = await _applicationService.CreateApplicationAsync(postulacion);

            return Json(new
            {
                exito = resultado.Exito,
                detalle = resultado.Detalle
            });
        }

        [Authorize]
        public async Task<IActionResult> ListForOffer(int idOferta, int pagina = 1)
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user?.Rol != "Reclutador")
            {
                return RedirectToAction("Index", "Home");
            }

            var resultado = await _applicationService.ListApplicationsForOfferAsync(idOferta, pagina, 10);

            if (!resultado.Exito)
            {
                TempData["ErrorMessage"] = resultado.Detalle;
                return RedirectToAction("MyOffers", "JobOffers");
            }

            return View((ApplicationListViewModel)resultado.Resultado);
        }
    }
}
