using PrjFlowersshoesAPI.Models;

namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ProductosVista
    {
        public PA_LISTAR_PRODUCTOS NuevoProductos { get; set; } = new PA_LISTAR_PRODUCTOS();
        public IEnumerable<PA_LISTAR_PRODUCTOS> listaProductos { get; set; } = Enumerable.Empty<PA_LISTAR_PRODUCTOS>();
    }
}
