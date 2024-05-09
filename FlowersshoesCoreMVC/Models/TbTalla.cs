             using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbTalla
    {
        public TbTalla()
        {
            TbProductos = new HashSet<TbProducto>();
        }

        public int Idtalla { get; set; }
        public string Talla { get; set; } = null!;

        public virtual ICollection<TbProducto> TbProductos { get; set; }
    }
}
