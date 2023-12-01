using ReglasDeNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventario
{
    public partial class Productos : Form
    {
        public Productos()
        {
            InitializeComponent();
        }


        public TextBox Descripcion
        {
            get { return tbDescripcion; }
        }

        public TextBox ProductoID
        {
            get { return tbProductoID; }
        }

        public TextBox Precio
        {
            get { return tbPrecioVenta; }
        }

        public TextBox Saldo
        {
            get { return tbSaldo; }
        }





        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Inventarios inventarios = (Inventarios)Application.OpenForms["inventarios"];
            this.Hide();
            inventarios.Visible = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            OpAccionesSQL opAccionesSQL = new OpAccionesSQL();

            if (opAccionesSQL.ProductoExiste(Convert.ToInt32(tbProductoID.Text)))
            {
                string Descripcion = tbDescripcion.Text;
                double PrecioVenta = Convert.ToDouble(tbPrecioVenta.Text);
                int Saldo = Convert.ToInt32(tbSaldo.Text);
                int ProductiID = Convert.ToInt32(tbProductoID.Text);
                if (opAccionesSQL.ActualizarProducto(Descripcion, PrecioVenta, Saldo, ProductiID))
                {
                    MessageBox.Show("Actualizado correctamente");
                    tbProductoID.Clear();
                    tbPrecioVenta.Clear();
                    tbDescripcion.Clear();
                    tbSaldo.Clear();
                    UltimoID();
                }
                else
                {
                    MessageBox.Show("Ha ocurrido un error: " + opAccionesSQL.sLastError);
                }
            }
            else
            {
                if (tbProductoID != null && tbPrecioVenta != null && tbDescripcion != null && tbSaldo != null)
                {


                    int ID = Convert.ToInt32(tbProductoID.Text);
                    decimal PrecioVenta = Convert.ToDecimal(tbPrecioVenta.Text);
                    double Saldo = Convert.ToDouble(tbSaldo.Text);

                    if (opAccionesSQL.RegistrarProducto(ID, tbDescripcion.Text, PrecioVenta, Saldo))
                    {
                        MessageBox.Show("Producto registrado correctamente.");

                        tbProductoID.Clear();
                        tbPrecioVenta.Clear();
                        tbDescripcion.Clear();
                        tbSaldo.Clear();
                    }

                    else
                    {
                        MessageBox.Show("Ese ProductoID ya existe.");
                        tbProductoID.Clear();
                        tbDescripcion.Clear();
                        tbPrecioVenta.Clear();
                        tbSaldo.Clear();
                        //MessageBox.Show("Ha ocurrido un error. " + opAccionesSQL.sLastError);
                    }
                }
            }

            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            OpAccionesSQL opAccionesSQL = new OpAccionesSQL();

            if (opAccionesSQL.EliminarProducto(Convert.ToInt32(tbProductoID.Text)))
            {
                MessageBox.Show("Producto eliminado correctamente.");

                tbProductoID.Clear();
                tbPrecioVenta.Clear();
                tbDescripcion.Clear();
                tbSaldo.Clear();
                UltimoID();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error. " + opAccionesSQL.sLastError);
            }
        }

        private void tbProductoID_KeyUp(object sender, KeyEventArgs e)
        {
            OpAccionesSQL opAccionesSQL = new OpAccionesSQL();
            if(e.KeyCode == Keys.Enter)
            {
                DataTable data = new DataTable();

                Int32 IDProducto = Convert.ToInt32(tbProductoID.Text);
                data = opAccionesSQL.BuscarProductos(IDProducto);
                if(data.Rows.Count > 0)
                {
                    tbDescripcion.Text = data.Rows[0][1].ToString();
                    tbPrecioVenta.Text = data.Rows[0][2].ToString();
                    tbSaldo.Text = data.Rows[0][3].ToString();

                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningun producto.");
                    tbProductoID.Clear();
                    tbPrecioVenta.Clear();
                    tbDescripcion.Clear();
                    tbSaldo.Clear();
                }
            }
            else if(e.KeyCode == Keys.F1)
            {
                Buscar buscar = new Buscar(this);
                this.Hide();
                this.Visible = false;
                buscar.ShowDialog();
            }
        }



        void UltimoID()
        {
            OpAccionesSQL accionesSQL = new OpAccionesSQL();
            int resultado = accionesSQL.ObtenerUltimoID();
            tbProductoID.Text = resultado.ToString();
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            UltimoID();
        }
    }
}
