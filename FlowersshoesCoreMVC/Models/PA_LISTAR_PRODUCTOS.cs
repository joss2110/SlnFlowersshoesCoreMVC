namespace FlowersshoesCoreMVC.Models
{
    public partial class PA_LISTAR_PRODUCTOS
    {
        public int idpro { get; set; }
        public string codbar { get; set; } = string.Empty;
        public string imagen { get; set; } = string.Empty;
        public string nompro { get; set; } = string.Empty;
        public decimal precio { get; set; }
        public string talla { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public string categoria { get; set; } = string.Empty;
        public string temporada { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string estado { get; set; } = string.Empty;
    }
}
