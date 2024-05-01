using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbDetalleVenta
    {
        public int Idventa { get; set; }
        public int Idpro { get; set; }
        public int Cantidad { get; set; }
        public decimal Preciouni { get; set; }
        public decimal Subtotal { get; set; }

        public virtual TbProducto IdproNavigation { get; set; } = null!;
        public virtual TbVenta IdventaNavigation { get; set; } = null!;
    }
}
