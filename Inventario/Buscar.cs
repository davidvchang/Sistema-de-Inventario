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
    public partial class Buscar : Form
    {
        public Buscar(Productos productos)
        {
            InitializeComponent();
            this.productos = productos;
        }

        public Buscar(Inventarios inventarios)
        {
            InitializeComponent();
            this.inventarios = inventarios;
        }



        public Productos productos;

        public Inventarios inventarios;

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            OpAccionesSQL accionesSQL = new OpAccionesSQL();

            data = accionesSQL.BuscarProductosF1(tbBuscar.Text);
            if (data.Rows.Count > 0)
            {
                dataGridView1.DataSource = data;
            }
            else
            {
                // Muestra un mensaje indicando que no se encontraron resultados
                MessageBox.Show("No se encontraron productos con la descripción proporcionada.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                // Obtiene los valores de la fila seleccionada
                int ProductoID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ProductoID"].Value.ToString());
                string descripcion = dataGridView1.Rows[e.RowIndex].Cells["Descripcion"].Value.ToString();
                decimal precio = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["PrecioVenta"].Value);
                double saldo = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Saldo"].Value);


                if(productos is Productos)
                {
                    // Llena los TextBox en Form1 con la información del producto seleccionado
                    productos.ProductoID.Text = ProductoID.ToString();
                    productos.Descripcion.Text = descripcion;
                    productos.Precio.Text = precio.ToString();
                    productos.Saldo.Text = saldo.ToString();


                    Productos productoss = (Productos)Application.OpenForms["productoss"];
                    this.Hide();
                    productos.Visible = true;
                }
                else if(inventarios is Inventarios)
                {
                    inventarios.ProductoID.Text = ProductoID.ToString();
                    inventarios.Descripcionn.Text = descripcion;
                    inventarios.Precio.Text = precio.ToString();

                    Inventarios inventarioss = (Inventarios)Application.OpenForms["inventarioss"];
                    this.Hide();
                    inventarios.Visible = true;
                }

            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Inventarios inventarios = (Inventarios)Application.OpenForms["inventarios"];
            this.Hide();
            inventarios.Visible = true;
        }
    }
}
