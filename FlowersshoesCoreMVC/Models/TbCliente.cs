using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbCliente
    {
        public TbCliente()
        {
            TbVenta = new HashSet<TbVenta>();
        }

        public int Idcli { get; set; }
        public string Nomcli { get; set; } = null!;
        public string? Apellidos { get; set; }
        public string? Tipodocumento { get; set; }
        public string? Nrodocumento { get; set; } 
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Estado { get; set; } = string.Empty;

        public virtual ICollection<TbVenta> TbVenta { get; set; }
    }
}
