using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase
{
    public partial class OrganizatorTable : Form
    {
        private string ID = "5";
        public OrganizatorTable()
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
                        new SqlCommand(@"SELECT Appointment.ID, Event_.NameE, EventZone.NameEZ, TimeStart, TimeEnd FROM dbo.Appointment
                                     JOIN Event_ ON Event_.ID = Appointment.EventID
                                     JOIN EventZone ON EventZone.ID = Appointment.ZoneID
                                     WHERE WorkerID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    listView1.Items.Clear();
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new string[] {
                        reader.GetInt32(0).ToString(),
                        reader.GetString(1),
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

        private void Button2_Click(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void Button1_Click(object sender, EventArgs e)
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
                            new SqlCommand(@"SELECT WorkerID FROM Appointment WHERE ID = @ID", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                        sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                        SqlDataReader reader = sqlCom.ExecuteReader();
                        if (reader.Read() && reader.GetInt32(0).ToString() == ID)
                        {
                            reader.Close();

                            DialogResult result = MessageBox.Show(
                                "Вы уверены?",
                                "Сообщение",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button1,
                                MessageBoxOptions.DefaultDesktopOnly);
                            if (result == DialogResult.Yes)
                            {
                                sqlCom =
                                new SqlCommand(@"DELETE FROM Appointment WHERE ID = @ID", connection);
                                sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                                sqlCom.Parameters["@ID"].Value = Convert.ToInt32(textBox1.Text);
                                sqlCom.ExecuteNonQuery();
                                UpdateTable();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такого номера нет в таблице.", "Ошибка");
                        }
                    }
                    transaction.Commit();
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
    }
}
