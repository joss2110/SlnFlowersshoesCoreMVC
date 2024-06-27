using FlowersshoesCoreMVC.DAO;
using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class VentasController : Controller
    {
        private readonly flowersshoesContext db;
        private readonly VentassDAO dao;

        public VentasController(flowersshoesContext ctx, VentassDAO _dao)
        {
            db = ctx;
            dao = _dao;
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
             trabajadorActual = RecuperarTrabajador()!; 

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            VentasVista viewmodel;
            clienteActual = RecuperarCliente()!;
            trabajadorActual = RecuperarTrabajador()!;

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
            trabajadorActual = RecuperarTrabajador()!;


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
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarCarrito(string codbar)
        {


            if (codbar != null)
            {
                TbProducto producto = db.TbProductos.FirstOrDefault(p => p.Codbar == codbar)!;

               

                if (producto != null)
                {

                   


                    listacarrito = RecuperarCarrito();
                    var encontrado = listacarrito.Find(c => c.idpro == producto.Idpro);
                    if (encontrado == null)
                    {
                        if (db.TbStocks.Find(producto.Idpro)!.Cantidad > 0)
                        {
                            PA_LISTAR_DETALLE_VENTAS ldv = new PA_LISTAR_DETALLE_VENTAS()
                            {
                                imagen = producto.Imagen!,
                                idpro = producto.Idpro,
                                nompro = producto.Nompro,
                                color = db.TbColores.Find(producto.Idcolor)!.Color,
                                talla = producto.talla,
                                cantidad = 1,
                                Preciouni = producto.Precio,
                                Subtotal = producto.Precio
                            };
                            listacarrito.Add(ldv);
                        }
                        else
                        {
                            TempData["mensaje"] = "El producto no tiene Stock";
                        }

                        
                    }
                    else
                    {
                        if (db.TbStocks.Find(producto.Idpro)!.Cantidad > encontrado.cantidad)
                        {
                            encontrado.cantidad += 1;
                            encontrado.Subtotal = encontrado.Preciouni * encontrado.cantidad;
                        }
                        else
                        {
                            TempData["mensaje"] = "El producto solo tiene "+encontrado.cantidad+" unidades en Stock";
                        }
                            
                    }
                    GrabarCarrito();

                }
                else
                {
                    TempData["mensaje"] = "producto no encontrado";
                }


            }
            else
            {
                TempData["mensaje"] = "ingrese el codigo de barras";

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

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LimpiarCarrito(string codbar)
        {
            listacarrito.Clear();
            GrabarCarrito();
          


            listacarrito = RecuperarCarrito();


            ViewBag.Total = 0;

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

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            return View("Index", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BuscarCliente(string nrodoc)
        {

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            if (nrodoc != null)
            {
                clienteActual = db.TbClientes.FirstOrDefault(c => c.Nrodocumento == nrodoc)!;
                if(clienteActual != null)
                {
                    GrabarCliente();

                    ViewBag.NombreCliente = clienteActual.Nomcli + " " + clienteActual.Apellidos;
                    ViewBag.IdCliente = clienteActual.Idcli;
                }
                else
                {
                    ViewBag.NombreCliente = "Cliente no encontrado";
                }

            }
            else
            {
                ViewBag.NombreCliente = "Cliente no encontrado";
                TempData["mensaje"] = "Ingrese numero de documento";
            }

            listacarrito = RecuperarCarrito();

            decimal sumaSubtotales = 0;


            foreach (PA_LISTAR_DETALLE_VENTAS item in listacarrito)
            {
                sumaSubtotales += item.Subtotal;
            }

            ViewBag.Total = sumaSubtotales;

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

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

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

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
            }

            return View("Index", viewmodel);
        }



        public ActionResult ReporteVentas ( int id , string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            ReporteVentasVista viewmodel;

            List<PA_LISTAR_VENTAS> listav = dao.listarVentas();

            

            List <PA_LISTAR_DETALLE_VENTAS> listavd = dao.listarDetaVentas(id);
           
            

            if (id == 0)
            {
                viewmodel = new ReporteVentasVista
                {

                    editventa = new PA_LISTAR_VENTAS(),
                    listaVenta = listav,
                    listaDetaVenta = new List<PA_LISTAR_DETALLE_VENTAS>()
                };
            }
            else
            {
                viewmodel = new ReporteVentasVista
                {
                    editventa = listav.Find(v => v.idventa == id)!,
                    listaVenta = listav,
                    listaDetaVenta = listavd
                };
                ViewBag.abrirModal = accion;
            }


            return View("ReporteVentas", viewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarVenta(ReporteVentasVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    

                    TbVenta venta = db.TbVentas.Find(model.editventa.idventa)!;

                    if(venta != null)
                    {
                        venta.EstadoComprobante = model.editventa.estadoComprobante;
                        TempData["mensaje"] = dao.EditarVenta(venta.Idventa, venta.EstadoComprobante);

                    }

                    return RedirectToAction(nameof(ReporteVentas));
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

            ReporteVentasVista viewmodel;

            List<PA_LISTAR_VENTAS> listav = dao.listarVentas();

            viewmodel = new ReporteVentasVista
            {

                editventa = new PA_LISTAR_VENTAS(),
                listaVenta = listav,
                listaDetaVenta = new List<PA_LISTAR_DETALLE_VENTAS>()
            };

            return View("ReporteVentas", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarVenta(ReporteVentasVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {


                    TbVenta venta = db.TbVentas.Find(model.editventa.idventa)!;

                    List<TbDetalleVenta> detallesVenta = db.TbDetalleVentas.Where(detalle => detalle.Idventa == venta.Idventa).ToList();


                    if (venta != null && detallesVenta !=null)
                    {
                        venta.EstadoComprobante = model.editventa.estadoComprobante;
                        TempData["mensaje"] = dao.EliminarVenta(venta.Idventa, detallesVenta);

                    }

                    return RedirectToAction(nameof(ReporteVentas));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo eliminar el registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            ReporteVentasVista viewmodel;

            List<PA_LISTAR_VENTAS> listav = dao.listarVentas();

            viewmodel = new ReporteVentasVista
            {

                editventa = new PA_LISTAR_VENTAS(),
                listaVenta = listav,
                listaDetaVenta = new List<PA_LISTAR_DETALLE_VENTAS>()
            };

            return View("ReporteVentas", viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurarVenta(ReporteVentasVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {


                    TbVenta venta = db.TbVentas.Find(model.editventa.idventa)!;

                    List<TbDetalleVenta> detallesVenta = db.TbDetalleVentas.Where(detalle => detalle.Idventa == venta.Idventa).ToList();


                    if (venta != null && detallesVenta != null)
                    {
                        venta.EstadoComprobante = model.editventa.estadoComprobante;
                        TempData["mensaje"] = dao.RestaurarVenta(venta.Idventa, detallesVenta);

                    }

                    return RedirectToAction(nameof(ReporteVentas));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo restaurar el registro, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            ReporteVentasVista viewmodel;

            List<PA_LISTAR_VENTAS> listav = dao.listarVentas();

            viewmodel = new ReporteVentasVista
            {

                editventa = new PA_LISTAR_VENTAS(),
                listaVenta = listav,
                listaDetaVenta = new List<PA_LISTAR_DETALLE_VENTAS>()
            };

            return View("ReporteVentas", viewmodel);
        }

    }
}
