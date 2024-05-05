using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbColores
    {
        public TbColores()
        {
            TbProductos = new HashSet<TbProducto>();
        }

        public int Idcolor { get; set; }
        public string Color { get; set; } = null!;
        public string Estado { get; set; } = string.Empty;

        public virtual ICollection<TbProducto> TbProductos { get; set; }
    }
}
