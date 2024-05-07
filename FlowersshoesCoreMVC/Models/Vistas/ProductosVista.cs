using FlowersshoesCoreMVC.Models;

namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ProductosVista
    {
        public TbProducto NuevoProductos { get; set; } = new TbProducto();
        public IEnumerable<PA_LISTAR_PRODUCTOS> listaProductos { get; set; } = Enumerable.Empty<PA_LISTAR_PRODUCTOS>();
    }
}
