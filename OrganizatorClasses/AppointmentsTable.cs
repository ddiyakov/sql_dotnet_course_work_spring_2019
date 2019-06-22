using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase.OrganizatorClasses
{
    public partial class AppointmentsTable : Form
    {
        public AppointmentsTable()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            UpdateTable();
        }

        public void UpdateTable()
        {
            SqlConnection connection = UserOrganizator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using (SqlCommand sql = new SqlCommand())
                {

                    SqlCommand sqlCom =
                        new SqlCommand(@"SELECT Appointment.ID, Event_.NameE, Appointment.WorkerID,
                                        EventZone.NameEZ, TimeStart, TimeEnd FROM dbo.Appointment
                                     JOIN Event_ ON Event_.ID = Appointment.EventID
                                     JOIN EventZone ON EventZone.ID = Appointment.ZoneID", connection);
                    sqlCom.Transaction = transaction;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new string[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetInt32(2).ToString(),
                        reader.GetString(3),
                        reader.GetDateTime(4).ToString("dd/MM/yyyy HH:mm"),
                        reader.GetDateTime(5).ToString("dd/MM/yyyy HH:mm")});
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

        private void Button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SqlConnection connection = UserOrganizator.GetDBConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand sql = new SqlCommand())
                    {
                        SqlCommand sqlCom =
                            new SqlCommand(@"SELECT * FROM Appointment WHERE ID = @ID", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                        sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                        SqlDataReader reader = sqlCom.ExecuteReader();
                        if (reader.Read())
                        {
                            reader.Close();
                            sqlCom = new SqlCommand(@"DELETE FROM Appointment WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            sqlCom.ExecuteNonQuery();
                            transaction.Commit();
                            UpdateTable();
                        }
                        else
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
            else
            {
                MessageBox.Show("Вы не ввели никакого номера.", "Ошибка");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Не все обязательные поля заполнены.", "Ошибка");
            }
            else
            {
                SqlConnection connection = UserOrganizator.GetDBConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand sql = new SqlCommand())
                    {
                        SqlCommand sqlCom =
                            new SqlCommand(@"INSERT INTO dbo.Appointment (WorkerID, EventID, ZoneID, TimeStart, TimeEnd)
                                        VALUES (@WID, @EID, @ZID, @TS, @TE)", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@WID", SqlDbType.Int);
                        sqlCom.Parameters.Add("@EID", SqlDbType.Int);
                        sqlCom.Parameters.Add("@ZID", SqlDbType.Int);
                        sqlCom.Parameters.Add("@TS", SqlDbType.DateTime);
                        sqlCom.Parameters.Add("@TE", SqlDbType.DateTime);
                        sqlCom.Parameters["@WID"].Value = Convert.ToInt32(textBox2.Text);
                        sqlCom.Parameters["@EID"].Value = Convert.ToInt32(textBox3.Text);
                        sqlCom.Parameters["@ZID"].Value = Convert.ToInt32(comboBox2.SelectedIndex + 1);
                        sqlCom.Parameters["@TS"].Value = Convert.ToDateTime(textBox4.Text);
                        sqlCom.Parameters["@TE"].Value = Convert.ToDateTime(textBox5.Text);
                        sqlCom.ExecuteNonQuery();
                        transaction.Commit();
                        UpdateTable();
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                    transaction.Rollback();
                }
                connection.Close();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox2.Text == "" || textBox3.Text == "" || comboBox2.SelectedIndex == -1)
                {
                    MessageBox.Show("Не все обязательные поля заполнены.", "Ошибка");
                }
                else
                {
                    SqlConnection connection = UserOrganizator.GetDBConnection();
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    //try
                    //{
                        using (SqlCommand sql = new SqlCommand())
                        {
                            SqlCommand sqlCom =
                                new SqlCommand(@"SELECT * FROM Appointment WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            SqlDataReader reader = sqlCom.ExecuteReader();
                            if (reader.Read())
                            {
                                reader.Close();
                                sqlCom = new SqlCommand(@"UPDATE dbo.Appointment SET WorkerID = @WID, EventID = @EID, ZoneID = @ZID,
                                         TimeStart = @TS, TimeEnd = @TE WHERE ID = @ID", connection);
                                sqlCom.Transaction = transaction;
                                sqlCom.Parameters.Add("@WID", SqlDbType.Int);
                                sqlCom.Parameters.Add("@EID", SqlDbType.Int);
                                sqlCom.Parameters.Add("@ZID", SqlDbType.Int);
                                sqlCom.Parameters.Add("@TS", SqlDbType.DateTime);
                                sqlCom.Parameters.Add("@TE", SqlDbType.DateTime);
                                sqlCom.Parameters["@WID"].Value = Convert.ToInt32(textBox3.Text);
                                sqlCom.Parameters["@EID"].Value = Convert.ToInt32(textBox2.Text);
                                sqlCom.Parameters["@ZID"].Value = Convert.ToInt32(comboBox2.SelectedIndex + 1);
                                sqlCom.Parameters["@TS"].Value = Convert.ToDateTime(textBox4.Text);
                                sqlCom.Parameters["@TE"].Value = Convert.ToDateTime(textBox5.Text);
                                sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                                sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                                sqlCom.ExecuteNonQuery();
                                transaction.Commit();
                                UpdateTable();
                            }
                            else
                            {
                                reader.Close();
                                MessageBox.Show("Такого номера нет в таблице.", "Ошибка");
                                transaction.Rollback();
                            }
                        }
                    //} catch (Exception ex)
                    //{
                    //    MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                    //    transaction.Rollback();
                    //}
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели никакого номера.", "Ошибка");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }
    }
}
