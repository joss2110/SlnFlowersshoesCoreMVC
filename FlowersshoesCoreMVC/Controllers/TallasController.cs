using FlowersshoesCoreMVC.Models;
using FlowersshoesCoreMVC.Models.Vistas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;


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
            TallasVista viewmodel;
            ViewBag.abrirModal = "No";

            if (id == 0)
            {
                viewmodel = new TallasVista
                {
                    listaTallas = lista
                };
            }
            else
            {
                // Lógica para manejar el caso donde id no es igual a cero
                // Por ejemplo, puedes definir un comportamiento predeterminado o lanzar una excepción
                return NotFound(); // Retorna un resultado NotFound como ejemplo
            }

            // Si no se ha devuelto ningún valor hasta este punto, devolver el objeto viewmodel
            return View(viewmodel);

        }
    }
}
