using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbDetalleIngreso
    {
        public int Idingre { get; set; }
        public int Idpro { get; set; }
        public int Cantidad { get; set; }

        public virtual TbIngreso IdingreNavigation { get; set; } = null!;
        public virtual TbProducto IdproNavigation { get; set; } = null!;
    }
}
