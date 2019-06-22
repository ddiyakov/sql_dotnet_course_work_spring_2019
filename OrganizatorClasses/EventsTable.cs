using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase.OrganizatorClasses
{
    public partial class EventsTable : Form
    {
        public EventsTable()
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
                        new SqlCommand(@"SELECT Event_.ID, Event_.NameE, Event_.DateE, Company.NameC, EventType.NameET FROM dbo.Event_
                                        JOIN EventType ON Event_.TypeID = EventType.ID
                                        JOIN Company ON Company.ID = Event_.CompanyID", connection);
                    sqlCom.Transaction = transaction;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new string[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetDateTime(2).ToString("dd/MM/yyyy HH:mm"),
                        reader.GetString(3),
                        reader.GetString(4)});
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
                            new SqlCommand(@"SELECT * FROM Event_ WHERE ID = @ID", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                        sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                        SqlDataReader reader = sqlCom.ExecuteReader();
                        if (reader.Read())
                        {
                            reader.Close();
                            sqlCom = new SqlCommand(@"DELETE FROM Event_ WHERE ID = @ID", connection);
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
            if (textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedIndex == -1)
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
                            new SqlCommand(@"INSERT INTO dbo.Event_ (NameE, DateE, TypeID, CompanyID)
                                        VALUES (@NE, @DE, @TID, @CID)", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@NE", SqlDbType.VarChar);
                        sqlCom.Parameters.Add("@DE", SqlDbType.DateTime);
                        sqlCom.Parameters.Add("@TID", SqlDbType.Int);
                        sqlCom.Parameters.Add("@CID", SqlDbType.Int);
                        sqlCom.Parameters["@NE"].Value = Convert.ToString(textBox2.Text);
                        sqlCom.Parameters["@DE"].Value = Convert.ToDateTime(textBox3.Text);
                        sqlCom.Parameters["@CID"].Value = Convert.ToInt32(textBox4.Text);
                        sqlCom.Parameters["@TID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
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
                if (textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedIndex == -1)
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
                                new SqlCommand(@"SELECT * FROM Event_ WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            SqlDataReader reader = sqlCom.ExecuteReader();
                            if (reader.Read())
                            {
                                reader.Close();
                                sqlCom = new SqlCommand(@"UPDATE dbo.Event_ SET NameE = @NE, DateE = @DE, TypeID = @TID, CompanyID = @CID WHERE ID = @ID", connection);
                                sqlCom.Transaction = transaction;
                                sqlCom.Parameters.Add("@NE", SqlDbType.VarChar);
                                sqlCom.Parameters.Add("@DE", SqlDbType.DateTime);
                                sqlCom.Parameters.Add("@TID", SqlDbType.Int);
                                sqlCom.Parameters.Add("@CID", SqlDbType.Int);
                                sqlCom.Parameters["@NE"].Value = Convert.ToString(textBox2.Text);
                                sqlCom.Parameters["@DE"].Value = Convert.ToDateTime(textBox3.Text);
                                sqlCom.Parameters["@CID"].Value = Convert.ToInt32(textBox4.Text);
                                sqlCom.Parameters["@TID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
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
