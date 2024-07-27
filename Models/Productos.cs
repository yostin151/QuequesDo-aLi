using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace QuequesDoñaLI_CRUD.Models
{
    public class Productos
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nombre { get; set; } = "";
        [MaxLength(100)]
        public string Relleno { get; set; } = "";
        [MaxLength(100)]
        public string Categoria { get; set; } = "";
        [Precision(16, 2)]
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = "";
        [MaxLength(100)]
        public string Imagen { get; set; } = "";
        public DateTime Fecha_Creacion { get; set; }

    }
}
