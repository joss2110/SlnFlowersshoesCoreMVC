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

        [HttpGet]
        public async Task<IActionResult> Stocks(int id, string accion)
        {
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

