namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ReporteVentasVista
    {
        public PA_LISTAR_VENTAS editventa { get; set; } = new PA_LISTAR_VENTAS();
        public IEnumerable<PA_LISTAR_VENTAS> listaVenta { get; set; } = Enumerable.Empty<PA_LISTAR_VENTAS>();
        public IEnumerable<PA_LISTAR_DETALLE_VENTAS> listaDetaVenta { get; set; } = Enumerable.Empty<PA_LISTAR_DETALLE_VENTAS>();

    }
}
