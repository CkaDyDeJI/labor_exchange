using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labor_exchange
{
    public partial class Tables : Form
    {
        public Tables()
        {
            InitializeComponent();
        }

        private void archiveBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.archiveBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.labor_exchangeDataSet);

        }

        private void Tables_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.proposition' table. You can move, or remove it, as needed.
            this.propositionTableAdapter.Fill(this.labor_exchangeDataSet.proposition);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.position' table. You can move, or remove it, as needed.
            this.positionTableAdapter.Fill(this.labor_exchangeDataSet.position);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.jobless' table. You can move, or remove it, as needed.
            this.joblessTableAdapter.Fill(this.labor_exchangeDataSet.jobless);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.job_book' table. You can move, or remove it, as needed.
            this.job_bookTableAdapter.Fill(this.labor_exchangeDataSet.job_book);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.company' table. You can move, or remove it, as needed.
            this.companyTableAdapter.Fill(this.labor_exchangeDataSet.company);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.archive_position' table. You can move, or remove it, as needed.
            this.archive_positionTableAdapter.Fill(this.labor_exchangeDataSet.archive_position);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.archive_person' table. You can move, or remove it, as needed.
            this.archive_personTableAdapter.Fill(this.labor_exchangeDataSet.archive_person);
            // TODO: This line of code loads data into the 'labor_exchangeDataSet.archive' table. You can move, or remove it, as needed.
            this.archiveTableAdapter.Fill(this.labor_exchangeDataSet.archive);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            archiveDataGridView.Dispose();
            archiveDataGridView = new DataGridView {Parent = panel1, Dock = DockStyle.Fill};
            //archiveDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //mARKSDataGridView.Columns.Clear();
            //mARKSDataGridView.Rows.Clear();

            if (comboBox1.SelectedIndex == 0)
            {
                archiveDataGridView.DataSource = archiveBindingSource;
                archiveBindingNavigator.BindingSource = archiveBindingSource;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                archiveDataGridView.DataSource = archive_personBindingSource;
                archiveBindingNavigator.BindingSource = archive_personBindingSource;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                archiveDataGridView.DataSource = archive_positionBindingSource;
                archiveBindingNavigator.BindingSource = archive_positionBindingSource;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                archiveDataGridView.DataSource = companyBindingSource;
                archiveBindingNavigator.BindingSource = companyBindingSource;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                archiveDataGridView.DataSource = job_bookBindingSource;
                archiveBindingNavigator.BindingSource = job_bookBindingSource;
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                archiveDataGridView.DataSource = joblessBindingSource;
                archiveBindingNavigator.BindingSource = joblessBindingSource;
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                archiveDataGridView.DataSource = positionBindingSource;
                archiveBindingNavigator.BindingSource = positionBindingSource;
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                archiveDataGridView.DataSource = propositionBindingSource;
                archiveBindingNavigator.BindingSource = propositionBindingSource;
            }

            label2.Text = $"Количество записей в таблице = {archiveDataGridView.RowCount - 1}";
            //mARKSDataGridView.Refresh();
        }
    }
}
