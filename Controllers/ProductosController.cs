using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using QuequesDoñaLI_CRUD.Models;
using QuequesDoñaLI_CRUD.Servicios;

namespace QuequesDoñaLI_CRUD.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var productos = context.Productos.OrderByDescending(p => p.Id).ToList();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.Imagen == null) /*si no esta la imagen hacemos esto*/
            {
                ModelState.AddModelError("Imagen", "la imagen es requerida");
            }
            /*de esta forma si no es valido, retornaremos el view regular*/
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            //guardar nombre de la imagen
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + productDto.Nombre;
            //guardar la extension del documento al mismo nombre
            newFileName += Path.GetExtension(productDto.Imagen!.FileName);

            // este es la ruta pulica de nuestro folder de la aplicacion www root,folder productos + nuevo nombre
            string imageDirectory = Path.Combine(environment.WebRootPath, "Productos");
            string imageFullPath = Path.Combine(imageDirectory, newFileName);
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.Imagen.CopyTo(stream);
            }
            //guardar el nuevo producto en la base de datos.
            //creamos objetos Productos usando la info recibida de ProductDto//
            Productos productos = new Productos()
            {
                Nombre = productDto.Nombre,
                Relleno = productDto.Relleno,
                Categoria = productDto.Categoria,
                Precio = productDto.Precio,
                Descripcion = productDto.Descripcion,
                Imagen = newFileName,
                Fecha_Creacion = DateTime.Now,

            };
            //de esta forma guardamos el objeto productos en la base de datos//
            context.Productos.Add(productos);
            context.SaveChanges();//y de esta forma guardamos los cambios//

            return RedirectToAction("Index", "Productos");
        }

        public IActionResult Editar(int id)
        { //de esta forma leemos el producto
            var producto = context.Productos.Find(id);
            //si no es encontrado hacemos esto
            if (producto == null)
            {
                return RedirectToAction("Index", "Productos");
            }
            //si si se encontro creamos productoDto de producto el cual es llenado con los objetos producto que obtenemos de la base de datos
            var productoDto = new ProductDto()
            {
                Nombre = producto.Nombre,
                Relleno = producto.Relleno,
                Categoria = producto.Categoria,
                Precio = producto.Precio,
                Descripcion = producto.Descripcion,
            };
            ViewData["ProductoId"] = producto.Id;//podemos agregar el product ID al view data
                                                 //dentro de la llave 
            ViewData["Imagen"] = producto.Imagen;
            ViewData["Fecha_Creacion"] = producto.Fecha_Creacion.ToString("MM/dd/yyyy");
            //ahora podemos mostrar el product ID usando el diccionario viewData que menciona el lugar donnde esta la imagen en producto.Imagen
            return View(productoDto);
            // ahora debemos crear un view para editar q debe llamarse igual
        }
        [HttpPost]
        public IActionResult Editar(int id, ProductDto productDto)
        {
            var producto = context.Productos.Find(id);
            //si no es encontrado hacemos esto
            if (producto == null)
            {
                return RedirectToAction("Index", "Productos");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductoId"] = producto.Id;//podemos agregar el product ID al view data
                                                     //dentro de la llave 
                ViewData["Imagen"] = producto.Imagen;
                ViewData["Fecha_Creacion"] = producto.Fecha_Creacion.ToString("MM/dd/yyyy");
                return View(productDto);

            }

            // ACTUALIZAR la imagen si tenemos una nueva imagen
            string newFileName = producto.Imagen; //aqui igualamos el nuevo nombre con el nombre viejo
            if ( productDto.Imagen != null) // si tenemos nueva imagen entonces actualizaremos el nuevo valor
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.Imagen.FileName);
                //el nombre de la ruta para la actualizacion(update)
                string imageFullPath = environment.WebRootPath + "/productos/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                { 
                    productDto.Imagen.CopyTo(stream); //aqui guardamos la nueva imagen el stream
                }

                //borramos la imgen vieja
                String oldImageFullPath = environment.WebRootPath + "/productos/" + producto.Imagen; //aqui se encuentra la imagen vieja y el file name
                System.IO.File.Delete(oldImageFullPath);// ESTE comando permite borrar la imagen que esta disponible en el path 

            }

            //update-actualizar el producto en la base de datos
            producto.Nombre = productDto.Nombre;// de los datos enviados
            producto.Relleno = productDto.Relleno;
            producto.Categoria = productDto.Categoria;
            producto.Precio = productDto.Precio;
            producto.Descripcion = productDto.Descripcion;
            producto.Imagen = newFileName;

            context.SaveChanges();
            return RedirectToAction("Index", "Productos");
        }
        public IActionResult Delete(int id)
        {
            var producto = context.Productos.Find(id);
            if (producto == null) 
            {
                return RedirectToAction("Index", "Productos");
                // si no encontramos a nadie con ese ID, entonces lo redireccionamos al area principal
            }
            string imageFullPath = environment.WebRootPath + "/productos" + producto.Imagen;
            System.IO.File.Delete(imageFullPath);// borramos imagen que esta en ese path        

            //borramos el producto de la base de datos (tabla Productos)
            // y luego borraremos el producto solamente
            context.Productos.Remove(producto);
            context.SaveChanges(true);// aqui guardamos las notificaciones

            return RedirectToAction("Index", "Productos");
        }

        public IActionResult Logout()
        {
            // Aquí puedes manejar cualquier lógica de cierre de sesión si es necesario.
            // Por ejemplo, puedes limpiar cualquier sesión o cookie.

            // Redireccionar a la página principal
            return Redirect("https://xn--quequesdoali20240627004813-prc.azurewebsites.net/");
        }

    }
}
