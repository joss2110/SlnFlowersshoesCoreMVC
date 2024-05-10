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
                                talla = db.TbTallas.Find(producto.Idtalla)!.Talla,
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
            }

            return View("NuevoIngreso", viewmodel);
        }

        public ActionResult ReporteIngresos()
        {

            return View();
        }

}