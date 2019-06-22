using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase.OrganizatorClasses
{
    public partial class WorkersTable : Form
    {
        public WorkersTable()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            UpdateTable();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (textBox1.Text != "5")
                {
                    SqlConnection connection = UserOrganizator.GetDBConnection();
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        using (SqlCommand sql = new SqlCommand())
                        {
                            SqlCommand sqlCom =
                                new SqlCommand(@"SELECT RankID FROM Worker WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            SqlDataReader reader = sqlCom.ExecuteReader();
                            if (reader.Read())
                            {
                                int rankID = reader.GetInt32(0);
                                if (rankID != 3)
                                {
                                    reader.Close();
                                    sqlCom = new SqlCommand(@"DELETE FROM Worker WHERE ID = @ID", connection);
                                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                                    sqlCom.ExecuteNonQuery();
                                    transaction.Commit();
                                    UpdateTable();
                                }
                                else
                                {
                                    reader.Close();
                                    MessageBox.Show("Вы не имеете права удалять других организаторов.", "Ошибка");
                                    transaction.Rollback();
                                }
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
                } else
                {
                    MessageBox.Show("Вы не можете удалить самого себя.", "Ошибка");
                }
            } else
            {
                MessageBox.Show("Вы не ввели никакого номера.", "Ошибка");
            }
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
                        new SqlCommand(@"SELECT Worker.ID, FirstName, SecondName, ThirdName, WorkerRank.NameWR FROM dbo.Worker
                                        JOIN WorkerRank ON WorkerRank.RID = Worker.RankID", connection);
                    sqlCom.Transaction = transaction;
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new string[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
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

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не все обязательные поля заполнены (отчество не обязательно).", "Ошибка");
            } else
            {
                SqlConnection connection = UserOrganizator.GetDBConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand sql = new SqlCommand())
                    {
                        SqlCommand sqlCom =
                            new SqlCommand(@"INSERT INTO dbo.Worker (FirstName, SecondName, ThirdName, RankID)
                                        VALUES (@FN, @SN, @TN, @RID)", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@FN", SqlDbType.VarChar);
                        sqlCom.Parameters.Add("@SN", SqlDbType.VarChar);
                        sqlCom.Parameters.Add("@TN", SqlDbType.VarChar);
                        sqlCom.Parameters.Add("@RID", SqlDbType.Int);
                        sqlCom.Parameters["@FN"].Value = Convert.ToString(textBox2.Text);
                        sqlCom.Parameters["@SN"].Value = Convert.ToString(textBox3.Text);
                        sqlCom.Parameters["@TN"].Value = Convert.ToString(textBox4.Text);
                        sqlCom.Parameters["@RID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
                        sqlCom.ExecuteNonQuery();
                        transaction.Commit();
                        UpdateTable();
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("Что-то пошло не так! Проверьте корректность введенных данных или обратитесь к разработчику.", "Ошибка");
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
                    MessageBox.Show("Не все обязательные поля заполнены (отчество не обязательно).", "Ошибка");
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
                                new SqlCommand(@"SELECT RankID FROM Worker WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                            SqlDataReader reader = sqlCom.ExecuteReader();
                            if (reader.Read())
                            {
                                int rankID = reader.GetInt32(0);
                                if (rankID != 3)
                                {
                                    reader.Close();
                                    sqlCom = new SqlCommand(@"UPDATE dbo.Worker SET FirstName = @FN, SecondName = @SN, ThirdName = @TN, RankID = @RID WHERE ID = @ID", connection);
                                    sqlCom.Transaction = transaction;
                                    sqlCom.Parameters.Add("@FN", SqlDbType.VarChar);
                                    sqlCom.Parameters.Add("@SN", SqlDbType.VarChar);
                                    sqlCom.Parameters.Add("@TN", SqlDbType.VarChar);
                                    sqlCom.Parameters.Add("@RID", SqlDbType.Int);
                                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                                    sqlCom.Parameters["@FN"].Value = Convert.ToString(textBox2.Text);
                                    sqlCom.Parameters["@SN"].Value = Convert.ToString(textBox3.Text);
                                    sqlCom.Parameters["@TN"].Value = Convert.ToString(textBox4.Text);
                                    sqlCom.Parameters["@RID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
                                    sqlCom.ExecuteNonQuery();
                                    transaction.Commit();
                                    UpdateTable();
                                }
                                else
                                {
                                    reader.Close();
                                    MessageBox.Show("Вы не имеете права изменять других организаторов.", "Ошибка");
                                    transaction.Rollback();
                                }
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
