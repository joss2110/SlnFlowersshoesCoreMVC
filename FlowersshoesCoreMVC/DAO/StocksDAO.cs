using System.Data.SqlClient;
using System.Drawing;
using FlowersshoesCoreMVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Net.Mime.MediaTypeNames;

namespace FlowersshoesCoreMVC.DAO
{
    public class StocksDAO
    {

        private string cad_sql = "";

        public StocksDAO(IConfiguration cfg)
        {
            cad_sql = cfg.GetConnectionString("cn2");
        }

        public List<TbStock> ListarStocks()
        {
            var lista = new List<TbStock>();

            SqlDataReader dr =
              SqlHelper.ExecuteReader(cad_sql, "PA_LISTAR_STOCKS");

            while (dr.Read())
            {
                lista.Add(
                  new TbStock()
                  {
                      Idstock = dr.GetInt32(0),
                      //Nompro = dr.GetString(1),
                      //Imagen = dr.GetString(2),
                      //Color = dr.GetString(3),
                      //Talla = dr.GetString(4),
                      Cantidad = dr.GetInt32(0),
                  });
            }
            dr.Close();

            return lista;
        }
    }
}
