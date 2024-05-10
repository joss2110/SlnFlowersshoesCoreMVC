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
        public int Idtra { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Estado { get; set; } = string.Empty;
      

        public virtual TbTrabajadore? IdtraNavigation { get; set; }
    }
}
