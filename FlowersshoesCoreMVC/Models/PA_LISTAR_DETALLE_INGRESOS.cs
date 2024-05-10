namespace FlowersshoesCoreMVC.Models
{
    public class PA_LISTAR_DETALLE_INGRESOS
    {
        public int idingre { get; set; }
        public string imagen { get; set; } = string.Empty;
        public int idpro { get; set; }
        public string nompro { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public string talla { get; set; } = string.Empty;
        public int cantidad { get; set; }
    }
}
