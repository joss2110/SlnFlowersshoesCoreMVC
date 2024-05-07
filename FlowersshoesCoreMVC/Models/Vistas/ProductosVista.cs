namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ProductosVista
    {
        public TbProducto NuevoProductos { get; set; } = new TbProducto();
        public IEnumerable<TbProducto> listaProductos { get; set; } = Enumerable.Empty<TbProducto>();
    }
}
