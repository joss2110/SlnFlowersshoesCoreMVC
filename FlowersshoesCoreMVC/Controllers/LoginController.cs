using System;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using FlowersshoesCoreMVC.Models;
using Newtonsoft.Json;

namespace FlowersshoesCoreMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly flowersshoesContext db;

        public LoginController(flowersshoesContext ctx)
        {
            db = ctx;
        }

        public IActionResult Login()
        {
            trabajadorActual = null;
            GrabarTrabajador();

            return View();
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

        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            if (email != null && pass != null)
            {
                TbTrabajadore trabajadorExistente = new TbTrabajadore();
                trabajadorExistente = db.TbTrabajadores.FirstOrDefault(t => t.Email == email)!;
                
                if (trabajadorExistente != null && trabajadorExistente.Pass == pass)
                {
                    trabajadorActual = trabajadorExistente;
                    GrabarTrabajador();
                    return RedirectToAction("Index", "Ventas");
                } else
                {
                    ViewBag.mensaje = "El correo o contraseña no es correcto";
                    return View("Login");
                }
            } else
            {
                ViewBag.mensaje = "Completa los campos";
                return View("Login");
            }
        }

    }
}
