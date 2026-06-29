using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Obligatorio para usar HttpContext.Session
using LOGIN.Models;
using System;

namespace LOGIN.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // 1. Validar sesión
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Cargar el nombre del usuario logueado
            ViewBag.NombreUsuario = HttpContext.Session.GetString("UsuarioNombre");

            // 3. CALCULO DINÁMICO DE VENTAS (Simulación en lo que conectas tu DbContext)
            // Aquí puedes cambiar este valor manualmente por ahora, o dejarlo así para comprobar que la vista lo lee:
            decimal totalGanadoHoy = 1.50m; 

            // Pasamos el total formateado a la vista con dos decimales
            ViewBag.TotalVentasHoy = totalGanadoHoy.ToString("N2");

            return View();
        }

        public IActionResult Privacy()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public IActionResult Salir()
        {
            // Limpia por completo la sesión al salir
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}