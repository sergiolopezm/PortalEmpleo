using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.Shared.InDTO.OfertaEmpleo;
using PortalEmpleo.WebApp.Models.ViewModels.JobOffers;
using PortalEmpleo.WebApp.Services.Interfaces;

namespace PortalEmpleo.WebApp.Controllers
{
    [Authorize(Policy = "RequireReclutador")]
    public class JobOffersController : Controller
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly IAuthService _authService;

        public JobOffersController(
            IJobOfferService jobOfferService,
            IAuthService authService)
        {
            _jobOfferService = jobOfferService;
            _authService = authService;
        }

        // GET: JobOffers
        public async Task<IActionResult> Index(JobOfferSearchViewModel searchModel)
        {
            var resultado = await _jobOfferService.ListJobOffersAsync(
                searchModel.CurrentPage,
                searchModel.PageSize,
                searchModel.SearchTerm);

            if (!resultado.Exito)
            {
                TempData["ErrorMessage"] = resultado.Detalle;
                return View(new JobOfferSearchViewModel());
            }

            searchModel.Results = (JobOfferListViewModel)resultado.Resultado;
            return View(searchModel);
        }

        // GET: JobOffers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var resultado = await _jobOfferService.GetJobOfferAsync(id);

            if (!resultado.Exito)
            {
                TempData["ErrorMessage"] = resultado.Detalle;
                return RedirectToAction(nameof(Index));
            }

            return View((JobOfferViewModel)resultado.Resultado);
        }

        // GET: JobOffers/Create
        [Authorize]
        public IActionResult Create()
        {
            var user = _authService.GetCurrentUserAsync().Result;
            if (user?.Rol != "Reclutador")
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new JobOfferCreateEditViewModel());
        }

        // POST: JobOffers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(JobOfferCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ofertaDto = new OfertaEmpleoDto
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Ubicacion = model.Ubicacion,
                Salario = model.Salario,
                IdTipoContrato = model.IdTipoContrato
            };

            var resultado = await _jobOfferService.CreateJobOfferAsync(ofertaDto);

            if (resultado.Exito)
            {
                TempData["SuccessMessage"] = "Oferta creada exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, resultado.Detalle ?? "Error al crear la oferta");
            return View(model);
        }

        // GET: JobOffers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var user = _authService.GetCurrentUserAsync().Result;
            if (user?.Rol != "Reclutador")
            {
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _jobOfferService.GetJobOfferAsync(id);

            if (!resultado.Exito)
            {
                TempData["ErrorMessage"] = resultado.Detalle;
                return RedirectToAction(nameof(Index));
            }

            var oferta = (JobOfferViewModel)resultado.Resultado;

            if (oferta.IdReclutador != user.IdUsuario)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new JobOfferCreateEditViewModel
            {
                Titulo = oferta.Titulo,
                Descripcion = oferta.Descripcion,
                Ubicacion = oferta.Ubicacion,
                Salario = oferta.Salario
            };

            return View(model);
        }

        // POST: JobOffers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, JobOfferCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ofertaDto = new OfertaEmpleoDto
            {
                Titulo = model.Titulo,
                Descripcion = model.Descripcion,
                Ubicacion = model.Ubicacion,
                Salario = model.Salario,
                IdTipoContrato = model.IdTipoContrato
            };

            var resultado = await _jobOfferService.UpdateJobOfferAsync(id, ofertaDto);

            if (resultado.Exito)
            {
                TempData["SuccessMessage"] = "Oferta actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, resultado.Detalle ?? "Error al actualizar la oferta");
            return View(model);
        }

        // POST: JobOffers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = _authService.GetCurrentUserAsync().Result;
            if (user?.Rol != "Reclutador")
            {
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _jobOfferService.DeleteJobOfferAsync(id);

            if (resultado.Exito)
            {
                TempData["SuccessMessage"] = "Oferta eliminada exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = resultado.Detalle;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobOffers/MyOffers
        [Authorize]
        public async Task<IActionResult> MyOffers(int page = 1)
        {
            var user = _authService.GetCurrentUserAsync().Result;
            if (user?.Rol != "Reclutador")
            {
                return RedirectToAction(nameof(Index));
            }

            var resultado = await _jobOfferService.ListRecruiterJobOffersAsync(user.IdUsuario, page, 10);

            if (!resultado.Exito)
            {
                TempData["ErrorMessage"] = resultado.Detalle;
                return RedirectToAction(nameof(Index));
            }

            return View((JobOfferListViewModel)resultado.Resultado);
        }
    }
}
