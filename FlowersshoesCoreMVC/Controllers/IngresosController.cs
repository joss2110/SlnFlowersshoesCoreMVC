using FlowersshoesCoreMVC.Models.Vistas;
using FlowersshoesCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Text;
using System.Net.Http;

namespace FlowersshoesCoreMVC.Controllers
{
    public class IngresosController : Controller
    {
        List<TbIngreso> lista = new List<TbIngreso>();
        List<TbTrabajadore> listaTrabajadores = new List<TbTrabajadore>();
        List<TbProducto> listaProductos = new List<TbProducto>();

        #region Listas

        public async Task<List<TbIngreso>> GetIngresos()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Ingresos/GetIngresos");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbIngreso>>(respuestaAPI)!;
            }
        }

        public async Task<List<TbDetalleIngreso>> GetDetalleIngresos(int idingre)
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync($"http://localhost:5050/api/DetalleIngresos/GetDetalleIngresos/{idingre}");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbDetalleIngreso>>(respuestaAPI)!;
            }
        }

        public async Task<List<TbTrabajadore>> GetTrabajadores()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Trabajadores/GetTrabajadores");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbTrabajadore>>(respuestaAPI)!;
            }
        }

        public async Task<List<TbProducto>> GetProductos()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Productos/GetProductos");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbProducto>>(respuestaAPI)!;
            }
        }

        #endregion

        public async Task<string> CrearIngreso(TbIngreso obj)
        {
            string cadena = string.Empty;
            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                    JsonConvert.SerializeObject(obj), Encoding.UTF8,
                    "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();

                respuesta = await httpcliente.PostAsync("http://localhost:5050/api/Ingresos/GrabarIngresos", contenido);

                TbDetalleIngreso objD = new TbDetalleIngreso();
                objD.Idingre = Convert.ToInt32(await respuesta.Content.ReadAsStringAsync());
                objD.Idpro = obj.Idpro;
                objD.Cantidad = obj.Cantidad;

                StringContent contenidoDetalle = new StringContent(
                    JsonConvert.SerializeObject(objD), Encoding.UTF8,
                    "application/json");

                respuesta = await httpcliente.PostAsync("http://localhost:5050/api/DetalleIngresos/GrabarDetalleIngresos", contenidoDetalle);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }

        public async Task<string> EliminarRestaurarIngreso(int id, int option)
        {
            string cadena = string.Empty;

            using (var httpClient = new HttpClient())
            {
                var ingreso = (await GetIngresos()).Find(x => x.Idingre.Equals(id));
                var detalle = (await GetDetalleIngresos(id)).Find(x => x.Idingre.Equals(id));
                if (option == 1)
                {
                    HttpResponseMessage respuesta = await httpClient.DeleteAsync($"http://localhost:5050/api/Ingresos/EliminarIngresos/{id}");

                    StringContent contenido = new StringContent(
                    JsonConvert.SerializeObject(detalle), Encoding.UTF8,
                    "application/json");
                    respuesta = await httpClient.PutAsync("http://localhost:5050/api/DetalleIngresos/EliminarDetalleIngresos", contenido);

                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
                else
                {
                    StringContent contenido = new StringContent(
                    JsonConvert.SerializeObject(ingreso), Encoding.UTF8,
                    "application/json");

                    HttpResponseMessage respuesta = await httpClient.PutAsync($"http://localhost:5050/api/Ingresos/RestaurarIngresos", contenido);

                    StringContent contenidoD = new StringContent(
                    JsonConvert.SerializeObject(detalle), Encoding.UTF8,
                    "application/json");
                    respuesta = await httpClient.PutAsync("http://localhost:5050/api/DetalleIngresos/RestaurarDetalleIngresos", contenidoD);

                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
            }

            return cadena;
        }

        [HttpGet]
        public async Task<IActionResult> Ingresos(int id, string accion)
        {
            lista = await GetIngresos();
            listaTrabajadores = await GetTrabajadores();
            listaProductos = await GetProductos();
            IngresosVista viewmodel;
            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new IngresosVista
                {
                    NuevoIngreso = new TbIngreso(),
                    listaIngresos = lista,
                    listaTrabajadores = listaTrabajadores,
                    listaProductos = listaProductos
                };
            }
            else
            {
                viewmodel = new IngresosVista
                {
                    NuevoIngreso = lista.Find(c => c.Idingre == id)!,
                    listaIngresos = lista,
                    listaTrabajadores = listaTrabajadores,
                    listaProductos = listaProductos
                };
                ViewBag.abrirModal = accion;
            }

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(IngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbIngreso nuevoIngreso = model.NuevoIngreso;

                    TempData["mensaje"] = await CrearIngreso(nuevoIngreso);

                    return RedirectToAction(nameof(Ingresos));
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

            lista = await GetIngresos();
            listaTrabajadores = await GetTrabajadores();
            listaProductos = await GetProductos();

            var viewmodel = new IngresosVista
            {
                NuevoIngreso = new TbIngreso(),
                listaIngresos = lista,
                listaTrabajadores = listaTrabajadores,
                listaProductos = listaProductos
            };

            return View("Ingresos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(IngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbIngreso nuevoColor = model.NuevoIngreso;

                    TempData["mensaje"] = await EliminarRestaurarIngreso(model.NuevoIngreso.Idingre, 1);

                    return RedirectToAction(nameof(Ingresos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo Eliminar el Registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetIngresos();
            listaTrabajadores = await GetTrabajadores();
            listaProductos = await GetProductos();

            var viewmodel = new IngresosVista
            {
                NuevoIngreso = new TbIngreso(),
                listaIngresos = lista,
                listaTrabajadores = listaTrabajadores,
                listaProductos = listaProductos
            };

            return View("Ingresos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(IngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbIngreso nuevoColor = model.NuevoIngreso;

                    TempData["mensaje"] = await EliminarRestaurarIngreso(model.NuevoIngreso.Idingre, 2);

                    return RedirectToAction(nameof(Ingresos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo Restaurar el Registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetIngresos();
            listaTrabajadores = await GetTrabajadores();
            listaProductos = await GetProductos();

            var viewmodel = new IngresosVista
            {
                NuevoIngreso = new TbIngreso(),
                listaIngresos = lista,
                listaTrabajadores = listaTrabajadores,
                listaProductos = listaProductos
            };

            return View("Ingresos", viewmodel);
        }

    }
}
