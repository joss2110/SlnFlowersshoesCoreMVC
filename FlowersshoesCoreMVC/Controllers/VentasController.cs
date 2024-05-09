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


        List<PA_LISTAR_DETALLE_VENTAS> listacarrito = new List<PA_LISTAR_DETALLE_VENTAS>();
        TbCliente clienteActual = new TbCliente();
        TbTrabajadore trabajadorActual = new TbTrabajadore();

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

        void GrabarCliente()
        {
            HttpContext.Session.SetString("cliente",
                    JsonConvert.SerializeObject(clienteActual));
        }


        List<PA_LISTAR_DETALLE_VENTAS> RecuperarCarrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");

            if (carritoJson != null)
            {
                return JsonConvert.DeserializeObject<List<PA_LISTAR_DETALLE_VENTAS>>(carritoJson)!;
            }
            else
            {
                // Si la cadena JSON es nula, devolver una lista vacía
                return new List<PA_LISTAR_DETALLE_VENTAS>();
            }
        }

        void GrabarCarrito()
        {
            HttpContext.Session.SetString("Carrito",
                    JsonConvert.SerializeObject(listacarrito));
        }





        // GET: VentasController
        public ActionResult Index( int id, string accion)
        {
            VentasVista viewmodel;

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

            ViewBag.abrirModal = "No";

            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

            if (id == 0)
            {
                
                viewmodel = new VentasVista
                {
                    nuevoCliente = new TbCliente(),
                    listaDetaVenta = listacarrito
                };
            }
            else
            {
                viewmodel = new VentasVista
                {
                    nuevoCliente = db.TbClientes.Find(id)!,
                    listaDetaVenta = listacarrito
                };
                ViewBag.abrirModal = accion;
            }

           

            return View(viewmodel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerarVenta()
        {

            clienteActual = RecuperarCliente()!;
            listacarrito = RecuperarCarrito();
           trabajadorActual = db.TbTrabajadores.Find(1)!;


            if (clienteActual != null && listacarrito.Count > 0)
            {
                List<TbDetalleVenta> lista = new List<TbDetalleVenta>();
                

                foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
                {

                    TbDetalleVenta dv = new TbDetalleVenta()
                    {
                        Idpro = item.idpro,
                        Cantidad = item.cantidad,
                        Preciouni = item.Preciouni,
                        Subtotal = item.Subtotal
                    };
                    lista.Add(dv);
                }

                try
                {
                    TempData["mensaje"] = dao.GererarVenta(trabajadorActual.Idtra, clienteActual.Idcli, lista);

                    listacarrito.Clear();
                    GrabarCarrito();
                    clienteActual = new TbCliente();

                }catch (Exception ex)
                {
                    TempData["mensaje"] = ex.Message;
                }



            }
            else
            {
                TempData["mensaje"] = "NO se pudo realizar la venta, No olvide ingresar el cliente y agregar productos a su carrito";
            }

            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

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

            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listacarrito
            };

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCarrito(string codbar)
        {
            TbProducto producto = db.TbProductos.FirstOrDefault(p => p.Codbar == codbar)!;

            if (codbar != null)
            {
                listacarrito = RecuperarCarrito();
                var encontrado = listacarrito.Find(c => c.idpro == producto.Idpro);
                if (encontrado == null)
                {
                    PA_LISTAR_DETALLE_VENTAS ldv = new PA_LISTAR_DETALLE_VENTAS()
                    {
                        imagen = producto.Imagen!,
                        idpro = producto.Idpro,
                        nompro = producto.Nompro,
                        color = db.TbColores.Find(producto.Idcolor)!.Color,
                        talla = db.TbTallas.Find(producto.Idtalla)!.Talla,
                        cantidad = 1,
                        Preciouni = producto.Precio,
                        Subtotal = producto.Precio
                    };
                    listacarrito.Add(ldv);
                }
                else
                {
                    encontrado.cantidad += 1;
                    encontrado.Subtotal = encontrado.Preciouni * encontrado.cantidad;
                }
                GrabarCarrito();

            }
           

           

            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

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

            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listacarrito
            };

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarCliente(string nrodoc)
        {
            if (nrodoc != null)
            {
                clienteActual = db.TbClientes.FirstOrDefault(c => c.Nrodocumento == nrodoc)!;
                GrabarCliente();
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

            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

            if (clienteActual != null)
            {
                ViewBag.NombreCliente = clienteActual.Nomcli + " " + clienteActual.Apellidos;
                ViewBag.IdCliente = clienteActual.Idcli;
            }
            else
            {
                ViewBag.NombreCliente = "Cliente no encontrado";
            }

            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listacarrito
            };

            return View("Index", viewmodel);
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



            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

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

            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listacarrito
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


            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

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

            var viewmodel = new VentasVista
            {
                nuevoCliente = new TbCliente(),
                listaDetaVenta = listacarrito
            };

            return View("Index", viewmodel);
        }

        public ActionResult ReporteVentas ( int idventa)
        {

            List<PA_LISTAR_VENTAS> listav = dao.listarVentas();

            List<PA_LISTAR_DETALLE_VENTAS> listavd = dao.listarDetaVentas(idventa);

            var viewmodel = new ReporteVentasVista
            {
                editventa = new TbVenta(),
                listaVenta = listav,
                listaDetaVenta = listavd

            };
            return View(viewmodel);
        }

    }
}
