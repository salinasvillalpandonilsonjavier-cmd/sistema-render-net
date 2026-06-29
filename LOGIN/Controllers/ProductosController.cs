using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LOGIN.Data;
using LOGIN.Models;

namespace LOGIN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado" });
            return producto;
        }

        // POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Asigna el objeto DateTime directo para sincronizar con la base de datos
            producto.FechaRegistro = DateTime.Now;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

        // PUT: api/Productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.Id) return BadRequest(new { mensaje = "El ID no coincide" });
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existente = await _context.Productos.FindAsync(id);
            if (existente == null) return NotFound(new { mensaje = "Producto no encontrado" });

            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Cantidad = producto.Cantidad;
            existente.Precio = producto.Precio;
            
            // Guarda los cambios de la URL de la imagen enviados desde el formulario
            existente.ImagenUrl = producto.ImagenUrl; 

            await _context.SaveChangesAsync();
            return Ok(existente);
        }

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado" });

            try
            {
                // 1. Limpiar primero las dependencias en el carrito para evitar romper la llave foránea
                var itemsCarrito = _context.CarritoItems.Where(c => c.ProductoId == id);
                _context.CarritoItems.RemoveRange(itemsCarrito);

                // 2. Eliminar de forma segura el producto de la tabla principal
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Producto eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al eliminar", detalle = ex.Message });
            }
        }
    }
}