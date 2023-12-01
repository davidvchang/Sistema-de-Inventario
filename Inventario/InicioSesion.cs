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
    public partial class InicioSesion : Form
    {
        public InicioSesion()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            IniciarSesion();
        }

        void IniciarSesion()
        {
            // mandar datos a la dll
            OpAccionesSQL.sServidor = cbServidor.Text;
            OpAccionesSQL.sUsuario = tbUsuario.Text;
            OpAccionesSQL.sContraseña = tbContraseña.Text;

            OpConexion conexionDB = new OpConexion();

            if (conexionDB.AbrirConexion(cbServidor.Text, tbUsuario.Text, tbContraseña.Text))
            {
                MessageBox.Show("Conectado correctamente");
                Inventarios inventarios = new Inventarios();
                
                this.Hide();
                inventarios.ShowDialog();
            }
            else
            {
                MessageBox.Show($"Ha ocurrido un error: {conexionDB.sLastError}");
            }
        }
    }
}
