using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReglasDeNegocio
{
    public class OpConexion
    {

        public String sLastError = "";
        static string sError = "";

        public SqlConnection conexion;
        public string cadenaConexion;

        public Boolean AbrirConexion(string servidor, string usuario, string contraseña)
        {
            String CadenaConexion = $"Server = {servidor}; Database = master; User Id = {usuario}; Password = {contraseña};";

            Boolean bAllok = true;
            try
            {
                SqlConnection conexion = new SqlConnection(CadenaConexion);
                conexion.Open();
                conexion.Close();
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllok = false;
            }


            return bAllok;
        }



        public static string CadenaConexion(string Servidor, string Usuario, string Contraseña)
        {
            String CadenaConexion = $"Data Source ={Servidor} ;Initial Catalog = master ; User ID = {Usuario}; Password ={Contraseña}; Connection Timeout = 50";
            SqlConnection connection = new SqlConnection(CadenaConexion);

            try
            {
                connection.Open();
            }

            catch (Exception f)
            {
                sError = f.Message;
            }

            finally
            {
                connection.Close();
            }

            return CadenaConexion;
        }



    }
}
