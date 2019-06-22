using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase
{
    public partial class VolunteersList : Form
    {
        public VolunteersList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            SqlConnection connection = UserCoordinator.GetDBConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                using (SqlCommand sql = new SqlCommand())
                {

                    SqlCommand sqlCom =
                        new SqlCommand(@"SELECT ID, FirstName, SecondName, ThirdName, WorkerRank.NameWR FROM HEALTHY_BASE()
                                     JOIN WorkerRank ON WorkerRank.RID = RankID", connection);
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
    }
}
