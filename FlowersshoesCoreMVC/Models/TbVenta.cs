using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbVenta
    {
        public int Idventa { get; set; }
        public int Idtra { get; set; }
        public int Idcli { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = null!;
        public string EstadoComprobante { get; set; } = null!;

        public virtual TbCliente IdcliNavigation { get; set; } = null!;
        public virtual TbTrabajadore IdtraNavigation { get; set; } = null!;
    }
}
