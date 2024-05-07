namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class IngresosVista
    {
        public TbIngreso NuevoIngreso { get; set; } = new TbIngreso();
        public IEnumerable<TbIngreso> listaIngresos { get; set; } = Enumerable.Empty<TbIngreso>();
        public IEnumerable<TbTrabajadore> listaTrabajadores { get; set; } = Enumerable.Empty<TbTrabajadore>();
        public IEnumerable<TbProducto> listaProductos { get; set; } = Enumerable.Empty<TbProducto>();
    }
}
