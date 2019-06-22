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

namespace VolunteersBase
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            switch (textBox1.Text + " " + textBox2.Text)
            {
                case "vol 123":
                    {
                        Hide();
                        VolunteerForm manForm = new VolunteerForm();
                        manForm.ShowDialog();
                        Close();
                        break;
                    }
                case "coor 123":
                    {
                        Hide();
                        CoordinatorForm workForm = new CoordinatorForm();
                        workForm.ShowDialog();
                        Close();
                        break;
                    }
                case "org 123":
                    {
                        Hide();
                        OrganizatorForm clientForm = new OrganizatorForm();
                        clientForm.ShowDialog();
                        Close();
                        break;
                    }
                default:
                    {
                        MessageBox.Show("Неверно указан логин или пароль. Введите данные заново", "Ошибка");
                        textBox2.Text = "";
                        break;
                    }
            }
        }
    }
}
