using FlowersshoesCoreMVC.Models;

namespace FlowersshoesCoreMVC.DAO
{
    public class VentasDAO
    {
        private string cad_cn = "";

        public VentasDAO(IConfiguration cfg)
        {
            cad_cn = cfg.GetConnectionString("cn1");
        }

        public string GererarVenta(int idcli, int idtra, List<TbDetalleVenta> detaVenta)
        {
            string resultado = "";

            try
            {
                int idventa = Convert.ToInt32(SqlHelper.ExecuteScalar(cad_cn, "PA_GRABAR_VENTA", idcli, idtra));
                foreach (var item in detaVenta)
                {
                    SqlHelper.ExecuteNonQuery(cad_cn, "PA_GRABAR_DETALLE_VENTA", idventa, item.Idpro, item.Cantidad, item.Preciouni, item.Subtotal);
                }
                resultado = "La venta se realzo con exito!";

            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            return resultado;
        }


    }
}
