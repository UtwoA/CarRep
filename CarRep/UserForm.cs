using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace CarRep
{
    public partial class UserForm : Form
    {
        private string connectionString = "Data Source=UtwoA\\SQLEXPRESS;Initial Catalog=CarRepDB;Integrated Security=True";
        private string username;

        public UserForm(string username)
        {
            InitializeComponent();
            this.username = username;
            LoadServices();
            LoadAppointments();
        }

        private void LoadServices()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Services";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable servicesTable = new DataTable();
                    adapter.Fill(servicesTable);

                    cmbServices.DisplayMember = "ServiceName";
                    cmbServices.ValueMember = "ServiceID";
                    cmbServices.DataSource = servicesTable;
                }
            }
            this.dgvAppointments.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAppointments_CellFormatting);

        }

        private void LoadAppointments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT a.AppointmentID, s.ServiceName, a.AppointmentDate, a.Status " +
                               "FROM Appointments a " +
                               "JOIN Services s ON a.ServiceID = s.ServiceID " +
                               "JOIN Users u ON a.UserID = u.UserID " +
                               "WHERE u.Username = @username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvAppointments.DataSource = dataTable;

                        // Добавляем колонку для кнопок, если она еще не добавлена
                        if (!dgvAppointments.Columns.Contains("DownloadReceipt"))
                        {
                            DataGridViewButtonColumn downloadColumn = new DataGridViewButtonColumn
                            {
                                Name = "DownloadReceipt",
                                HeaderText = "Чек",
                                Text = "Скачать чек",
                                UseColumnTextForButtonValue = true
                            };
                            dgvAppointments.Columns.Add(downloadColumn);
                        }
                    }
                }
            }
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            int serviceId = (int)cmbServices.SelectedValue;
            DateTime appointmentDate = dtpAppointmentDate.Value;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string getUserIdQuery = "SELECT UserID FROM Users WHERE Username = @username";
                int userId;

                using (SqlCommand getUserIdCommand = new SqlCommand(getUserIdQuery, connection))
                {
                    getUserIdCommand.Parameters.AddWithValue("@username", username);
                    userId = (int)getUserIdCommand.ExecuteScalar();
                }

                string insertQuery = "INSERT INTO Appointments (UserID, ServiceID, AppointmentDate, Status) VALUES (@userId, @serviceId, @appointmentDate, 'Pending')";
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@serviceId", serviceId);
                    command.Parameters.AddWithValue("@appointmentDate", appointmentDate);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Appointment booked successfully.");
                        LoadAppointments();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void GenerateReceipt(int appointmentId, string filePath)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT s.ServiceName, s.ServiceDescription, s.Price, a.AppointmentDate, u.Username " +
                               "FROM Appointments a " +
                               "JOIN Services s ON a.ServiceID = s.ServiceID " +
                               "JOIN Users u ON a.UserID = u.UserID " +
                               "WHERE a.AppointmentID = @appointmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@appointmentID", appointmentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string serviceName = reader["ServiceName"].ToString();
                            string serviceDescription = reader["ServiceDescription"].ToString();
                            decimal price = (decimal)reader["Price"];
                            DateTime appointmentDate = (DateTime)reader["AppointmentDate"];
                            string username = reader["Username"].ToString();

                            Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                            document.Open();

                            // Заголовок
                            string fontPath = @"C:\Users\works\source\repos\CarRep\CarRep\bin\Debug\ArialUnicodeMS.ttf"; // Укажите путь к файлу шрифта
                            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            Font headerFont = new Font(baseFont, 18);
                            Paragraph header = new Paragraph("Чек на услугу", headerFont);
                            header.Alignment = Element.ALIGN_CENTER;
                            document.Add(header);

                            document.Add(new Paragraph("\n"));

                            // Информация о пользователе
                            Font infoFont = new Font(baseFont, 12);
                            Paragraph userInfo = new Paragraph($"Имя пользователя: {username}", infoFont);
                            document.Add(userInfo);

                            // Таблица с данными услуги
                            PdfPTable table = new PdfPTable(2);
                            table.WidthPercentage = 100;
                            table.SetWidths(new float[] { 1f, 2f });

                            // Заголовки таблицы
                            Font tableHeaderFont = new Font(baseFont, 12);
                            PdfPCell cell = new PdfPCell(new Phrase("Параметр", tableHeaderFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY
                            };
                            table.AddCell(cell);
                            cell = new PdfPCell(new Phrase("Значение", tableHeaderFont))
                            {
                                BackgroundColor = BaseColor.LIGHT_GRAY
                            };
                            table.AddCell(cell);

                            // Данные услуги
                            Font tableCellFont = new Font(baseFont, 12);
                            table.AddCell(new Phrase("Услуга:", tableCellFont));
                            table.AddCell(new Phrase(serviceName, tableCellFont));
                            table.AddCell(new Phrase("Описание услуги:", tableCellFont));
                            table.AddCell(new Phrase(serviceDescription, tableCellFont));
                            table.AddCell(new Phrase("Цена:", tableCellFont));
                            table.AddCell(new Phrase($"{price:C}", tableCellFont));
                            table.AddCell(new Phrase("Дата записи:", tableCellFont));
                            table.AddCell(new Phrase(appointmentDate.ToString("dd MMMM yyyy"), tableCellFont));

                            document.Add(table);

                            document.Add(new Paragraph("\n"));

                            // Подпись  
                            Font footerFont = new Font(baseFont, 12);
                            Paragraph footer = new Paragraph("Спасибо за использование наших услуг!", footerFont);
                            footer.Alignment = Element.ALIGN_CENTER;
                            document.Add(footer);

                            document.Close();
                        }
                    }
                }
            }
        }




        private void dgvAppointments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Убедитесь, что это правильный столбец
            if (dgvAppointments.Columns[e.ColumnIndex].Name == "DownloadReceipt" && e.RowIndex >= 0)
            {
                var statusCell = dgvAppointments.Rows[e.RowIndex].Cells["Status"];
                if (statusCell.Value != null && statusCell.Value.ToString() == "Completed")
                {
                    e.Value = "Скачать чек";
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = string.Empty;
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvAppointments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAppointments.Columns[e.ColumnIndex].Name == "DownloadReceipt" && e.RowIndex >= 0)
            {
                var statusCell = dgvAppointments.Rows[e.RowIndex].Cells["Status"];
                if (statusCell.Value != null && statusCell.Value.ToString() == "Completed")
                {
                    int appointmentId = (int)dgvAppointments.Rows[e.RowIndex].Cells["AppointmentID"].Value;
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "PDF файлы (*.pdf)|*.pdf",
                        FileName = $"Receipt_{appointmentId}.pdf"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        GenerateReceipt(appointmentId, saveFileDialog.FileName);
                        MessageBox.Show($"Чек для записи {appointmentId} был создан и сохранен.");
                    }
                }
            }
        }
    }
}
