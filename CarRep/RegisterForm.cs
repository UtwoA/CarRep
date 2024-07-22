using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static CarRep.LoginForm;

namespace CarRep
{
    public partial class RegisterForm : Form
    {
        private string connectionString = "Data Source=UtwoA\\SQLEXPRESS;Initial Catalog=CarRepDB;Integrated Security=True";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string role = "User"; // Устанавливаем роль по умолчанию

            // Хэшируем пароль
            byte[] passwordHash = PasswordHelper.ComputeHash(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверка на существование пользователя
                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                using (SqlCommand checkUserCommand = new SqlCommand(checkUserQuery, connection))
                {
                    checkUserCommand.Parameters.AddWithValue("@username", username);
                    int userCount = (int)checkUserCommand.ExecuteScalar();

                    if (userCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким именем уже существует.");
                        return;
                    }
                }

                // Вставка нового пользователя
                string insertUserQuery = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@username, @passwordHash, @role)";
                using (SqlCommand insertUserCommand = new SqlCommand(insertUserQuery, connection))
                {
                    insertUserCommand.Parameters.AddWithValue("@username", username);
                    insertUserCommand.Parameters.AddWithValue("@passwordHash", passwordHash);
                    insertUserCommand.Parameters.AddWithValue("@role", role);

                    try
                    {
                        insertUserCommand.ExecuteNonQuery();
                        MessageBox.Show("Регистрация прошла успешно.");
                        this.Close(); // Закрыть форму после успешной регистрации
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
        }
    }
}
