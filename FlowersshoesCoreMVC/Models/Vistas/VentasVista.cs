namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class VentasVista
    {
        public TbCliente nuevoCliente { get; set; } = new TbCliente();
        public IEnumerable<TbDetalleVenta> listaDetaVenta { get; set; } = Enumerable.Empty <TbDetalleVenta>();

    }
}
