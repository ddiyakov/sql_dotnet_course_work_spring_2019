﻿using System;
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
using VolunteersBase.OrganizatorClasses;

namespace VolunteersBase
{
    public partial class OrganizatorForm : Form
    {
        private string ID = "5";
        public OrganizatorForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            SqlConnection connection = UserOrganizator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using (SqlCommand sql = new SqlCommand())
                {
                    SqlCommand sqlCom = new SqlCommand(@"SELECT * FROM dbo.Worker WHERE ID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    SqlDataReader reader = sqlCom.ExecuteReader();
                    reader.Read();
                    label7.Text = reader.GetString(0);
                    label8.Text = reader.GetString(1);
                    label9.Text = reader.GetString(2);
                    reader.Close();

                    SqlCommand sqlCom2 = new SqlCommand(@"SELECT * FROM dbo.MedicalBook WHERE WorkerID = @ID", connection);
                    sqlCom2.Transaction = transaction;
                    sqlCom2.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom2.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    SqlDataReader reader2 = sqlCom2.ExecuteReader();
                    reader2.Read();
                    if (reader2.GetBoolean(1) == false)
                    {
                        label12.Text = "Болен";
                        button2.Text = "Сообщить о выздоровлении";
                    }
                    else
                    {
                        label12.Text = "Здоров";
                    }
                    reader2.Close();

                    SqlCommand sqlCom1 = new SqlCommand(@"SELECT * FROM dbo.VPB WHERE WorkerID = @ID", connection);
                    sqlCom1.Transaction = transaction;
                    sqlCom1.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom1.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    SqlDataReader reader1 = sqlCom1.ExecuteReader();
                    reader1.Read();
                    label10.Text = reader1.GetInt32(1).ToString();
                    label11.Text = reader1.GetInt32(2).ToString();
                    reader1.Close();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                transaction.Rollback();
            }
            connection.Close();
        }

        private void Button3_Click(object sender, EventArgs e) //выйти
        {
            Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog();
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SqlConnection connection = UserOrganizator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {

                if (label12.Text == "Здоров")
                {
                    DialogResult result = MessageBox.Show(
                        "Вы уверены? \n" +
                        "Ваше действие уберет Вас со всех мероприятий следующей недели!",
                        "Сообщение",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    if (result == DialogResult.Yes)
                    {
                        SqlCommand sqlCom = new SqlCommand(@"UPDATE dbo.MedicalBook SET IsHealthy = 0 WHERE WorkerID = @ID", connection);
                        sqlCom.Transaction = transaction;
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                        sqlCom.Parameters["@ID"].Value = Convert.ToInt32(ID);
                        sqlCom.ExecuteNonQuery();
                        label12.Text = "Болен";
                        button2.Text = "Сообщить о выздоровлении";
                        MessageBox.Show("Выздоравливайте!", "Сообщение");
                    }
                }
                else
                {
                    SqlCommand sqlCom = new SqlCommand(@"UPDATE dbo.MedicalBook SET IsHealthy = 1 WHERE WorkerID = @ID", connection);
                    sqlCom.Transaction = transaction;
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int);
                    sqlCom.Parameters["@ID"].Value = Convert.ToInt32(ID);
                    sqlCom.ExecuteNonQuery();
                    label12.Text = "Здоров";
                    button2.Text = "Сообщить о болезни";
                    MessageBox.Show("Рады видеть Вас снова в строю!", "Сообщение");
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Что-то пошло не так! Обратитесь к разработчику.", "Ошибка");
                transaction.Rollback();
            }
            connection.Close();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            OrganizatorTable organizatorTable = new OrganizatorTable();
            organizatorTable.ShowDialog();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    EventsTable eventsTable = new EventsTable();
                    eventsTable.Show();
                    break;
                case 1:
                    WorkersTable workersTable = new WorkersTable();
                    workersTable.Show();
                    break;
                case 2:
                    DocsTable docsTable = new DocsTable();
                    docsTable.Show();
                    break;
                case 3:
                    AppointmentsTable appointmentsTable = new AppointmentsTable();
                    appointmentsTable.Show();
                    break;
                case 4:
                    CompaniesTable companiesTable = new CompaniesTable();
                    companiesTable.Show();
                    break;
            }
        }
    }
}
