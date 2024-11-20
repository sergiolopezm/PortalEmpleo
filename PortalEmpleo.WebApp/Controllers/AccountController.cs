using Microsoft.AspNetCore.Mvc;
using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Shared.InDTO;
using PortalEmpleo.WebApp.Models.ViewModels.Account;
using PortalEmpleo.WebApp.Services.Interfaces;
using System;

namespace PortalEmpleo.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogRepository _logRepository;

        public AccountController(IAuthService authService, ILogRepository logRepository)
        {
            _authService = authService;
            _logRepository = logRepository;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (_authService.IsAuthenticated())
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = new UsuarioLoginDto
            {
                NombreUsuario = model.NombreUsuario,
                Contraseña = model.Contraseña,
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString()
            };

            var result = await _authService.LoginAsync(loginDto);

            if (result.Exito)
            {
                var currentUser = await _authService.GetCurrentUserAsync();

                if (currentUser?.Rol == "Reclutador")
                {
                    // Cambiamos a redirección a Index
                    return RedirectToAction("Index", "JobOffers");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.Detalle ?? "Error al iniciar sesión");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_authService.IsAuthenticated())
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registroDto = new UsuarioRegistroDto
            {
                NombreUsuario = model.NombreUsuario,
                Contraseña = model.Contraseña,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                RolId = model.RolId
            };

            var result = await _authService.RegisterAsync(registroDto);

            if (result.Exito)
            {
                TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicie sesión.";
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, result.Detalle ?? "Error al registrar usuario");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
