using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Data.SqlClient;

namespace ProyectoFinal
{
    public partial class menuPrincipal : Form {

        string cadenaConexion = "server=DESKTOP-BNU6A8E\\SQLEXPRESS01;database=envios;integrated security=true;";

        SqlConnection MiConexion = new SqlConnection("server=DESKTOP-BNU6A8E\\SQLEXPRESS01;database=envios;integrated security=true;");

        public menuPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int IParam);

        private void barraMenu_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            popup.Visible = true;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            if(popup.Visible == true)
            {
                popup.Visible = false;
            }
        }

        private void popup_Click(object sender, EventArgs e)
        {

        }

        private bool formSeguimientoAbierto = false;

        private string comprobacion;

        private void dniComprobacion_TextChanged(object sender, EventArgs e)
        {
            // Castear el sender al tipo de control correcto
            TextBox textBox = sender as TextBox;

            // Verificar si el casting fue exitoso y el control no es nulo
            if (textBox != null)
            {
                // Asignar el valor del control al campo comprobacion
                comprobacion = textBox.Text;
            }
        }

        private void btnSeguimiento_Click_1(object sender, EventArgs e)
        {
            // Verificar si comprobacion es null o está vacío
            if (string.IsNullOrEmpty(comprobacion))
            {
                // Mostrar un mensaje de error o realizar alguna acción apropiada
                MessageBox.Show("Error: El valor de comprobacion es nulo o está vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Salir del método para evitar la ejecución de la consulta
            }

            string consultaDniCorrecto = "SELECT dni FROM usuarios WHERE dni = @dni";
            using (SqlConnection MiConexion = new SqlConnection(cadenaConexion))
            {
                MiConexion.Open();
                SqlCommand dniCorrecto = new SqlCommand(consultaDniCorrecto, MiConexion);
                dniCorrecto.Parameters.AddWithValue("@dni", comprobacion);

                // Utilizamos ExecuteScalar para obtener un solo valor de la consulta
                object result = dniCorrecto.ExecuteScalar();

                // Verificar si el resultado no es nulo antes de intentar convertirlo a int
                if (result != null)
                {
                    // Convertir el resultado a int
                    int CDC = Convert.ToInt32(result);

                    if (CDC > 0)
                    {
                        if (formSeguimientoAbierto)
                        {
                            MessageBox.Show("Error: El Formulario de seguimiento se encuentra activo.", "Error 101", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            seguimiento fS = new seguimiento();
                            fS.FormClosed += (s, args) => formSeguimientoAbierto = false; // Actualiza el estado cuando se cierra
                            formSeguimientoAbierto = true;
                            fS.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Tu dni no esta en nuestra base de datos, si crees que es un error contacta con soporte.", "Error 102", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Manejar el caso en el que no se encontró ningún resultado
                    MessageBox.Show("Error: No se encontró ningún resultado para el DNI proporcionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
 }

