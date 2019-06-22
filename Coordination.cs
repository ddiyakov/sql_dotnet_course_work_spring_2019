using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase
{
    public partial class Coordination : Form
    {
        private string ID = "";
        public Coordination(string EventID)
        {
            ID = EventID;
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            UpdateTable();
        }

        public void UpdateTable()
        {
            SqlConnection connection = UserCoordinator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using (SqlCommand sql = new SqlCommand())
                {

                    SqlCommand sqlCom =
                        new SqlCommand(@"SELECT Appointment.ID, Appointment.WorkerID, EventZone.NameEZ, TimeStart, TimeEnd FROM dbo.Appointment
                                     JOIN EventZone ON EventZone.ID = Appointment.ZoneID
                                     WHERE EventID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new string[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetInt32(1).ToString(),
                        reader.GetString(2),
                        reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm"),
                        reader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm")});

                        listView1.Items.Add(lvi);
                    }
                    reader.Close();
                }
                transaction.Commit();
            } catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                transaction.Rollback();
            }
            connection.Close();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            VolunteersList volunteersList = new VolunteersList();
            volunteersList.ShowDialog();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            SqlConnection connection = UserCoordinator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using (SqlCommand sql = new SqlCommand())
                {

                    SqlCommand sqlCom =
                        new SqlCommand(@"SELECT WorkerID, ZoneID, TimeStart, TimeEnd, EventID FROM Appointment WHERE ID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    if (reader.Read() && reader.GetInt32(4).ToString() == ID)
                    {
                        int rankID = reader.GetInt32(1);
                        if (rankID != 4 && rankID != 5)
                        {
                            reader.Close();
                            sqlCom = new SqlCommand(@"DELETE FROM Appointment WHERE ID = @ID", connection);
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            sqlCom.ExecuteNonQuery();
                            UpdateTable();
                            transaction.Commit();
                        }
                        else
                        {
                            reader.Close();
                            MessageBox.Show("Вы не имеете права изменять организатора и координатора мероприятия.", "Ошибка");
                            transaction.Rollback();
                        }
                    } else
                    {
                        reader.Close();
                        MessageBox.Show("Такого номера нет в таблице.", "Ошибка");
                        transaction.Rollback();
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                transaction.Rollback();
            }
            connection.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CoordinatorEdit coordinatorEdit = new CoordinatorEdit("", ID);
            coordinatorEdit.ShowDialog();
            UpdateTable();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            CoordinatorEdit coordinatorEdit = new CoordinatorEdit(textBox1.Text, ID);

            SqlConnection connection = UserCoordinator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using (SqlCommand sql = new SqlCommand())
                {

                    SqlCommand sqlCom =
                        new SqlCommand(@"SELECT WorkerID, ZoneID, TimeStart, TimeEnd, EventID  FROM Appointment WHERE ID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    if (reader.Read() && reader.GetInt32(4).ToString() == ID)
                    {
                        int rankID = reader.GetInt32(1);
                        if (rankID != 4 && rankID != 5)
                        {
                            coordinatorEdit.textBox1.Text = reader.GetInt32(0).ToString();
                            coordinatorEdit.comboBox1.SelectedIndex = rankID - 1;
                            coordinatorEdit.textBox4.Text = reader.GetDateTime(2).ToString("dd/MM/yyyy HH:mm");
                            coordinatorEdit.textBox5.Text = reader.GetDateTime(3).ToString("dd/MM/yyyy HH:mm");
                            reader.Close();
                            coordinatorEdit.ShowDialog();
                            UpdateTable();
                            transaction.Commit();
                        }
                        else
                        {
                            MessageBox.Show("Вы не имеете права изменять организатора и координатора мероприятия.", "Ошибка");
                            transaction.Rollback();
                        }
                    } else
                    {
                        reader.Close();
                        MessageBox.Show("Такого номера нет в таблице.", "Ошибка");
                        transaction.Rollback();
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                transaction.Rollback();
            }
            connection.Close();
        }
    }
}
