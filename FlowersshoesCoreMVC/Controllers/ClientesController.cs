using FlowersshoesCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace FlowersshoesCoreMVC.Controllers
{
    public class ClientesController : Controller
    {
        List<TbCliente> listaclientes = new List<TbCliente>();

        public async Task<List<TbCliente>> traerClientes()
        {
            // permite realizar una solicitud al servicio web api
            using (var httpcliente = new HttpClient())
            {
                // realizamos una solicitud Get
                var respuesta =
                    await httpcliente.GetAsync(
                        "http://localhost:5050/api/Clientes/GetClientes");
                // convertimos el contenido de la variable respuesta a una cadena
                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                // para despues deserializarlo al formato Json de un List<Medicos>
                return JsonConvert.DeserializeObject<List<TbCliente>>(respuestaAPI)!;
            }
        }

        public async Task<ActionResult> ListarClientes()
        {
            listaclientes = await traerClientes();
            //
            return View(listaclientes);
        }

        // GET: TbClientesController/Details/5
        public async Task<ActionResult> DetailsMedico(int id)
        {
            if (listaclientes.Count == 0)
                listaclientes = await traerClientes();

            TbCliente buscado = listaclientes.Find(m => m.Idcli == id)!;

            return View(buscado);
        }

        public async Task<string> enviarCliente(int opc, TbCliente obj)
        {
            string cadena = string.Empty;

            //
            // permite realizar una solicitud al servicio web api
            using (var httpcliente = new HttpClient())
            {
                obj.Estado = "Activo";
                StringContent contenido = new StringContent(
                JsonConvert.SerializeObject(obj), Encoding.UTF8,
                       "application/json");

                HttpResponseMessage respuesta = new HttpResponseMessage();

                if (opc == 1) // post = grabar
                    respuesta =
                        await httpcliente.PostAsync("http://localhost:5050/api/Clientes/GrabarClientes", contenido);
                else  // put = actualizar
                    respuesta =
                        await httpcliente.PutAsync("http://localhost:5050/api/Clientes/ActualizarClientes", contenido);

                string respuestaAPI = await respuesta.Content.ReadAsStringAsync();
                cadena = respuestaAPI;
            }
            return cadena;
        }
        // GET: TbClientesController/Create
        public async Task<ActionResult> CrearCliente()
        {
            return View();
        }

        // POST: TbClientesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearCliente(TbCliente obj)
        {
            if (ModelState.IsValid)
            {
                TempData["mensaje"] = await enviarCliente(1, obj);
                return RedirectToAction("ListarClientes");
            }
            return View(obj);
        }

        public async Task<ActionResult> ActualizarCliente(int id)
        {
            if (listaclientes.Count == 0)
                listaclientes = await traerClientes();

            TbCliente buscado = listaclientes.Find(m => m.Idcli == id)!;
          
            return View(buscado);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
