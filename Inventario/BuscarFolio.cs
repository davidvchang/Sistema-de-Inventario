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
    public partial class BuscarFolio : Form
    {
        public BuscarFolio(Inventarios inventarios)
        {
            InitializeComponent();
            this.inventarios = inventarios;
        }

        public Inventarios inventarios;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtiene los valores de la fila seleccionada
                int Folio = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Folio"].Value.ToString());

                    inventarios.Folio.Text = Folio.ToString();

                    Inventarios inventarioss = (Inventarios)Application.OpenForms["inventarioss"];
                    this.Hide();
                    inventarios.Visible = true;

            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable data = new DataTable();
            OpAccionesSQL accionesSQL = new OpAccionesSQL();

            data = accionesSQL.BuscarFolio(tbBuscar.Text);
            if (data.Rows.Count > 0)
            {
                dataGridView1.DataSource = data;
            }
            else
            {
                // Muestra un mensaje indicando que no se encontraron resultados
                MessageBox.Show("No se encontraron el folio requerido.", "Búsqueda", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
