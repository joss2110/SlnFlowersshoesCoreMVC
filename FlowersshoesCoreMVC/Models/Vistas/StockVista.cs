using PrjFlowersshoesAPI.Models;

namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class StockVista
    {
        public IEnumerable<PA_LISTAR_STOCKS> listaStocks { get; set; } = Enumerable.Empty<PA_LISTAR_STOCKS>();
    }
}
