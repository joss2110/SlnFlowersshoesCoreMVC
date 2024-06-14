using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PrjFlowersshoesAPI.Models;

namespace FlowersshoesCoreMVC.Controllers
{
    public class StocksController : Controller
    {
        List<PA_LISTAR_STOCKS> lista = new List<PA_LISTAR_STOCKS>();

        public async Task<List<PA_LISTAR_STOCKS>> GetStocks()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Stocks/GetStocks");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PA_LISTAR_STOCKS>>(respuestaAPI)!;
            }
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

        [HttpGet]
        public async Task<IActionResult> Stocks(int id, string accion)
        {
            trabajadorActual = RecuperarTrabajador()!;

            if (trabajadorActual != null)
            {
                ViewBag.trabajador = trabajadorActual;
                ViewBag.rolTrabajador = trabajadorActual.Idrol;
                ViewBag.NombresTrabajador = trabajadorActual.Nombres;
            }

            lista = await GetStocks();
            StockVista viewmodel;
            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new StockVista
                {
                    listaStocks = lista
                };
            }
            else
            {

                return NotFound();
            }
            return View(viewmodel);

        }
    }

   
}

