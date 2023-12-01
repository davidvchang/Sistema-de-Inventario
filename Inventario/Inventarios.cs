using ReglasDeNegocio;
using System;
using System.Collections;
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
    public partial class Inventarios : Form
    {
        public Inventarios()
        {
            InitializeComponent();
        }


        public TextBox Descripcionn
        {
            get { return tbDescripcionProducto; }
        }

        public TextBox ProductoID
        {
            get { return tbProducto; }
        }

        public TextBox Precio
        {
            get { return tbPrecioVenta; }
        }


        public TextBox Folio
        {
            get { return tbFolio; }
        }


        public Double Total = 0;

        public String ProductoId = String.Empty;
        public String Saldo = String.Empty;
        public String Precioo = String.Empty;
        public String Descripcionnn = String.Empty;
        public Double Totall = 0;
        ArrayList lista = new ArrayList();
        Datos datos = new Datos();


        private void tbFolio_KeyUp(object sender, KeyEventArgs e)
        {
            OpAccionesSQL opAccionesSQL = new OpAccionesSQL();

            if (e.KeyCode == Keys.F1)
            {
                BuscarFolio buscarFolio = new BuscarFolio(this);
                this.Hide();
                buscarFolio.ShowDialog();
            }
            else if(e.KeyCode == Keys.Enter)
            {
                DataTable data = new DataTable();

                // Agregar filas al DataGridView si es necesario
                while (dataGridView1.Rows.Count < data.Rows.Count)
                {
                    dataGridView1.Rows.Add();
                }   

                Int32 Folio = Convert.ToInt32(tbFolio.Text);
                data = opAccionesSQL.LlenarDGVFolio(Folio);
                if (data.Rows.Count > 0)
                {
                    // Asegurarnos de tener suficientes filas en el DataGridView
                    dataGridView1.Rows.Clear(); // Limpiar las filas existentes

                    // Agregar filas al DataGridView si es necesario
                    while (dataGridView1.Rows.Count < data.Rows.Count)
                    {
                        dataGridView1.Rows.Add();
                    }

                    // Llenar las celdas del DataGridView con los valores del DataTable
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        for (int j = 0; j < data.Columns.Count; j++)
                        {
                            // Asegurarnos de tener suficientes celdas en el DataGridView
                            if (j < dataGridView1.Columns.Count)
                            {
                                dataGridView1.Rows[i].Cells[j].Value = data.Rows[i][j].ToString();
                            }
                        }
                    }
                    dataGridView1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No se ha encontrado el folio.");
                    Limpiar();
                    tbProducto.Clear();
                    tbPrecioVenta.Clear();
                    tbDescripcionProducto.Clear();
                    dataGridView1.Enabled = true;
                    dataGridView1.Rows.Clear();
                    UltimoFolio();
                    //tbSaldo.Clear();
                }
            }
        }

        private void registrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Productos productos = new Productos();
            this.Hide();
            productos.ShowDialog();
        }

        private void tbProducto_KeyUp(object sender, KeyEventArgs e)
        {
            OpAccionesSQL opAccionesSQL = new OpAccionesSQL();
            if (e.KeyCode == Keys.F1)
            {
                Buscar buscarr = new Buscar(this);
                this.Hide();
                buscarr.ShowDialog();
            }
            else if(e.KeyCode == Keys.Enter)
            {
                DataTable data = new DataTable();

                Int32 IDProducto = Convert.ToInt32(tbProducto.Text);
                data = opAccionesSQL.BuscarProductos(IDProducto);
                if (data.Rows.Count > 0)
                {
                    tbDescripcionProducto.Text = data.Rows[0][1].ToString();
                    tbPrecioVenta.Text = data.Rows[0][2].ToString();
                    //tb.Text = data.Rows[0][3].ToString();

                }
                else
                {
                    MessageBox.Show("No se ha encontrado ningun producto.");
                    tbProducto.Clear();
                    tbPrecioVenta.Clear();
                    tbDescripcionProducto.Clear();
                    //tbSaldo.Clear();
                }
            }
        }


        // Declarar una lista para almacenar los datos
        List<List<object>> listaDatos = new List<List<object>>();


        private void btnAñadir_Click(object sender, EventArgs e)
        {

            if (tbFolio != null && comboBox1.Text == "Entrada" || comboBox1.Text == "Salida" && tbProducto != null && tbDescripcionProducto != null && tbPrecioVenta != null)
            {
                OpAccionesSQL accionesSQL = new OpAccionesSQL();
                datos.Row(tbProducto.Text, Convert.ToDouble(tbPrecioVenta.Text), Convert.ToDouble(numericUpDownCantidad.Value));
                datos.sFolio = Convert.ToInt32(tbFolio.Text);
                datos.sFecha = dateTimePickerFecha.Value;
                datos.sTipoMovimiento = comboBox1.Text;

                Double Cantidad = 0;
                Double Precio = 0;

                if (comboBox1.Text == "Salida" || comboBox1.Text == "Entrada")
                    Precio = Convert.ToDouble(tbPrecioVenta.Text);

                Cantidad = Convert.ToDouble(numericUpDownCantidad.Value);

                Double Importe = Cantidad * Precio;

                if (comboBox1.Text == "Salida" || comboBox1.Text == "Entrada")
                    dataGridView1.Rows.Add(tbProducto.Text, tbDescripcionProducto.Text, numericUpDownCantidad.Value, tbPrecioVenta.Text, Importe);



                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Total += Convert.ToSingle(dataGridView1.Rows[i].Cells[4].Value);
                }

                tbTotal.Text = Convert.ToString(Total);
                datos.sTotal = Convert.ToSingle(tbTotal.Text);
                Total = 0;

                btnGuardar.Enabled = true;
                btnEliminar.Enabled = true;
                btnLimpiar.Enabled = true;
            }

            else
            {
                MessageBox.Show("Por favor llene todos los campos.");
                btnGuardar.Enabled = false;
                if (tbFolio.Text != "" && tbProducto.Text != "")
                {
                    btnEliminar.Enabled = true;
                }
                else
                {
                    btnEliminar.Enabled = false;
                }
                btnLimpiar.Enabled = false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            OpAccionesSQL accionesSQL = new OpAccionesSQL();


            int Folio = Convert.ToInt32(tbFolio.Text);
            //DateTime Fecha = Convert.ToDateTime(dateTimePickerFecha.Text);
            //int ID = Convert.ToInt32(tbProducto.Text);
            //int Cantidad = Convert.ToInt32(numericUpDownCantidad.Value);
            //double PrecioVenta = Convert.ToDouble(tbPrecioVenta.Text);
            //double Total = Convert.ToDouble(tbTotal.Text);

            if (dataGridView1 == null)
            {
                MessageBox.Show("No se permite guardar.");
            }

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                // Verifica si la fila no está marcada como eliminada
                if (!fila.IsNewRow)
                {
                    if (accionesSQL.ExisteFolio(Folio))
                    {
                        MessageBox.Show("El Folio ingresado ya existe");
                    }
                    else
                    {
                        if (accionesSQL.Alta(datos, Convert.ToInt32(tbFolio.Text), Convert.ToInt32(tbProducto.Text), tbDescripcionProducto.Text, Convert.ToInt32(numericUpDownCantidad.Value),
                                                Convert.ToDouble(tbPrecioVenta.Text), dateTimePickerFecha.Value, Convert.ToDouble(tbTotal.Text), comboBox1.Text))
                        {
                            MessageBox.Show("Guardado correctamente.");
                            datos.renglonesDetalle.Clear();
                            Limpiar();
                            UltimoFolio();
                            btnEliminar.Enabled = false;
                            tbProducto.Enabled = false;
                            dateTimePickerFecha.Enabled = false;
                            numericUpDownCantidad.Enabled = false;
                            comboBox1.Enabled = false;
                            btnGuardar.Enabled = false;
                        }
                        else
                        {
                            //MessageBox.Show("Saldo Insuficiente");
                            Limpiar();
                            UltimoFolio();
                            MessageBox.Show("Ha ocurrido un error. " + accionesSQL.sLastError);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debe de ingresar datos en el datagridview");
                }
            }

        }

        void Limpiar()
        {
            tbFolio.Clear();
            comboBox1.Text = null;
            tbProducto.Clear();
            tbDescripcionProducto.Clear();
            tbPrecioVenta.Clear();
            tbTotal.Clear();
            numericUpDownCantidad.Value = 1;
            dateTimePickerFecha.Value = DateTime.Now;
            dataGridView1.Rows.Clear();
        }

        private void tbProducto_KeyPress(object sender, KeyPressEventArgs e)
        {
            //// Verificar si la tecla presionada es F1
            //if (e.KeyChar == (char)Keys.F1)
            //{
            //    // Permitir la tecla F1
            //    e.Handled = false;
            //}
            //else
            //{
            //    // Bloquear cualquier otra entrada de teclado
            //    e.Handled = true;
            //}
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            datos.renglonesDetalle.Clear();
            Limpiar();
            UltimoFolio();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            OpAccionesSQL accionesSQL = new OpAccionesSQL();

            if (tbFolio.Text == "" || tbFolio == null)
            {
                MessageBox.Show("Folio no ingresado");
            }
            else if (!accionesSQL.ExisteFolio(Convert.ToInt32(tbFolio.Text)))
            {
                MessageBox.Show("El Folio ingresado no existe");
            }


            else
            {
                if (accionesSQL.Baja(Convert.ToInt32(tbFolio.Text)))
                {
                    datos.renglonesDetalle.Clear();
                    MessageBox.Show("Eliminado correctamente.");
                    
                    Limpiar();
                    UltimoFolio();
                }
                else
                {
                    MessageBox.Show("Debe de ingresar el folio y el producto para eliminar.");
                    //MessageBox.Show("Ha ocurrido un error. " + accionesSQL.sLastError);
                }
            }
        }

        private void tbFolio_TextChanged(object sender, EventArgs e)
        {
            tbProducto.Enabled = true;
            btnEliminar.Enabled = true;
            dateTimePickerFecha.Enabled = true;
            numericUpDownCantidad.Enabled = true;
            comboBox1.Enabled = true;
            dataGridView1.Enabled = true;
            dataGridView1.Rows.Clear();
        }

        private void Inventarios_Load(object sender, EventArgs e)
        {
            UltimoFolio();
            dataGridView1.Enabled = true;
            if (tbFolio != null || tbFolio.Text != "")
            {
                tbProducto.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                tbProducto.Enabled = false;
            }

            if(dataGridView1 == null)
            {
                btnGuardar.Enabled = false;
            }
            dateTimePickerFecha.Enabled = false;
            numericUpDownCantidad.Enabled = false;
            comboBox1.Enabled = false;
        }

        void UltimoFolio()
        {
            OpAccionesSQL accionesSQL = new OpAccionesSQL();
            int resultado = accionesSQL.ObtenerUltimoFolio();
            tbFolio.Text = resultado.ToString();
        }

        private void tbProducto_TextChanged(object sender, EventArgs e)
        {
            btnEliminar.Enabled = true;
            dateTimePickerFecha.Enabled = true;
            numericUpDownCantidad.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtiene el índice de la fila que se hizo doble clic
                //int indiceFila = e.RowIndex;




                // Obtiene la fila que se hizo doble clic
                DataGridViewRow filaSeleccionada = dataGridView1.Rows[e.RowIndex];
                int numFila = filaSeleccionada.Index;

                // Elimina la fila del DataGridView
                dataGridView1.Rows.Remove(filaSeleccionada);
                // Recalcula el total después de la eliminación
                datos.renglonesDetalle.RemoveAt(numFila);
                if (dataGridView1 == null)
                {
                    btnGuardar.Enabled = false;
                }
            }










        }

    }
}
