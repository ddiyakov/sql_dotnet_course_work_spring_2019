using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VolunteersBase.UsersClasses;

namespace VolunteersBase
{
    public partial class CoordinatorEdit : Form
    {
        private string AID = "";
        private string EID = "";
        public CoordinatorEdit(string AID, string EID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AID = AID;
            this.EID = EID;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox4.Text == "" || textBox5.Text == "" || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Не все поля заполнены.", "Ошибка");
            } else { 
                SqlConnection connection = UserCoordinator.GetDBConnection();
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(); 
                if (AID != "")
                {
                    try
                    {
                        using (SqlCommand sql = new SqlCommand())
                        {
                            SqlCommand sqlCom = new SqlCommand(@"UPDATE dbo.Appointment SET 
                                                         WorkerID = @WID, ZoneID = @ZID,
                                                         TimeStart = @TS, TimeEnd = @TE
                                                         WHERE ID = @ID", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@WID", SqlDbType.Int);
                            sqlCom.Parameters.Add("@ZID", SqlDbType.Int);
                            sqlCom.Parameters.Add("@TS", SqlDbType.DateTime);
                            sqlCom.Parameters.Add("@TE", SqlDbType.DateTime);
                            sqlCom.Parameters.Add("@ID", SqlDbType.Int);

                            sqlCom.Parameters["@WID"].Value = Convert.ToInt32(textBox1.Text);
                            sqlCom.Parameters["@ZID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
                            sqlCom.Parameters["@TS"].Value = Convert.ToDateTime(textBox4.Text);
                            sqlCom.Parameters["@TE"].Value = Convert.ToDateTime(textBox5.Text);
                            sqlCom.Parameters["@ID"].Value = Convert.ToInt32(AID);
                            sqlCom.ExecuteNonQuery();
                            Close();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Перепроверьте введенные значения, данные невозможно изменить.\n" +
                            "Подсказка: поле (ID) - целое число, поля (Начало) и (Окончание) - дата в формате dd.mm.yyyy hh:mm.",
                            "Ошибка");
                        transaction.Rollback();
                    }
                    
                }
                else
                {
                    try
                    {
                        using (SqlCommand sql = new SqlCommand())
                        {
                            SqlCommand sqlCom = new SqlCommand(@"INSERT INTO dbo.Appointment(WorkerID, EventID, ZoneID, TimeStart, TimeEnd)
                                                         VALUES (@WID, @EID, @ZID, @TS, @TE)", connection);
                            sqlCom.Transaction = transaction;
                            sqlCom.Parameters.Add("@WID", SqlDbType.Int);
                            sqlCom.Parameters.Add("@EID", SqlDbType.Int);
                            sqlCom.Parameters.Add("@ZID", SqlDbType.Int);
                            sqlCom.Parameters.Add("@TS", SqlDbType.DateTime);
                            sqlCom.Parameters.Add("@TE", SqlDbType.DateTime);

                            sqlCom.Parameters["@WID"].Value = Convert.ToInt32(textBox1.Text);
                            sqlCom.Parameters["@EID"].Value = Convert.ToInt32(EID);
                            sqlCom.Parameters["@ZID"].Value = Convert.ToInt32(comboBox1.SelectedIndex + 1);
                            sqlCom.Parameters["@TS"].Value = Convert.ToDateTime(textBox4.Text);
                            sqlCom.Parameters["@TE"].Value = Convert.ToDateTime(textBox5.Text);
                            sqlCom.ExecuteNonQuery();
                            Close();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Перепроверьте введенные значения, данные невозможно изменить.\n" +
                            "Подсказка: поле (ID) - целое число, поля (Начало) и (Окончание) - дата в формате dd.mm.yyyy hh:mm.",
                            "Ошибка");
                        transaction.Rollback();
                    }
                }
                connection.Close();
            }
        }
    }
}
