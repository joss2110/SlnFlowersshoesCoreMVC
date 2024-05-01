using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbColore
    {
        public TbColore()
        {
            TbProductos = new HashSet<TbProducto>();
        }

        public int Idcolor { get; set; }
        public string Color { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<TbProducto> TbProductos { get; set; }
    }
}
