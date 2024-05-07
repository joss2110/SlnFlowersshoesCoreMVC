using FlowersshoesCoreMVC.DAO;
using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class VentasController : Controller
    {
        private readonly flowersshoesContext db;
        private readonly VentasDAO dao;

        public VentasController(flowersshoesContext ctx, VentasDAO _dao)
        {
            db = ctx;
            dao = _dao;
        }



        public async Task<string> crearCliente(TbCliente obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta =
                        await httpcliente.PostAsync("http://localhost:5050/api/Clientes/GrabarClientes", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }

        public async Task<string> EditarCliente(TbCliente obj)
        {
            string cadena = string.Empty;


            using (var httpcliente = new HttpClient())
            {

                StringContent contenido = new StringContent(
                   JsonConvert.SerializeObject(obj), Encoding.UTF8,
                          "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();
                respuesta =
                   await httpcliente.PutAsync("http://localhost:5050/api/Clientes/ActualizarClientes", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }


        List<TbDetalleVenta> listavd = new List<TbDetalleVenta>();
        TbCliente clienteActual = new TbCliente();

        TbCliente? RecuperarCliente()
        {
            var clienteJson = HttpContext.Session.GetString("cliente");

            if (!string.IsNullOrEmpty(clienteJson))
            {
                try
                {
                    return JsonConvert.DeserializeObject<TbCliente>(clienteJson);
                }
                catch
                {
                    HttpContext.Session.Remove("cliente");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        // GET: VentasController
        public ActionResult Index(string nrodoc, int id, string accion)
        {
            VentasVista viewmodel;
            TbCliente cliente = db.TbClientes.FirstOrDefault(c => c.Nrodocumento == nrodoc)!;


            if (HttpContext.Session.GetString("detaVenta") == null)
            {
               HttpContext.Session.SetString("detaVenta", JsonConvert.SerializeObject(listavd));
            }

            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new VentasVista
                {
                    nuevoCliente = new TbCliente(),
                    listaDetaVenta = listavd
                };
            }
            else
            {
                viewmodel = new VentasVista
                {
                    nuevoCliente = db.TbClientes.Find(id)!,
                    listaDetaVenta = listavd
                };
                ViewBag.abrirModal = accion;
            }

            if(nrodoc != null)
            {
                HttpContext.Session.SetString("cliente", JsonConvert.SerializeObject(cliente));
            }
            

            

            clienteActual = RecuperarCliente()!;
            

            if (clienteActual != null)
            {
                ViewBag.NombreCliente = clienteActual.Nomcli + " " + clienteActual.Apellidos;
                ViewBag.IdCliente = clienteActual.Idcli;
            }
            else
            {
                ViewBag.NombreCliente = "Cliente no encontrado";
            }

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(VentasVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbCliente nuevoCliente = model.nuevoCliente;

                    TempData["mensaje"] = await crearCliente(nuevoCliente);

                    HttpContext.Session.SetString("cliente", JsonConvert.SerializeObject(nuevoCliente));

                    clienteActual = RecuperarCliente()!;


                    if (clienteActual != null)
                    {
                        ViewBag.NombreCliente = clienteActual.Nomcli + " " + clienteActual.Apellidos;
                        ViewBag.IdCliente = clienteActual.Idcli;
                    }
                    else
                    {
                        ViewBag.NombreCliente = "Cliente no encontrado";
                    }

                    return RedirectToAction(nameof(Index));
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


            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listavd
            };

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(VentasVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbCliente nuevoCliente = model.nuevoCliente;

                    TempData["mensaje"] = await EditarCliente(nuevoCliente);

                    HttpContext.Session.SetString("cliente", JsonConvert.SerializeObject(nuevoCliente));

                    clienteActual = RecuperarCliente()!;


                    if (clienteActual != null)
                    {
                        ViewBag.NombreCliente = clienteActual.Nomcli + " " + clienteActual.Apellidos;
                        ViewBag.IdCliente = clienteActual.Idcli;
                    }
                    else
                    {
                        ViewBag.NombreCliente = "Cliente no encontrado";
                    }

                    return RedirectToAction(nameof(Index));
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


            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listavd
            };

            return View("Index", viewmodel);
        }



    }
}
