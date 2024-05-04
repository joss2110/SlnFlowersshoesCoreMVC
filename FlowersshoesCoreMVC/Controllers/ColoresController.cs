using Microsoft.AspNetCore.Mvc;
using FlowersshoesCoreMVC.Models;
using Newtonsoft.Json;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class ColoresController : Controller
    {

        List<TbColores> lista = new List<TbColores>();

        public async Task<List<TbColores>> GetColores()
        {
            using(var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Colores/GetColores");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbColores>>(respuestaAPI)!;
            }
        }

        public async Task<string> CrearColor(TbColores obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta =await httpcliente.PostAsync("http://localhost:5050/api/Colores/GrabarColor", contenido);
               
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }

        public async Task<string> EditarColor(TbColores obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta = await httpcliente.PutAsync("http://localhost:5050/api/Colores/ActualizarColor", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }

        [HttpGet]
        public async Task<IActionResult> Colores(int id)
        {
            lista = await GetColores();
            ColoresVista viewmodel;
            ViewBag.abrirModal = false;

            if (id == 0)
            {
                viewmodel = new ColoresVista
                {
                    NuevoColor = new TbColores(),
                    listaColores = lista
                };
            }
            else
            {
                viewmodel = new ColoresVista
                {
                    NuevoColor = lista.Find(c => c.Idcolor == id)!,
                    listaColores = lista
                };
                ViewBag.abrirModal = true;
            }

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(ColoresVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbColores nuevoColor = model.NuevoColor;

                    TempData["mensaje"] = await CrearColor(nuevoColor);

                    return RedirectToAction(nameof(Colores));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo agregar un nuevo Registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetColores();

            var viewmodel = new ColoresVista
            {
                NuevoColor = new TbColores(),
                listaColores = lista
            };

            return View("Colores", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ColoresVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbColores nuevoColor = model.NuevoColor;

                    TempData["mensaje"] = await EditarColor(nuevoColor);

                    return RedirectToAction(nameof(Colores));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo agregar un nuevo Registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            lista = await GetColores();

            var viewmodel = new ColoresVista
            {
                NuevoColor = new TbColores(),
                listaColores = lista
            };

            return View("Colores", viewmodel);
        }

    }
}
