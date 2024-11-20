using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.WebApp.Models;
using PortalEmpleo.WebApp.Services.Interfaces;
using System.Diagnostics;

namespace PortalEmpleo.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly IAuthService _authService;

        public HomeController(
            IJobOfferService jobOfferService,
            IAuthService authService)
        {
            _jobOfferService = jobOfferService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _authService.GetCurrentUserAsync();

            if (currentUser?.Rol == "Reclutador")
            {
                // Para reclutadores, mostrar sus últimas ofertas publicadas
                var resultado = await _jobOfferService.ListRecruiterJobOffersAsync(currentUser.IdUsuario, 1, 5);
                ViewBag.MisOfertas = resultado.Exito ? resultado.Resultado : null;
            }
            else
            {
                // Para candidatos y usuarios no autenticados, mostrar las últimas ofertas
                var resultado = await _jobOfferService.ListJobOffersAsync(1, 5);
                ViewBag.UltimasOfertas = resultado.Exito ? resultado.Resultado : null;
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
