namespace FlowersshoesCoreMVC.Models
{
    public class PA_LISTAR_DETALLE_VENTAS
    {
        public int Idventa { get; set; }
        public string imagen { get; set; } = string.Empty;
        public string nompro { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public string talla { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public int MyProperty { get; set; }
        public decimal Preciouni { get; set; }
        public decimal Subtotal { get; set; }
    }
}
