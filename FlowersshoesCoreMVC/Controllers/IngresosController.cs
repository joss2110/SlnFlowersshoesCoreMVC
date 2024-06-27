using FlowersshoesCoreMVC.Models.Vistas;
using FlowersshoesCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Text;
using System.Net.Http;
using FlowersshoesCoreMVC.DAO;
using System.Collections.Generic;
using System;


namespace FlowersshoesCoreMVC.Controllers
{
    public class IngresosController : Controller
    {
        private readonly flowersshoesContext db;
        private readonly IngresosDAO dao;

        public IngresosController(flowersshoesContext ctx, IngresosDAO _dao)
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

        List<PA_LISTAR_DETALLE_INGRESOS> listacarrito = new List<PA_LISTAR_DETALLE_INGRESOS>();
        string descripcion = string.Empty;

        List<PA_LISTAR_DETALLE_INGRESOS> RecuperarCarrito()
        {
            var carritoJson = HttpContext.Session.GetString("CarritoIngre");

            if (carritoJson != null)
            {
                return JsonConvert.DeserializeObject<List<PA_LISTAR_DETALLE_INGRESOS>>(carritoJson)!;
            }
            else
            {
                return new List<PA_LISTAR_DETALLE_INGRESOS>();
            }
        }

        void GrabarCarrito()
        {
            HttpContext.Session.SetString("CarritoIngre",
                    JsonConvert.SerializeObject(listacarrito));
        }


        public ActionResult NuevoIngreso(int id, string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            IngresosVista viewmodel;

            listacarrito = RecuperarCarrito();

            viewmodel = new IngresosVista
            {
                listaDetaingresos = listacarrito
            };

            return View(viewmodel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GenerarIngreso(string descripcioningre)
        {

            listacarrito = RecuperarCarrito();
            trabajadorActual = RecuperarTrabajador()!;


            List<TbDetalleIngreso> lista = new List<TbDetalleIngreso>();


            foreach (PA_LISTAR_DETALLE_INGRESOS item in listacarrito)
            {

                TbDetalleIngreso di = new TbDetalleIngreso()
                {
                    Idpro = item.idpro,
                    Cantidad = item.cantidad
                };
                lista.Add(di);
            }
            if(descripcioningre != null)
            {
                try
                {
                    TempData["mensaje"] = dao.GererarIngreso(trabajadorActual.Idtra, descripcioningre, lista);

                    listacarrito.Clear();
                    GrabarCarrito();
                    ViewBag.Descripcion = "";
                }
                catch (Exception ex)
                {
                    TempData["mensaje"] = ex.Message;
                }
            }
            else
            {
                TempData["mensaje"] = "ingrese una descripcion";
            }
           


            listacarrito = RecuperarCarrito();


            var viewmodel = new IngresosVista
            {
                listaDetaingresos = listacarrito
            };
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            return View("NuevoIngreso", viewmodel);
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

                        PA_LISTAR_DETALLE_INGRESOS ldv = new PA_LISTAR_DETALLE_INGRESOS()
                        {
                            imagen = producto.Imagen!,
                            idpro = producto.Idpro,
                            nompro = producto.Nompro,
                            color = db.TbColores.Find(producto.Idcolor)!.Color,
                            talla = producto.talla!,
                            cantidad = 1,
                        };
                        listacarrito.Add(ldv);



                    }
                    else
                    {

                        encontrado.cantidad += 1;

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


            var viewmodel = new IngresosVista
            {
                listaDetaingresos = listacarrito
            };

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            return View("NuevoIngreso", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LimpiarCarrito(string codbar)
        {
            listacarrito.Clear();
            GrabarCarrito();


            listacarrito = RecuperarCarrito();

            var viewmodel = new IngresosVista
            {
                listaDetaingresos = listacarrito
            };

            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            return View("NuevoIngreso", viewmodel);
        }
        public ActionResult ReporteIngresos(int idingre, string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            List<PA_LISTAR_INGRESOS> listai = dao.listarIngresos();

            List<PA_LISTAR_DETALLE_INGRESOS> listaid = dao.listarDetaIngres(idingre);

            ReporteIngresosVista viewmodel;

            if (idingre == 0)
            {
                viewmodel = new ReporteIngresosVista
                {

                    editingreso = new PA_LISTAR_INGRESOS(),
                    listaIngreso = listai,
                    listaDetaIngreso = new List<PA_LISTAR_DETALLE_INGRESOS>()
                };
            }
            else
            {
                viewmodel = new ReporteIngresosVista
                {
                    editingreso = listai.Find(v => v.idingre == idingre)!,
                    listaIngreso = listai,
                    listaDetaIngreso = listaid
                };
                ViewBag.abrirModal = accion;
            }

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarIngreso(ReporteIngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {


                    TbIngreso ingreso = db.TbIngresos.Find(model.editingreso.idingre)!;

                    if (ingreso != null)
                    {
                        ingreso.Descripcion = model.editingreso.descripcion;
                        TempData["mensaje"] = dao.EditarIngreso(ingreso.Idingre, ingreso.Descripcion);

                    }

                    return RedirectToAction(nameof(ReporteIngresos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo editar el ingreso, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            ReporteIngresosVista viewmodel;

            List<PA_LISTAR_INGRESOS> listai = dao.listarIngresos();

            viewmodel = new ReporteIngresosVista
            {

                editingreso = new PA_LISTAR_INGRESOS(),
                listaIngreso = listai,
                listaDetaIngreso = new List<PA_LISTAR_DETALLE_INGRESOS>()
            };

            return View("ReporteIngresos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarIngreso(ReporteIngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {


                    TbIngreso ingreso = db.TbIngresos.Find(model.editingreso.idingre)!;

                    List<TbDetalleIngreso> detallesIngreso = db.TbDetalleIngresos.Where(detalle => detalle.Idingre == ingreso.Idingre).ToList();


                    if (ingreso != null && detallesIngreso != null)
                    {
                        ingreso.Descripcion = model.editingreso.descripcion;
                        TempData["mensaje"] = dao.EliminarIngre(ingreso.Idingre, detallesIngreso);

                    }

                    return RedirectToAction(nameof(ReporteIngresos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo eliminar el ingreso, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            ReporteIngresosVista viewmodel;

            List<PA_LISTAR_INGRESOS> listai = dao.listarIngresos();

            viewmodel = new ReporteIngresosVista
            {

                editingreso = new PA_LISTAR_INGRESOS(),
                listaIngreso = listai,
                listaDetaIngreso = new List<PA_LISTAR_DETALLE_INGRESOS>()
            };

            return View("ReporteIngresos", viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestaurarIngreso(ReporteIngresosVista model)
        {
            try
            {
                if (ModelState.IsValid == true)
                {
                    TbIngreso ingreso = db.TbIngresos.Find(model.editingreso.idingre)!;

                    List<TbDetalleIngreso> detallesIngreso = db.TbDetalleIngresos.Where(detalle => detalle.Idingre == ingreso.Idingre).ToList();

                    if (ingreso != null && detallesIngreso != null)
                    {
                        ingreso.Descripcion = model.editingreso.descripcion;
                        TempData["mensaje"] = dao.RestaurarIngre(ingreso.Idingre, detallesIngreso);

                    }

                    return RedirectToAction(nameof(ReporteIngresos));
                }
                else
                {
                    TempData["mensaje"] = "No se pudo restaurar el ingreso, intentalo nuevamente";
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error: " + ex.Message;
            }

            ReporteIngresosVista viewmodel;

            List<PA_LISTAR_INGRESOS> listai = dao.listarIngresos();

            viewmodel = new ReporteIngresosVista
            {

                editingreso = new PA_LISTAR_INGRESOS(),
                listaIngreso = listai,
                listaDetaIngreso = new List<PA_LISTAR_DETALLE_INGRESOS>()
            };

            return View("ReporteIngresos", viewmodel);
        }









    }
}