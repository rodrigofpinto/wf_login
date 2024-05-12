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
using System.Data.SqlClient;

namespace wf_login
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\pinto\\Documents\\wf_login\\wf_login\\Database1.mdf;Integrated Security=True";
            string query = "SELECT COUNT(*) FROM dados WHERE username=@username AND password=@password";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", txtUser.Text);
                    cmd.Parameters.AddWithValue("@password", txtPass.Text);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        Form2 f2 = new Form2();
                        f2.Show();
                        this.Hide();
                        MessageBox.Show("Login com sucesso", "Info");
                    }
                    else
                    {
                        MessageBox.Show("Error login", "Erro");
                    }
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Length < 4)
            {
                MessageBox.Show("O nome de usuário deve ter pelo menos 4 caracteres.", "Erro de Registro");
                return;
            }

            if (txtPass.Text.Length < 8)
            {
                MessageBox.Show("A senha deve ter pelo menos 8 caracteres.", "Erro de Registro");
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\pinto\\Documents\\wf_login\\wf_login\\Database1.mdf;Integrated Security=True";
            string checkQuery = "SELECT COUNT(*) FROM dados WHERE username=@username";
            string insertQuery = "INSERT INTO dados (username, password) VALUES (@username, @password)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Verifica se o usuário já existe
                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, con))
                {
                    cmdCheck.Parameters.AddWithValue("@username", txtUser.Text);
                    int userExists = (int)cmdCheck.ExecuteScalar();

                    if (userExists > 0)
                    {
                        MessageBox.Show("Este nome de usuário já está sendo usado.", "Erro de Registro");
                        return;
                    }
                }

                // Insere o novo usuário
                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, con))
                {
                    cmdInsert.Parameters.AddWithValue("@username", txtUser.Text);
                    cmdInsert.Parameters.AddWithValue("@password", txtPass.Text);
                    int result = cmdInsert.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Registro realizado com sucesso!", "Registro Concluído");
                    }
                    else
                    {
                        MessageBox.Show("Erro ao registrar usuário.", "Erro de Registro");
                    }
                }
            }
        }

        private void checkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar=checkShowPass.Checked?'\0' : '*';
        }
    }
}
