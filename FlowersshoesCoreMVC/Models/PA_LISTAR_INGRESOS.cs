namespace FlowersshoesCoreMVC.Models
{
    public class PA_LISTAR_INGRESOS
    {
        public int idingre { get; set; }
        
        public DateTime fecha { get; set; }
        public string trabajador { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
