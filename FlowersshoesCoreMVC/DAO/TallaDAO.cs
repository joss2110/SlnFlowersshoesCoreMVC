using System.Data.SqlClient;
using FlowersshoesCoreMVC.Models;

namespace FlowersshoesCoreMVC.DAO
{
    public class TallaDAO
    {
        private string cad_sql = "";

        public TallaDAO(IConfiguration cfg)
        {
            cad_sql = cfg.GetConnectionString("cn2");
        }

        public List<TbTalla> ListarTallas()
        {
            var lista = new List<TbTalla>();

            SqlDataReader dr =
              SqlHelper.ExecuteReader(cad_sql, "PA_LISTAR_TALLAS");

            while (dr.Read())
            {
                lista.Add(
                  new TbTalla()
                  {
                      Idtalla = dr.GetInt32(0),
                      Talla = dr.GetString(1)
                  });
            }
            dr.Close();

            return lista;
        }
    }
}
