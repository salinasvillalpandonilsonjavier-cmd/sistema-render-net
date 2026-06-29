using System;

namespace LOGIN.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int Cantidad { get; set; }
        
        // CORREGIDO: Cambiado de string a decimal para que se puedan hacer multiplicaciones matemáticas
        public decimal Precio { get; set; } 
        
        public string? ImagenUrl { get; set; } = "/images/default-product.png";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}