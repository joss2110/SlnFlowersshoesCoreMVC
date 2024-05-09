using FlowersshoesCoreMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FlowersshoesCoreMVC.DAO
{
    public class VentasDAO
    {
        private string cad_cn = "";

        public VentasDAO(IConfiguration cfg)
        {
            cad_cn = cfg.GetConnectionString("cn1");
        }

        public string GererarVenta( int idtra,int idcli, List<TbDetalleVenta> detaVenta)
        {
            string resultado = "";

            List<KeyValuePair<string, object>> parametros = new List<KeyValuePair<string, object>>();
            parametros.Add(new KeyValuePair<string, object>("@idtra", idtra));
            parametros.Add(new KeyValuePair<string, object>("@idcli", idcli));

          

            try
            {
                int idventa = Convert.ToInt32(SqlHelper.ExecuteNonQuery3(cad_cn, "PA_GRABAR_VENTA", parametros));

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

        public List<PA_LISTAR_VENTAS> listarVentas()
        {
            List<PA_LISTAR_VENTAS> lista = new List<PA_LISTAR_VENTAS>();

            SqlDataReader rd = SqlHelper.ExecuteReader(cad_cn, "PA_LISTAR_VENTAS");

            while (rd.Read())
            {
                var venta = new PA_LISTAR_VENTAS();

                venta.idventa = rd.GetInt32(0);
                venta.trabajador = rd.GetString(1);
                venta.cliente = rd.GetString(2);
                venta.fecha = rd.GetDateTime(3);

                if (!rd.IsDBNull(4))
                    venta.total = rd.GetDecimal(4);

                if (!rd.IsDBNull(5))
                    venta.estadoComprobante = rd.GetString(5);

                if (!rd.IsDBNull(6))
                    venta.estado = rd.GetString(6);

                lista.Add(venta);
            }

            return lista;
        }

        public List<PA_LISTAR_DETALLE_VENTAS> listarDetaVentas(int idventa)
        {
            List<PA_LISTAR_DETALLE_VENTAS> lista = new List<PA_LISTAR_DETALLE_VENTAS>();

            SqlDataReader rd = SqlHelper.ExecuteReader(cad_cn, "PA_LISTAR_DETALLE_VENTAS", idventa);

            while (rd.Read())
            {
                var detaventa = new PA_LISTAR_DETALLE_VENTAS();

                detaventa.idventa = rd.GetInt32(0);
                detaventa.imagen = rd.GetString(1);
                detaventa.nompro = rd.GetString(2);
                detaventa.color = rd.GetString(3);
                detaventa.talla = rd.GetString(4);
                detaventa.cantidad = rd.GetInt32(5);
                detaventa.Preciouni = rd.GetDecimal(6);
                detaventa.Subtotal = rd.GetDecimal(7);

                lista.Add(detaventa);
            }

            return lista;
        }


    }
}
