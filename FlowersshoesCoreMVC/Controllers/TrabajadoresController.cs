using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using PrjFlowersshoesAPI.Models;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class TrabajadoresController : Controller
    {
        private readonly flowersshoesContext db;

        public TrabajadoresController(flowersshoesContext ctx)
        {
            db = ctx;
        }

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

        List<PA_LISTAR_TRABAJADORES> listatra = new List<PA_LISTAR_TRABAJADORES>();

        public async Task<List<PA_LISTAR_TRABAJADORES>> GetTrabajadores()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Trabajadores/GetTrabajadores");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PA_LISTAR_TRABAJADORES>>(respuestaAPI)!;
            }
        }

        public async Task<List<TbRole>> GetRoles()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Roles/GetRoles");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbRole>>(respuestaAPI)!;
            }
        }

        public async Task<string> CrearTrabajador(TbTrabajadore obj)
        {
            string cadena = String.Empty;

            using (var httpcliente = new HttpClient())
            {
                StringContent contenido = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta = await httpcliente.PostAsync("http://localhost:5050/api/Trabajadores/GrabarTrabajadores", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            //
            return cadena;
        }

        public async Task<string> EditarTrabajador(TbTrabajadore obj)
        {
            string cadena = String.Empty;

            using (var httpcliente = new HttpClient())
            {
                StringContent contenido = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta = await httpcliente.PutAsync("http://localhost:5050/api/Trabajadores/ActualizarTrabajadores", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            //
            return cadena;
        }

        public async Task<string> EliminarRestaurarTrabajador(int idtra, int opc)
        {
            string cadena = String.Empty;
            using (var httpcliente = new HttpClient())
            {
                if (opc == 1)
                {
                    HttpResponseMessage respuesta = await httpcliente.DeleteAsync($"http://localhost:5050/api/Trabajadores/DeleteTrabajadores/{idtra}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                } else
                {
                    HttpResponseMessage respuesta = await httpcliente.DeleteAsync($"http://localhost:5050/api/Trabajadores/RestaurarTrabajadores/{idtra}");
                    cadena = await respuesta.Content.ReadAsStringAsync();
                }
            }
            //
            return cadena;
        }

        [HttpGet]
        public async Task<IActionResult> Trabajadores(int id, string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            listatra = await GetTrabajadores();
            TrabajadoresVista viewmodel;
            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new TrabajadoresVista
                {
                    NuevoTrabajador = new TbTrabajadore(),
                    listaTrabajadores = listatra
                };
            } else
            {
                viewmodel = new TrabajadoresVista
                {
                    NuevoTrabajador = db.TbTrabajadores.Find(id)!,
                    listaTrabajadores = listatra
                };
                ViewBag.abrirModal = accion;
            }
            ViewBag.roles =
                new SelectList(await GetRoles(), "Idrol", "NomRol");
            //
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(TrabajadoresVista model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TbTrabajadore nuevoTrabajador = model.NuevoTrabajador;
                    TempData["mensaje"] = await CrearTrabajador(nuevoTrabajador);
                    return RedirectToAction(nameof(Trabajadores));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo agregar un nuevo trabajador";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            listatra = await GetTrabajadores();

            var viewmodel = new TrabajadoresVista
            {
                NuevoTrabajador = new TbTrabajadore(),
                listaTrabajadores = listatra
            };

            ViewBag.roles =
                new SelectList(await GetRoles(), "Idrol", "NomRol");
            return View("Trabajadores", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(TrabajadoresVista model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TbTrabajadore nuevoTrabajador = model.NuevoTrabajador;
                    TempData["mensaje"] = await EditarTrabajador(nuevoTrabajador);
                    return RedirectToAction(nameof(Trabajadores));
                } else
                {
                    TempData["mensaje"] = "No se pudo editar al trabajador";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Erorr: " + ex.Message;
            }

            listatra = await GetTrabajadores();

            var viewmodel = new TrabajadoresVista
            {
                NuevoTrabajador = new TbTrabajadore(),
                listaTrabajadores = listatra
            };
            //
            ViewBag.roles =
                new SelectList(await GetRoles(), "Idrol", "NomRol");
            return View("Trabajadores", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(TrabajadoresVista model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TbTrabajadore nuevoTrabajador = model.NuevoTrabajador;
                    TempData["mensaje"] = await EliminarRestaurarTrabajador(model.NuevoTrabajador.Idtra, 1);
                    return RedirectToAction(nameof(Trabajadores));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar al trabajador";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            listatra = await GetTrabajadores();

            var viewmodel = new TrabajadoresVista
            {
                NuevoTrabajador = new TbTrabajadore(),
                listaTrabajadores = listatra
            };
            //
            ViewBag.roles =
                new SelectList(await GetRoles(), "Idrol", "NomRol");
            return View("Trabajadores", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restaurar(TrabajadoresVista model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TbTrabajadore nuevoTrabajador = model.NuevoTrabajador;
                    TempData["mensaje"] = await EliminarRestaurarTrabajador(model.NuevoTrabajador.Idtra, 2);
                    return RedirectToAction(nameof(Trabajadores));
                } else
                {
                    TempData["mensaje"] = "No se puedo editar al trabajador";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            listatra = await GetTrabajadores();

            var viewmodel = new TrabajadoresVista
            {
                NuevoTrabajador = new TbTrabajadore(),
                listaTrabajadores = listatra
            };
            //
            ViewBag.roles =
                new SelectList(await GetRoles(), "Idrol", "NomRol");
            return View("Trabajadores", viewmodel);
        }

    }
}
