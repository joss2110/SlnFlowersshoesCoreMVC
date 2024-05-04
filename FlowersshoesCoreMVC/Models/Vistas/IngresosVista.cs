namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class IngresosVista
    {
        public TbIngreso NuevoIngreso { get; set; } = new TbIngreso();
        public IEnumerable<TbIngreso> listaIngresos { get; set; } = Enumerable.Empty<TbIngreso>();
    }
}
