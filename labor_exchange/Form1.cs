using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Dapper;

using MySql.Data.MySqlClient;


namespace labor_exchange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var authForm = new LoginForm(this);
            authForm.ShowDialog();

            LoadProperForm();
        }


        private void LoadProperForm()
        {
            if (Information.currentUser == "client") {
                var userForm = new UserForm(this);
                userForm.ShowDialog();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var tablesForm = new Tables();
            tablesForm.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.position_view' table. You can move, or remove it, as needed.
            this.position_viewTableAdapter.Fill(this.labor_exchangeDataSet.position_view);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.person_view' table. You can move, or remove it, as needed.
            this.person_viewTableAdapter.Fill(this.labor_exchangeDataSet.person_view);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (person_viewDataGridView.SelectedRows.Count == 0 && position_viewDataGridView.SelectedRows.Count == 0)
                return;

            if (MessageBox.Show("Вы уверены, что хотите прикрепить к данному работнику выбранную вакансию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (var conn = new MySqlConnection(Information.connString)) {
                conn.Query (
                    $"insert into proposition values ({person_viewDataGridView.SelectedRows[0].Cells[0].Value}, {position_viewDataGridView.SelectedRows[0].Cells[0].Value})");


            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var newPerson = new NewPerson();
            newPerson.ShowDialog();

            position_viewTableAdapter.Fill(labor_exchangeDataSet.position_view);
        }
    }
}
