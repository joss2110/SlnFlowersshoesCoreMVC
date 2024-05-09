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
        public string? Nombres { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NroDocumento { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string? Pass { get; set; }
        public int Idrol { get; set; }
        public string? Estado { get; set; } = String.Empty;

        public virtual TbRole? IdrolNavigation { get; set; } = null!;
        public virtual ICollection<TbIngreso> TbIngresos { get; set; }
        public virtual ICollection<TbVenta> TbVenta { get; set; }
    }
}
