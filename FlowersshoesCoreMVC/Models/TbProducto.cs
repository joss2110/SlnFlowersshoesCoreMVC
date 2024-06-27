using System;
using System.Collections.Generic;

namespace FlowersshoesCoreMVC.Models
{
    public partial class TbProducto
    {
        public TbProducto()
        {
            TbStocks = new HashSet<TbStock>();
        }

        public int Idpro { get; set; }
        public string? Codbar { get; set; }
        public string? Imagen { get; set; }
        public string Nompro { get; set; } = null!;
        public decimal Precio { get; set; }
        public int talla { get; set; }
        public int Idcolor { get; set; }
        public string? Categoria { get; set; }
        public string? Temporada { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = string.Empty;

        public virtual TbColores IdcolorNavigation { get; set; } 
        public virtual ICollection<TbStock> TbStocks { get; set; }
    }
}
