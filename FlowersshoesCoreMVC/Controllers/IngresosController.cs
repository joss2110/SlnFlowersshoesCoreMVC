using FlowersshoesCoreMVC.Models.Vistas;
using FlowersshoesCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class IngresosController : Controller
    {
        List<TbIngreso> lista = new List<TbIngreso>();
        List<TbTrabajadore> listaTrabajadores = new List<TbTrabajadore>();
        List<TbProducto> listaProductos = new List<TbProducto>();

        public async Task<List<TbIngreso>> GetIngresos()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Ingresos/GetIngresos");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbIngreso>>(respuestaAPI)!;
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

    }
}
