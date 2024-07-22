using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CarRep
{
    public partial class AdminForm : Form
    {
        private string connectionString = "Data Source=UtwoA\\SQLEXPRESS;Initial Catalog=CarRepDB;Integrated Security=True";

        public AdminForm()
        {
            InitializeComponent();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT a.AppointmentID, u.Username, s.ServiceName, a.AppointmentDate, a.Status FROM Appointments a JOIN Services s ON a.ServiceID = s.ServiceID JOIN Users u ON a.UserID = u.UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable appointmentsTable = new DataTable();
                    adapter.Fill(appointmentsTable);

                    dgvAdminAppointments.DataSource = appointmentsTable;
                }
            }
        }

        private int GetSelectedAppointmentId()
        {
            if (dgvAdminAppointments.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvAdminAppointments.SelectedRows[0];
                return (int)selectedRow.Cells["AppointmentID"].Value;
            }
            else
            {
                MessageBox.Show("Please select an appointment.");
                return -1;
            }
        }

        private void UpdateAppointmentStatus(string status)
        {
            int appointmentId = GetSelectedAppointmentId();
            if (appointmentId == -1) return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Appointments SET Status = @status WHERE AppointmentID = @appointmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@appointmentId", appointmentId);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Appointment " + status.ToLower() + " successfully.");
                        LoadAppointments();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            UpdateAppointmentStatus("Approved");
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            UpdateAppointmentStatus("Rejected");
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            UpdateAppointmentStatus("Completed");
        }
    }
}
