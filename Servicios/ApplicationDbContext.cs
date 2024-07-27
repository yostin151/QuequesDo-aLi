using Microsoft.EntityFrameworkCore;
using QuequesDoñaLI_CRUD.Models;

namespace QuequesDoñaLI_CRUD.Servicios
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet <Productos> Productos { get; set; }
//aqui agregamos el enlace con de la base de datos y que obtenga
//los nombres de productos//
    }
}
