using FlowersshoesCoreMVC.Models.Vistas;
using FlowersshoesCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Text;
using System.Net.Http;
using FlowersshoesCoreMVC.DAO;


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



        string RecuperarDescripcion()
        {
            var decripcionJson = HttpContext.Session.GetString("descripcioningre");

            if (decripcionJson != null)
            {
                return JsonConvert.DeserializeObject<string>(decripcionJson)!;
            }
            else
            {
                return "";
            }
        }

        void GrabarDescripcion()
        {
            HttpContext.Session.SetString("descripcioningre",
                    JsonConvert.SerializeObject(descripcion));
        }


        public ActionResult NuevoIngreso(int id, string accion)
            {
                trabajadorActual = RecuperarTrabajador()!;

                if (trabajadorActual != null)
                {
                    ViewBag.trabajador = trabajadorActual;
                    ViewBag.rolTrabajador = trabajadorActual.Idrol;
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
            public IActionResult GenerarVenta(string descripcioningre)
            {

                listacarrito = RecuperarCarrito();
                trabajadorActual = RecuperarTrabajador()!;
                descripcion = RecuperarDescripcion();
                
                
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

                 descripcion = RecuperarDescripcion();

                try
                {
                    TempData["mensaje"] = dao.GererarIngreso(trabajadorActual.Idtra, descripcion, lista);

                    listacarrito.Clear();
                    GrabarCarrito();
                    descripcion = "";
                    GrabarDescripcion();

                }
                catch (Exception ex)
                {
                    TempData["mensaje"] = ex.Message;
                }


                listacarrito = RecuperarCarrito();
                ViewBag.Descripcion = descripcion;

                var viewmodel = new IngresosVista
                {
                    listaDetaingresos = listacarrito
                };
                trabajadorActual = RecuperarTrabajador()!;

                if (trabajadorActual != null)
                {
                    ViewBag.trabajador = trabajadorActual;
                    ViewBag.rolTrabajador = trabajadorActual.Idrol;
                }

                return View("Index", viewmodel);
            }

            

        }
    }

