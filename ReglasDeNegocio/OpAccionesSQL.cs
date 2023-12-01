using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace ReglasDeNegocio
{
    public class OpAccionesSQL
    {
        public String sLastError = "";


        string Servidor = string.Empty;
        string Usuario = string.Empty;
        string Contraseña = string.Empty;


        public static string sServidor;
        public static string sUsuario;
        public static string sContraseña;

        


        public Boolean RegistrarProducto(Int32 ProductoID, String Descripcion, Decimal PrecioVenta, Double Saldo)
        {
            Boolean bAllOk = true;
            using(SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    string query = $"Use Inventario INSERT INTO Productos(ProductoID, Descripcion, PrecioVenta, Saldo) " +
                                                    $"VALUES({ProductoID}, '{Descripcion}', {PrecioVenta}, {Saldo})";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Transaction = transaccion;
                    comando.ExecuteNonQuery();
                    transaccion.Commit();

                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllOk = false;
                    transaccion.Rollback();
                }
                finally
                {
                    conexion.Close();
                }

                return bAllOk;
            }
            
        }

        public Boolean EliminarProducto(Int32 ProductoID)
        {
            Boolean bAllOk = true;
            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));

            try
            {
                conexion.Open();
                string query = $"USE Inventario DELETE Productos WHERE ProductoID = {ProductoID}";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }
            finally
            {
                conexion.Close();
            }
            return bAllOk;
        }

        public DataTable BuscarProductos(Int32 ProductoID)
        {
            DataTable data = new DataTable();

            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));

            try
            {
                conexion.Open();
                string query = $"USE Inventario SELECT ProductoID, Descripcion, PrecioVenta, Saldo FROM Productos WHERE ProductoID = {ProductoID}";
                SqlCommand comando = new SqlCommand(query, conexion);
                data.Load(comando.ExecuteReader());
                

            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            finally
            {
                conexion.Close();
            }
            return data;
        }

        public DataTable LlenarDGVFolio(Int32 sFolio)
        {
            DataTable data = new DataTable();

            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));

            try
            {
                conexion.Open();
                string query = $"USE Inventario SELECT p.ProductoID, p.Descripcion, id.Cantidad, p.PrecioVenta, id.Cantidad * p.PrecioVenta " +
                    $"FROM Productos p " +
                    $"INNER JOIN InventarioDetalle id " +
                    $"ON p.ProductoID = id.ProductoID " +
                    $"WHERE id.Folio = {sFolio}";
                SqlCommand comando = new SqlCommand(query, conexion);
                data.Load(comando.ExecuteReader());


            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            finally
            {
                conexion.Close();
            }
            return data;
        }


        public DataTable BuscarProductosF1(String Descripcion)
        {
            DataTable data = new DataTable();

            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));

            try
            {
                conexion.Open();
                string query = $"USE Inventario SELECT * FROM Productos Where Descripcion Like '%{Descripcion}%'";
                SqlCommand comando = new SqlCommand(query, conexion);
                data.Load(comando.ExecuteReader());


            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            finally
            {
                conexion.Close();
            }
            return data;
        }

        public DataTable BuscarFolio(String Folio)
        {
            DataTable data = new DataTable();

            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));

            try
            {
                conexion.Open();
                string query = $"USE Inventario SELECT Folio FROM Inventarios Where Folio Like '%{Folio}%'";
                SqlCommand comando = new SqlCommand(query, conexion);
                data.Load(comando.ExecuteReader());


            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }
            finally
            {
                conexion.Close();
            }
            return data;
        }
        public Boolean ExisteFolio(Int32 sFolio)
        {
            //Int32 resultado = 0;
            Boolean folioExiste = false;
            SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña));
            try
            {
                conexion.Open();
                string query = $"USE Inventario SELECT COUNT(*) FROM Inventarios WHERE Folio = {sFolio}";
                SqlCommand comando = new SqlCommand(query, conexion);

                // Usar ExecuteScalar para obtener el resultado del conteo
                int resultado = Convert.ToInt32(comando.ExecuteScalar());

                folioExiste = (resultado > 0);

            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                //resultado = -1;
            }
            finally
            {
                conexion.Close();
            }
            return folioExiste;
        }

        public bool ProductoExiste(int productoID)
        {
            bool existe = false;

            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();

                string query = "USE Inventario SELECT COUNT(*) FROM Productos WHERE ProductoID = @ProductoID";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@ProductoID", productoID);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    existe = (count > 0);
                }
            }

            return existe;
        }


        public Boolean Alta(Datos datos, Int32 sFolio, Int32 sProductoID, String sDescripcionN, Int32 sCantidad, Double sPrecioVenta, DateTime sFecha, Double sTotal, String sTipoMovimiento)
        {
            Boolean bAllOk = true;
            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {

                    //string query = $"USE Inventario INSERT INTO Inventarios (Folio, Fecha, Total, TipoMovimiento) " +
                    //    $"VALUES ({datos.sFolio}, '{datos.sFecha.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {datos.sTotal}, '{datos.sTipoMovimiento}')";


                    string query = $"USE Inventario INSERT INTO Inventarios (Folio, Fecha, Total, TipoMovimiento) " +
                        $"VALUES ({sFolio}, '{sFecha.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {sTotal}, '{sTipoMovimiento}')";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Transaction = transaccion;
                    comando.ExecuteNonQuery();



                    for (int i = 0; i < datos.NumRow(); i++)
                    {
                        String sProductoId = String.Empty;
                        String sDescripcion = String.Empty;
                        Double dPrecio = 0;
                        Double dCantidad = 0;

                        DataTable dataTable = new DataTable();

                        datos.GetRow(i, ref sProductoId, ref dPrecio, ref dCantidad);


                        DataTable dt = new DataTable();
                        string queryExiste = $"USE Inventario SELECT COUNT (*) FROM Productos WHERE ProductoID = {sProductoId}";
                        SqlCommand comando2 = new SqlCommand(queryExiste, conexion);
                        comando2.Transaction = transaccion;
                        dt.Load(comando2.ExecuteReader());


                        if (Int32.Parse(dt.Rows[0][0].ToString()) <= 0)
                        {

                            string querySaldo = $"USE Inventario INSERT INTO Productos (ProductoId, Saldo) VALUES ({sProductoId}, {sCantidad})";
                            SqlCommand comandoSaldo = new SqlCommand(querySaldo, conexion);
                            comandoSaldo.Transaction = transaccion;
                            comandoSaldo.ExecuteNonQuery();

                        }

                        else
                        {
                            if (datos.sTipoMovimiento == "Entrada")
                            {
                                string querySaldo = $"USE Inventario UPDATE Productos SET Saldo = Saldo + {dCantidad} WHERE ProductoID = {sProductoId}";
                                SqlCommand comandoSaldo = new SqlCommand(querySaldo, conexion);
                                comandoSaldo.Transaction = transaccion;
                                comandoSaldo.ExecuteNonQuery();

                            }
                            if (datos.sTipoMovimiento == "Salida")
                            {
                                DataTable dataTable2 = new DataTable();

                                string querySaldo = $"USE Inventario SELECT Saldo FROM Productos WHERE ProductoID = {sProductoId}";
                                SqlCommand comandoSaldo = new SqlCommand(querySaldo, conexion);
                                comandoSaldo.Transaction = transaccion;
                                comandoSaldo.ExecuteNonQuery();
                                dataTable2.Load(comandoSaldo.ExecuteReader());

                                if (dataTable2.Rows.Count > 0)
                                {
                                    if (Convert.ToDouble(dataTable2.Rows[0][0].ToString()) < sCantidad)
                                    {
                                        bAllOk = false;
                                        transaccion.Rollback();
                                        return bAllOk;
                                    }
                                    else
                                    {
                                        string querySaldo2 = $"USE Inventario UPDATE Productos SET Saldo = Saldo - {dCantidad} WHERE ProductoID = {sProductoId}";
                                        SqlCommand comandoSaldo2 = new SqlCommand(querySaldo2, conexion);
                                        comandoSaldo2.Transaction = transaccion;
                                        comandoSaldo2.ExecuteNonQuery();
                                    }
                                }
                                else
                                {

                                    string querySaldo3 = $"USE Inventario UPDATE Productos SET Saldo = Saldo - {dCantidad} WHERE ProductoID = {sProductoId}";
                                    SqlCommand comandoSaldo3 = new SqlCommand(querySaldo3, conexion);
                                    comandoSaldo3.Transaction = transaccion;
                                    comandoSaldo3.ExecuteNonQuery();

                                }

                            }

                        }
                    
                        string queryDetalle = $"USE Inventario INSERT INTO InventarioDetalle (Folio, ProductoID, Cantidad) " +
                                                $"VALUES ({datos.sFolio}, {sProductoId}, {dCantidad})";
                        SqlCommand comandoDetalle = new SqlCommand(queryDetalle, conexion);
                        comandoDetalle.Transaction = transaccion;
                        comandoDetalle.ExecuteNonQuery();
                    }
                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllOk = false;
                    transaccion.Rollback();
                }
                finally
                {
                    conexion.Close();
                }
                return bAllOk;
            }
        }

        public Boolean Baja(Int32 nFolio)
        {
            Datos datos = new Datos();
            Boolean bAllOk = true;

            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();

                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    List<int> numerosUnicos = ObtenerNumerosUnicos();

                    // Obtener la cantidad y el tipo de movimiento del producto antes de la eliminación
                    List<float> cantidadEliminada = ObtenerCantidadEliminada(numerosUnicos, nFolio);
                    string tipoMovimiento = ObtenerTipoMovimiento(nFolio);


                    string query = $"USE Inventario DELETE FROM InventarioDetalle WHERE Folio = {nFolio}; ";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Transaction = transaccion;
                    comando.ExecuteNonQuery();

                    string query2 = $"USE Inventario DELETE FROM Inventarios WHERE Folio = {nFolio}; ";
                    SqlCommand comando2 = new SqlCommand(query2, conexion);
                    comando2.Transaction = transaccion;
                    comando2.ExecuteNonQuery();



                    //string query3 = $"USE Inventario DELETE FROM Productos WHERE ProductoID = {sProductoID};";
                    //SqlCommand comando3 = new SqlCommand(query3, conexion);
                    //comando3.Transaction = transaccion;
                    //comando3.ExecuteNonQuery();

                    

                    // Actualizar la cantidad del producto según el tipo de movimiento
                    if (tipoMovimiento == "Entrada")
                    {
                        for (int i = 0; i < cantidadEliminada.Count; i++)
                        {
                            float cantidadEliminadaa = cantidadEliminada[i];
                            int numero = numerosUnicos[i];

                            string queryActualizarEntrada = $"USE Inventario UPDATE Productos SET Saldo = Saldo - {cantidadEliminadaa} WHERE ProductoID = {numero}; ";
                            SqlCommand comandoActualizarEntrada = new SqlCommand(queryActualizarEntrada, conexion);
                            comandoActualizarEntrada.Transaction = transaccion;
                            comandoActualizarEntrada.ExecuteNonQuery();
                        }
                    }
                    else if (tipoMovimiento == "Salida")
                    {
                        for (int i = 0; i < cantidadEliminada.Count; i++)
                        {
                            float cantidadEliminadaa = cantidadEliminada[i];
                            int numero = numerosUnicos[i];

                            string queryActualizarSalida = $"USE Inventario UPDATE Productos SET Saldo = Saldo + {cantidadEliminadaa} WHERE ProductoID = {numero}; ";
                            //string queryActualizarSalida = $"USE Inventario UPDATE Productos SET Saldo = Saldo + {cantidadEliminada}; ";
                            SqlCommand comandoActualizarSalida = new SqlCommand(queryActualizarSalida, conexion);
                            comandoActualizarSalida.Transaction = transaccion;
                            comandoActualizarSalida.ExecuteNonQuery();
                        }
                        
                    }

                    transaccion.Commit();

                }
                catch (Exception error)
                {
                    transaccion.Rollback();
                    bAllOk = false;
                    sLastError = error.Message;

                }
                return bAllOk;

            }

        }

        static List<int> ObtenerNumerosUnicos()
        {
            List<int> numeros = new List<int>();

            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();

                string query = "USE Inventario SELECT DISTINCT ProductoID FROM InventarioDetalle "; // Reemplaza "Informacion" con el nombre de tu tabla
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int numero = reader.GetInt32(0); // Ajusta el índice según el orden de las columnas en tu consulta
                            numeros.Add(numero);
                        }
                    }
                }
            }

            return numeros;
        }



        public Boolean ActualizarProducto(String sDescripcion, Double sPrecioVenta, Int32 sSaldo, Int32 sProductoID)
        {
            Boolean bAllOk = true;
            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    string query = $"USE Inventario UPDATE Productos SET Descripcion = '{sDescripcion}', PrecioVenta = {sPrecioVenta}, Saldo = {sSaldo} WHERE ProductoID = {sProductoID};";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Transaction = transaccion;
                    comando.ExecuteNonQuery();
                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    sLastError = ex.Message;
                    bAllOk = false;
                }
                return bAllOk;
            }
        }


        public Int32 ObtenerUltimoFolio()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
                {
                    conexion.Open();

                    // Consulta SQL para obtener el último folio en inventario
                    string query = "USE Inventario SELECT MAX(folio) FROM Inventarios";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    object resultado = comando.ExecuteScalar();

                    if (resultado != DBNull.Value)
                    {
                        // Si hay un folio, incrementa en 1 y muestra en el TextBox
                        int ultimoFolio = Convert.ToInt32(resultado);
                        return ultimoFolio + 1;
                    }
                    else
                    {
                        // Si no hay folios, establece el valor 1 en el TextBox
                        return 1;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el último folio: " + ex.Message);
                return -1;
            }
        }

        public Int32 ObtenerUltimoID()
        {
            try
            {
                using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
                {
                    conexion.Open();

                    // Consulta SQL para obtener el último id en Productos
                    string query = "USE Inventario SELECT MAX(ProductoID) FROM Productos";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    object resultado = comando.ExecuteScalar();

                    if (resultado != DBNull.Value)
                    {
                        // Si hay un id, incrementa en 1 y muestra en el TextBox
                        int ultimoID = Convert.ToInt32(resultado);
                        return ultimoID + 1;
                    }
                    else
                    {
                        // Si no hay folios, establece el valor 1 en el TextBox
                        return 1;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el último folio: " + ex.Message);
                return -1;
            }
        }

        static List<float> ObtenerCantidadEliminada(List<int> numeros, Int32 sFolio)
        {

            List<float> cantidadesEliminadas = new List<float>();
            //float cantidadTotalEliminada = 0.0f; // Usamos float en lugar de int

            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                conexion.Open();

                foreach (var numero in numeros)
                {
                    string query = $"USE Inventario SELECT SUM(Cantidad) FROM InventarioDetalle WHERE Folio = {sFolio} AND ProductoID = {numero}";
                    using (SqlCommand command = new SqlCommand(query, conexion))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Verifica si el valor no es nulo antes de intentar obtenerlo
                                if (!reader.IsDBNull(0))
                                {
                                    // Intenta obtener el valor como un float
                                    if (float.TryParse(reader.GetValue(0).ToString(), out float cantidad))
                                    {
                                        //cantidadTotalEliminada += cantidad;
                                        cantidadesEliminadas.Add(cantidad);


                                    }
                                    else
                                    {
                                        MessageBox.Show("Ha ocurrido un error. ");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //return Convert.ToInt32(cantidadTotalEliminada);
            return cantidadesEliminadas;
        }

        public String ObtenerTipoMovimiento(Int32 sFolio)
        {
            string TipoMovimiento = "";
            using (SqlConnection conexion = new SqlConnection(OpConexion.CadenaConexion(sServidor, sUsuario, sContraseña)))
            {
                try
                {
                    conexion.Open();
                    string query = $"USE Inventario SELECT TipoMovimiento FROM Inventarios WHERE Folio = {sFolio}";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    object result = comando.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        TipoMovimiento = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener la cantidad eliminada: {ex.Message}");
                }
            }

            return TipoMovimiento;
        }




    }
}
