using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbRole
    {
        public TbRole()
        {
            TbTrabajadores = new HashSet<TbTrabajadore>();
        }

        public int Idrol { get; set; }
        public string? NomRol { get; set; }

        public virtual ICollection<TbTrabajadore> TbTrabajadores { get; set; }
    }
}
