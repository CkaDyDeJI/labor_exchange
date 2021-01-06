using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Dapper;

using Microsoft.VisualBasic;

using MySql.Data.MySqlClient;

using Xceed.Document.NET;
using Xceed.Words.NET;


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

            fillComboBoxes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (person_viewDataGridView.SelectedCells.Count == 0 && position_viewDataGridView.SelectedCells.Count == 0)
                return;

            if (MessageBox.Show("Вы уверены, что хотите прикрепить к данному работнику выбранную вакансию?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (var conn = new MySqlConnection(Information.connString)) {
                conn.Query (
                    $"insert into proposition values ({person_viewDataGridView.SelectedCells[0].OwningRow.Cells[0].Value}, {position_viewDataGridView.SelectedCells[0].OwningRow.Cells[0].Value})");
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            var newPerson = new NewPerson();
            newPerson.ShowDialog();

            person_viewTableAdapter.Fill (labor_exchangeDataSet.person_view);
            person_viewBindingSource.DataSource = labor_exchangeDataSet.person_view;
            person_viewBindingSource.ResetBindings(false);
            //person_viewDataGridView.DataSource = person_viewBindingSource;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var newPosition = new NewPosition();
            newPosition.ShowDialog();

            position_viewTableAdapter.Fill(labor_exchangeDataSet.position_view);
            position_viewBindingSource.DataSource = labor_exchangeDataSet.position_view;
            position_viewBindingSource.ResetBindings(false);
            //position_viewDataGridView.DataSource = position_viewBindingSource;
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") {
                person_viewTableAdapter.Fill(this.labor_exchangeDataSet.person_view);
                person_viewBindingSource.DataSource = labor_exchangeDataSet.person_view;
                person_viewBindingSource.ResetBindings(false);
                //person_viewDataGridView.DataSource = person_viewBindingSource;
            } else {
                var fieldToSearch = comboBox1.Text;
                var valueToSearch = textBox1.Text;

                using (var conn = new MySqlConnection(Information.connString))
                {
                    var command = new MySqlCommand($"select * from person_view where {fieldToSearch} = '{valueToSearch}'");
                    command.Connection = conn;

                    var adapter = new MySqlDataAdapter(command);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);

                    person_viewBindingSource.DataSource = dataset.Tables[0];
                    person_viewBindingSource.ResetBindings(false);
                    //person_viewDataGridView.DataSource = bindingSource;
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                position_viewTableAdapter.Fill(this.labor_exchangeDataSet.position_view);
                position_viewBindingSource.DataSource = labor_exchangeDataSet.position_view;
                position_viewBindingSource.ResetBindings(false);
                //person_viewDataGridView.DataSource = person_viewBindingSource;
            }
            else
            {
                var fieldToSearch = comboBox2.Text;
                var valueToSearch = textBox2.Text;

                using (var conn = new MySqlConnection(Information.connString))
                {
                    var command = new MySqlCommand($"select * from position_view where {fieldToSearch} = '{valueToSearch}'");
                    command.Connection = conn;

                    var adapter = new MySqlDataAdapter(command);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);

                    position_viewBindingSource.DataSource = dataset.Tables[0];
                    position_viewBindingSource.ResetBindings(false);
                    //position_viewDataGridView.DataSource = bindingSource;
                }
            }
        }


        private void fillComboBoxes()
        {
            string[] tempList;
            using (var conn = new MySqlConnection(Information.connString))
            {
                tempList = conn.Query<String>($"SELECT column_name\nFROM information_schema.columns\nWHERE table_name='person_view';").ToArray();
                comboBox1.DataSource = tempList;

                tempList = conn.Query<String>($"SELECT column_name\nFROM information_schema.columns\nWHERE table_name='position_view';").ToArray();
                comboBox2.DataSource = tempList;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var passport = Interaction.InputBox ("Укажите номер пасспорта");

            if (passport != "") {
                if (MessageBox.Show("Вы уверены, что хотите удалить пользователя без возможности восстановления?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

                using (var conn = new MySqlConnection(Information.connString)) {
                    conn.Query ($"delete from jobless where passport = {passport}");
                    conn.Query ($"delete from job_book where id_person = {passport}");
                }

                MessageBox.Show ("Удаление завершено");

                person_viewTableAdapter.Fill(this.labor_exchangeDataSet.person_view);
                person_viewBindingSource.DataSource = labor_exchangeDataSet.person_view;
                person_viewBindingSource.ResetBindings(false);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "report";
            saveFileDialog1.ShowDialog();
            var path = saveFileDialog1.FileName;

            var conn = new MySqlConnection(Information.connString);
            conn.Open();

            DocX document = DocX.Create(path);
            document.InsertParagraph("Отчет о работе биржы труда").Font("Times New Roman").FontSize(18)
                .Alignment = Alignment.center;
            document.InsertParagraph();

            Table mainTable = document.AddTable (4, 2);
            mainTable.Alignment = Alignment.both;
            mainTable.AutoFit = AutoFit.ColumnWidth;
            mainTable.SetWidthsPercentage(new[] { 70.0f, 30.0f });
            mainTable.Design = TableDesign.None;


            var joblessCount = conn.QueryFirstOrDefault <string> ("select count(*) from jobless");
            mainTable.Rows[0].Cells[0].Paragraphs[0].Append ("Количество безработных:");
            mainTable.Rows[0].Cells[1].Paragraphs[0].Append (joblessCount);

            var jobCount = conn.QueryFirstOrDefault <string> ("select count(*) from archive");
            mainTable.Rows[1].Cells[0].Paragraphs[0].Append("Количество людей, нашедших работу:");
            mainTable.Rows[1].Cells[1].Paragraphs[0].Append(jobCount);

            var positionCount = conn.QueryFirstOrDefault<string>("select count(*) from position");
            mainTable.Rows[2].Cells[0].Paragraphs[0].Append("Общее количество вакансий:");
            mainTable.Rows[2].Cells[1].Paragraphs[0].Append(positionCount);

            var popularList = String.Join(", ", conn.Query<string>("select name from (select count(*) as count, name from archive_position\ngroup by name\norder by count desc) as A limit 5").ToArray());
            mainTable.Rows[3].Cells[0].Paragraphs[0].Append("Наиболее популярные вакансии:");
            mainTable.Rows[3].Cells[1].Paragraphs[0].Append(popularList);

            document.InsertTable (mainTable);
            document.Save();
            conn.Close();

            Process.Start (path);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var companybik = Interaction.InputBox ("Введите БИК компании: ");
            if (companybik == "")
                return;

            saveFileDialog1.FileName = "list";
            saveFileDialog1.ShowDialog();
            var path = saveFileDialog1.FileName;

            var conn = new MySqlConnection(Information.connString);
            conn.Open();

            DocX document = DocX.Create(path);
            document.InsertParagraph ("Список должностей").Font ("Times New Roman").FontSize (18).Alignment = Alignment.left;
            document.InsertParagraph ();
            
            var count = conn.QueryFirst <string> ($"select count(*) from position where company_id = {companybik}");
            var positionList = conn.Query <Position> ($"select name, payment, conditions, requirements from position where company_id = {companybik}").ToList();

            Table mainTable = document.AddTable (Convert.ToInt32(count) + 1, 4);
            mainTable.Alignment = Alignment.both;
            mainTable.AutoFit = AutoFit.ColumnWidth;
            mainTable.SetWidthsPercentage(new[] { 25.0f, 25.0f, 25.0f, 25.0f });
            mainTable.Design = TableDesign.None;

            mainTable.Rows[0].Cells[0].Paragraphs[0].Append("Название должнолсти");
            mainTable.Rows[0].Cells[1].Paragraphs[0].Append("Оплата");
            mainTable.Rows[0].Cells[2].Paragraphs[0].Append("Условия");
            mainTable.Rows[0].Cells[3].Paragraphs[0].Append("Требования");

            for (int i = 0; i < Convert.ToInt32(count); i++) {
                mainTable.Rows[i + 1].Cells[0].Paragraphs[0].Append (positionList[i].name);
                mainTable.Rows[i + 1].Cells[1].Paragraphs[0].Append (positionList[i].payment);
                mainTable.Rows[i + 1].Cells[2].Paragraphs[0].Append (positionList[i].conditions);
                mainTable.Rows[i + 1].Cells[3].Paragraphs[0].Append (positionList[i].requirements);
            }

            document.InsertTable (mainTable);

            document.InsertParagraph();
            var phone = conn.QueryFirstOrDefault <string> ($"select phone from company where bik = {companybik}");
            document.InsertParagraph ($"Звонить по телефону {phone}");

            document.Save();
            conn.Close();

            Process.Start(path);
        }
    }


    class Position
    {
        public string name { get; set; }
        public string payment { get; set; }
        public string conditions { get; set; }
        public string requirements { get; set; }
    }
}
