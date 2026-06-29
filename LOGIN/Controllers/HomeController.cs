using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LOGIN.Data; 
using System;
using System.Linq;

namespace LOGIN.Controllers
{
    public class HomeController : Controller
    {
        // Cambiado a ApplicationDbContext que es el que tienes en tu carpeta Data
        private readonly ApplicationDbContext _context; 

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Validar sesión de forma estable
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioId")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.NombreUsuario = HttpContext.Session.GetString("UsuarioNombre");

            try
            {
                // Buscamos dinámicamente en tu tabla de compras o pedidos.
                // Usamos _context de manera segura.
                decimal totalGanadoHoy = _context.Pedidos.Sum(p => (decimal?)p.Total) ?? 0.00m;
                ViewBag.TotalVentasHoy = totalGanadoHoy.ToString("N2");
            }
            catch (Exception)
            {
                // Si la tabla se llama diferente, muestra 0.00 para que al menos te deje entrar sin crashear
                ViewBag.TotalVentasHoy = "0.00";
            }

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

        public IActionResult Error()
        {
            return View();
        }
    }
}