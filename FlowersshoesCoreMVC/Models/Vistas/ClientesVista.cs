namespace FlowersshoesCoreMVC.Models.Vistas
{
    public class ClientesVista
    {
        public TbCliente NuevoClientes { get; set; } = new TbCliente();
        public IEnumerable<TbCliente> listaClientes { get; set; } = Enumerable.Empty<TbCliente>();
    }
}
