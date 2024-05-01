using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbStock
    {
        public int Idstock { get; set; }
        public int Idpro { get; set; }
        public int Cantidad { get; set; }

        public virtual TbProducto IdproNavigation { get; set; } = null!;
    }
}
