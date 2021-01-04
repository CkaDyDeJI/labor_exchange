using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Dapper;

using Microsoft.VisualBasic;

using MySql.Data.MySqlClient;


namespace labor_exchange
{
    public partial class UserForm : Form
    {
        private Form1 opener;
        private string user;

        public UserForm(Form1 parentForm)
        {
            InitializeComponent();
            opener = parentForm;
        }


        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Закрытие этого окна без входа приведет к закрытию всего приложения.\nВы хотите продолжить?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Dispose();
                opener.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
        }


        private void UserForm_Load(object sender, EventArgs e)
        {
            FillData();
        }


        private void FillData()
        {
            while (true)
            {
                user = Interaction.InputBox("Укажите ваш ID (паспорт)");

                if (user != "")
                {
                    using (var conn = new MySqlConnection(Information.connString))
                    {
                        if (!conn.Query<int>($"select count(fio) from jobless where passport = {user}").Any())
                        {
                            MessageBox.Show("Человек не найден");

                            continue;
                        }

                        label1.Text = conn.QueryFirst <string> ($"select fio from jobless where passport = {user}");

                        var command = new MySqlCommand($"select comp_pos_view.* from comp_pos_view join proposition on proposition.position_id = comp_pos_view.id join jobless on jobless.passport = proposition.person_id where jobless.passport = {user}");
                        command.Connection = conn;

                        var adapter = new MySqlDataAdapter(command);
                        var dataset = new DataSet();
                        adapter.Fill(dataset);

                        var bindingSource = new BindingSource();
                        bindingSource.DataSource = dataset.Tables[0];
                        dataGridView1.DataSource = bindingSource;

                        break;
                    }
                }
                else
                {
                    if (MessageBox.Show("Такого пользователя не существует. Вы хотите выйти?", "Внимание",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.Dispose();
                        opener.Dispose();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            if (MessageBox.Show (
                "Вы принимаете данную должность? Принятие удалит остальные ваканси и переведет вас в неактивный статус",
                "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var temp = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            using (var conn = new MySqlConnection(Information.connString)) {
                conn.Query ($"insert into archive_position select * from position where id = {temp}");
                conn.Query ($"update job_book set last_job_place_id = {temp} where id_person = {user}");
                conn.Query ($"delete from position where id = {temp}");
                conn.Query ($"delete from jobless where passport = {user}");
                conn.Query ($"insert into archive values ({user}, {temp})");
                //conn.Query($"delete from proposition where person_id = {user} or position_id = {temp}");
            }
        }
    }
}
