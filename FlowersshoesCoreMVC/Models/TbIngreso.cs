using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbIngreso
    {
        public TbIngreso()
        {
            IdtraNavigation = new TbTrabajadore();
        }
        public int Idingre { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Descripcion { get; set; } = null!;
        public int Idtra { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Idpro { get; set; }
        public int Cantidad { get; set; }

        public virtual TbTrabajadore? IdtraNavigation { get; set; }
    }
}
