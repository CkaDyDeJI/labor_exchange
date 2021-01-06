using System;
using System.Linq;
using System.Windows.Forms;

using Dapper;

using MySql.Data.MySqlClient;


namespace labor_exchange
{
    public partial class NewPerson : Form
    {
        public NewPerson()
        {
            InitializeComponent();
        }

        private void NewPerson_Load(object sender, EventArgs e)
        {
            string[] comboBoxPosibilities;
            using (var conn = new MySqlConnection(Information.connString)) {
                comboBoxPosibilities = conn.Query <string> ("select name from company").ToArray();
            }

            comboBox1.DataSource = comboBoxPosibilities;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] comboBoxPosibilities;
            using (var conn = new MySqlConnection(Information.connString))
            {
                comboBoxPosibilities = conn.Query<string>($"select archive_position.name from archive_position join company on company.bik = archive_position.company_id where company.name = '{comboBox1.Text}'").ToArray();
            }

            comboBox2.DataSource = comboBoxPosibilities;
            comboBox2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                using (var conn = new MySqlConnection(Information.connString)) {
                    conn.Query ($"insert into jobless values ({textBox1.Text}, '{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{textBox6.Text}')");

                    var tempComp = conn.QueryFirstOrDefault <string> ($"select bik from company where name = '{comboBox1.Text}'");
                    var tempPos = conn.QueryFirstOrDefault <string> ($"select id from archive_position where name = '{comboBox2.Text}'");
                    conn.Query ($"insert into job_book(id_person, profession, education, requirements, last_job_place_id, position, reason) values ({textBox1.Text}, '{textBox7.Text}', '{textBox8.Text}', '{textBox9.Text}', {tempComp}, {tempPos}, '{textBox12.Text}')");

                    MessageBox.Show ("Добавлено успешно");
                    this.Dispose();
                }
            }
            catch {
                MessageBox.Show ("Не все поля заполнены");
            }
        }
    }
}
