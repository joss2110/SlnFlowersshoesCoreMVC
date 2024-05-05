using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FlowersshoesCoreMVC.Controllers
{
    public class TallasController : Controller
    {
        List<TbTalla> lista = new List<TbTalla>();
        // GET: TallasController
        public async Task<List<TbTalla>> GetTallas()
        {
            using (var httpcliente = new HttpClient())
            {
                var respuesta = await httpcliente.GetAsync("http://localhost:5050/api/Tallas/GetTallas");
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TbTalla>>(respuestaAPI)!;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Tallas(int id, string accion)
        {
            lista = await GetTallas();
            

//

            return View(lista);
        }
    }
}
