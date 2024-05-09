using PrjFlowersshoesAPI.Models;

namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class TrabajadoresVista
    {
        public TbTrabajadore NuevoTrabajador { get; set; } = new TbTrabajadore();
        public IEnumerable<PA_LISTAR_TRABAJADORES> listaTrabajadores { get; set; } = Enumerable.Empty<PA_LISTAR_TRABAJADORES>();
    }
}
