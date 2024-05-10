namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ReporteIngresosVista
    {
        public PA_LISTAR_INGRESOS editingreso { get; set; } = new PA_LISTAR_INGRESOS();
        public IEnumerable<PA_LISTAR_INGRESOS> listaIngreso { get; set; } = Enumerable.Empty<PA_LISTAR_INGRESOS>();
        public IEnumerable<PA_LISTAR_DETALLE_INGRESOS> listaDetaIngreso { get; set; } = Enumerable.Empty<PA_LISTAR_DETALLE_INGRESOS>();

    }
}
