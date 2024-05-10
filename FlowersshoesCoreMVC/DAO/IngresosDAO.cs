using FlowersshoesCoreMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FlowersshoesCoreMVC.DAO
{
    public class IngresosDAO
    {
        private string cad_cn = "";

        public IngresosDAO(IConfiguration cfg)
        {
            cad_cn = cfg.GetConnectionString("cn1");
        }

        public string GererarIngreso(int idtra, string descripcion, List<TbDetalleIngreso> detaIngreso)
        {
            string resultado = "";

            List<KeyValuePair<string, object>> parametros = new List<KeyValuePair<string, object>>();
            parametros.Add(new KeyValuePair<string, object>("@idtra", idtra));
            parametros.Add(new KeyValuePair<string, object>("@descripcion", descripcion));

            try
            {
                int idingre = Convert.ToInt32(SqlHelper.ExecuteNonQuery3(cad_cn, "PA_GRABAR_INGRESOS", parametros));

                foreach (var item in detaIngreso)
                {
                    SqlHelper.ExecuteNonQuery(cad_cn, "PA_GRABAR_DETALLE_INGRESOS", idingre, item.Idpro, item.Cantidad);
                }
                resultado = "El ingreso se realizo con exito!!";

            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            return resultado;
        }
       
        /// ///////////////
        public string EditarIngreso(int idingre, string descripcion)
        {
            string resultado = "";

            try
            {
                SqlHelper.ExecuteNonQuery(cad_cn, "PA_EDITAR_INGRESOS", idingre, descripcion);
     
                resultado = "El ingreso se edito con exito!!";

            }
            catch (Exception ex)
            {
                resultado = ex.Message;
            }

            return resultado;
        }

        public List<PA_LISTAR_INGRESOS> listarIngresos()
        {
            List<PA_LISTAR_INGRESOS> lista = new List<PA_LISTAR_INGRESOS>();

            SqlDataReader rd = SqlHelper.ExecuteReader(cad_cn, "PA_LISTAR_INGRESOS");

            while (rd.Read())
            {
                var ingreso = new PA_LISTAR_INGRESOS();

                ingreso.idingre = rd.GetInt32(0);
                ingreso.fecha = rd.GetDateTime(1);
                ingreso.trabajador = rd.GetString(2);
                if (!rd.IsDBNull(3))
                    ingreso.descripcion = rd.GetString(3);
                ingreso.estado = rd.GetString(4);

                

                lista.Add(ingreso);
            }

            return lista;
        }

        public List<PA_LISTAR_DETALLE_INGRESOS> listarDetaIngres(int idingre)
        {
            List<PA_LISTAR_DETALLE_INGRESOS> lista = new List<PA_LISTAR_DETALLE_INGRESOS>();

            SqlDataReader rd = SqlHelper.ExecuteReader(cad_cn, "PA_LISTAR_DETALLE_INGRESOS", idingre);

            while (rd.Read())
            {
                var detaingre = new PA_LISTAR_DETALLE_INGRESOS();

                detaingre.idingre = rd.GetInt32(0);
                detaingre.imagen = rd.GetString(1);
                detaingre.nompro = rd.GetString(2);
                detaingre.color = rd.GetString(3);
                detaingre.talla = rd.GetString(4);
                detaingre.cantidad = rd.GetInt32(5);

                lista.Add(detaingre);
            }

            return lista;
        }

        public string EliminarIngre(int idingre, List<TbDetalleIngreso> detaIngre)
        {
            string mensaje = "";

            try
            {
                SqlHelper.ExecuteNonQuery(cad_cn, "PA_ELIMINAR_INGRESOS", idingre);

                foreach(var item in detaIngre)
                {
                    SqlHelper.ExecuteNonQuery(cad_cn, "PA_ELIMINAR_DETALLE_INGRESO", item.Idpro, item.Cantidad);
                    mensaje = "El ingreso ha sido Eliminado con Exito";
                }

                mensaje = "El ingreso ha sido Eliminado con Exito";

            }catch (Exception ex)
            {
                mensaje = ex.Message;
            }
             
            return mensaje;

        }

        public string RestaurarIngre(int idingre, List<TbDetalleIngreso> detaIngre)
        {
            string mensaje = "";

            try
            {
                SqlHelper.ExecuteNonQuery(cad_cn, "PA_RESTAURAR_INGRESOS", idingre);

                foreach (var item in detaIngre)
                {
                    SqlHelper.ExecuteNonQuery(cad_cn, "PA_RESTAURAR_DETALLE_INGRESO", item.Idpro, item.Cantidad);
                }

                mensaje = "El ingreso ha sido Restaurado con Exito";

            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }

            return mensaje;

        }




    }
}