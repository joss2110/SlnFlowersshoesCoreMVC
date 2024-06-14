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

        TbTrabajadore? RecuperarTrabajador()
        {
            var trabajadorJson = HttpContext.Session.GetString("trabajadorActual");

            if (!string.IsNullOrEmpty(trabajadorJson))
            {
                try
                {
                    return JsonConvert.DeserializeObject<TbTrabajadore>(trabajadorJson);
                }
                catch
                {
                    HttpContext.Session.Remove("trabajadorActual");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        TbTrabajadore trabajadorActual = new TbTrabajadore();

        void GrabarTrabajador()
        {
            HttpContext.Session.SetString("trabajadorActual",
                    JsonConvert.SerializeObject(trabajadorActual));
        }

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

        public async Task<string> EliminarRestaurarColor(int id, int option)
        {
            string cadena = string.Empty;

            using (var httpClient = new HttpClient())
            {
                if (option == 1)
                {
                    HttpResponseMessage respuesta = await httpClient.DeleteAsync($"http://localhost:5050/api/Colores/EliminarColor/{id}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
                else
                {
                    HttpResponseMessage respuesta = await httpClient.DeleteAsync($"http://localhost:5050/api/Colores/RestaurarColor/{id}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
            }

            return cadena;
        }



        [HttpGet]
        public async Task<IActionResult> Colores(int id,string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            lista = await GetColores();
            ColoresVista viewmodel;
            ViewBag.abrirModal = "No";

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
                ViewBag.abrirModal = accion;
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
                    TempData["mensaje"] = "No se pudo Agregar un nuevo Registro, intentalo nuevamente";
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
                    TempData["mensaje"] = "No se pudo Editar el Registro, intentalo nuevamente";
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
        public async Task<IActionResult> Eliminar(ColoresVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbColores nuevoColor = model.NuevoColor;

                    TempData["mensaje"] = await EliminarRestaurarColor(model.NuevoColor.Idcolor,1);

                    return RedirectToAction(nameof(Colores));
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
        public async Task<IActionResult> Restaurar(ColoresVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbColores nuevoColor = model.NuevoColor;

                    TempData["mensaje"] = await EliminarRestaurarColor(model.NuevoColor.Idcolor, 2);

                    return RedirectToAction(nameof(Colores));
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
