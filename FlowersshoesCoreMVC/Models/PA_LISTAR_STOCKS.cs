namespace PrjFlowersshoesAPI.Models
{
    public partial class PA_LISTAR_STOCKS
    {
        public int idstock { get; set; }
        public string codbar { get; set; }
        public string nompro { get; set; }
        public string imagen { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public decimal precio { get; set; }

        public int cantidad { get; set; }
    }
}