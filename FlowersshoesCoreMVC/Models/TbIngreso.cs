using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbIngreso
    {
        public int Idingre { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        public int Idtra { get; set; }
        public string Estado { get; set; } = null!;

        public virtual TbTrabajadore IdtraNavigation { get; set; } = null!;
    }
}
