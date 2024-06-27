namespace FlowersshoesCoreMVC.Models
{
    public class PA_LISTAR_DETALLE_VENTAS
    {
        public int idventa { get; set; }
        public string imagen { get; set; } = string.Empty;
        public int idpro { get; set; }
        public string nompro { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public int talla { get; set; }
        public int cantidad { get; set; }
        public decimal Preciouni { get; set; }
        public decimal Subtotal { get; set; }
    }
}
