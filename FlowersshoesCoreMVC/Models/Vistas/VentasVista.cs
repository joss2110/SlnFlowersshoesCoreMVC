namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class VentasVista
    {
        public TbCliente nuevoCliente { get; set; } = new TbCliente();
        public IEnumerable<PA_LISTAR_DETALLE_VENTAS> listaDetaVenta { get; set; } = Enumerable.Empty <PA_LISTAR_DETALLE_VENTAS>();

    }
}
