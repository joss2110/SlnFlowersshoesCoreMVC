namespace FlowersshoesCoreMVC.Models
{
    public class PA_LISTAR_VENTAS
    {
        public int idventa { get; set; }
        public string trabajador { get; set; } = string.Empty;
        public string cliente { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public decimal total { get; set; }
        public string estadoComprobante { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
