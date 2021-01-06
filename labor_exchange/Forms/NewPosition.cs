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

using Org.BouncyCastle.Crypto.Engines;


namespace labor_exchange
{
    public partial class NewPosition : Form
    {
        public NewPosition()
        {
            InitializeComponent();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false) {
                comboBox1.Enabled = false;
                groupBox2.Enabled = true;
            } else {
                groupBox2.Enabled = false;
                comboBox1.Enabled = true;
            }
        }


        private void NewPosition_Load(object sender, EventArgs e)
        {
            string[] comboBoxPosibilities;
            using (var conn = new MySqlConnection(Information.connString))
            {
                comboBoxPosibilities = conn.Query<string>("select name from company").ToArray();
            }

            comboBox1.DataSource = comboBoxPosibilities;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try {
                using (var conn = new MySqlConnection(Information.connString)) {
                    if (checkBox1.Checked == true) {
                        string tempCom = conn.QueryFirst <string> ($"select bik from company where name = '{comboBox1.Text}'");
                        conn.Query (
                            $"insert into position (company_id, name, payment, conditions, requirements) values ({tempCom}, '{textBox2.Text}', {textBox3.Text}, '{textBox4.Text}', '{textBox5.Text}')");
                    } else {
                        conn.Query ($"insert into company values ({textBox6.Text}, '{textBox7.Text}', '{textBox8.Text}', '{textBox9.Text}', '{textBox10.Text}')");
                        conn.Query ($"insert into position (company_id, name, payment, conditions, requirements) values ({textBox6.Text}, '{textBox2.Text}', {textBox3.Text}, '{textBox4.Text}', '{textBox5.Text}')");

                        MessageBox.Show("Добавлено успешно");
                        this.Dispose();
                    }
                }
            }
            catch {
                MessageBox.Show("Не все поля заполнены");
            }
            
        }
    }
}
