using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbTrabajadore
    {
        public TbTrabajadore()
        {
            TbIngresos = new HashSet<TbIngreso>();
            TbVenta = new HashSet<TbVenta>();
        }

        public int Idtra { get; set; }
        public string Nombres { get; set; } = null!;
        public string TipoDocumento { get; set; } = null!;
        public string NroDocumento { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Pass { get; set; } = null!;
        public int Idrol { get; set; }
        public string Estado { get; set; } = null!;

        public virtual TbRole IdrolNavigation { get; set; } = null!;
        public virtual ICollection<TbIngreso> TbIngresos { get; set; }
        public virtual ICollection<TbVenta> TbVenta { get; set; }
    }
}
