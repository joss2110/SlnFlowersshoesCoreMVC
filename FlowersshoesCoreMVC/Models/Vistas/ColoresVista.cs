namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ColoresVista
    {
        public TbColores NuevoColor { get; set; } = new TbColores();
        public IEnumerable<TbColores> listaColores { get; set; } = Enumerable.Empty<TbColores>();
    }
}
