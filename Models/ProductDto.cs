using System.ComponentModel.DataAnnotations;

namespace QuequesDoñaLI_CRUD.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = "";
        [Required, MaxLength(100)]
        public string Relleno { get; set; } = "";
        [Required, MaxLength(100)]
        public string Categoria { get; set; } = "";
        [Required]
        public decimal Precio { get; set; }
        [Required]public string Descripcion { get; set; } = "";
        public IFormFile? Imagen { get; set; } 
    }
}
