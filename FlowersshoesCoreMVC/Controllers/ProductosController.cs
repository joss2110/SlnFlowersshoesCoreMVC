using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class ProductosController : Controller
    {
        List<TbProducto> lista = new List<TbProducto>();

        public async Task<List<TbProducto>> GetProductos()
        {

            using (var httpcliente = new HttpClient())
            {

                var respuesta =
                    await httpcliente.GetAsync(
                        "http://localhost:5050/api/Productos/GetProductos");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbProducto>>(respuestaAPI)!;
            }
        }




        public async Task<string> CrearProducto(TbProducto obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta =
                        await httpcliente.PostAsync("http://localhost:5050/api/Productos/GrabarProducto", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }

        public async Task<string> EditarProducto(TbProducto obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta =
                   await httpcliente.PutAsync("http://localhost:5050/api/Productos/ActualizarProducto", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }



        public async Task<string> EliminarRestaurarProducto(int id, int option)
        {
            string cadena = string.Empty;

            using (var httpClient = new HttpClient())
            {
                if (option == 1)
                {
                    HttpResponseMessage respuesta = await httpClient.DeleteAsync($"http://localhost:5050/api/Productos/DeleteProductos/{id}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
                else
                {
                    HttpResponseMessage respuesta = await httpClient.DeleteAsync($"http://localhost:5050/api/Productos/RestaurarProductos/{id}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
            }

            return cadena;
        }

        [HttpGet]
        public async Task<IActionResult> Productos(int id, string accion)
        {
            lista = await GetProductos();
            ProductosVista viewmodel;
            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new ProductosVista
                {
                    NuevoProductos = new TbProducto(),
                    listaProductos = lista
                };
            }
            else
            {
                viewmodel = new ProductosVista
                {
                    NuevoProductos = lista.Find(c => c.Idpro == id)!,
                    listaProductos = lista
                };
                ViewBag.abrirModal = accion;
            }



            return View(viewmodel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(ProductosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbProducto nuevoProducto = model.NuevoProductos;

                    TempData["mensaje"] = await CrearProducto(nuevoProducto);

                    return RedirectToAction(nameof(Productos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo Agregar un nuevo Registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetProductos();

            var viewmodel = new ProductosVista
            {
                NuevoProductos = new TbProducto(),
                listaProductos = lista
            };

            return View("Productos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProductosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbProducto nuevoProducto = model.NuevoProductos;

                    TempData["mensaje"] = await EditarProducto(nuevoProducto);

                    return RedirectToAction(nameof(Productos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar el registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetProductos();

            var viewmodel = new ProductosVista
            {
                NuevoProductos = new TbProducto(),
                listaProductos = lista
            };

            return View("Productos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(ProductosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbProducto nuevoProducto = model.NuevoProductos;

                    TempData["mensaje"] = await EliminarRestaurarProducto(model.NuevoProductos.Idpro, 1);

                    return RedirectToAction(nameof(Productos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar el registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetProductos();

            var viewmodel = new ProductosVista
            {
                NuevoProductos = new TbProducto(),
                listaProductos = lista
            };

            return View("Productos", viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(ProductosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbProducto nuevoProducto = model.NuevoProductos;

                    TempData["mensaje"] = await EliminarRestaurarProducto(model.NuevoProductos.Idpro, 2);

                    return RedirectToAction(nameof(Productos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar el registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetProductos();

            var viewmodel = new ProductosVista
            {
                NuevoProductos = new TbProducto(),
                listaProductos = lista
            };

            return View("Productos", viewmodel);
        }






    }
}
