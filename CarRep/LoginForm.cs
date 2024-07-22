using System;
using System.Collections;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CarRep
{
    public partial class LoginForm : Form
    {
        private string connectionString = "Data Source=UtwoA\\SQLEXPRESS;Initial Catalog=CarRepDB;Integrated Security=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        public static class PasswordHelper
        {
            public static byte[] ComputeHash(string input)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                }
            }

            public static bool VerifyPassword(string input, byte[] storedHash)
            {
                byte[] inputHash = ComputeHash(input);
                return StructuralComparisons.StructuralEqualityComparer.Equals(inputHash, storedHash);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT PasswordHash, Role FROM Users WHERE Username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] storedHash = (byte[])reader["PasswordHash"];
                            string role = reader["Role"].ToString();

                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                if (role == "Admin")
                                {
                                    AdminForm adminForm = new AdminForm();
                                    adminForm.Show();
                                    this.Hide();
                                }
                                else if (role == "User")
                                {
                                    UserForm userForm = new UserForm(username);
                                    userForm.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid password.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("User not found.");
                        }
                    }
                }
            }
        }
    }
}
