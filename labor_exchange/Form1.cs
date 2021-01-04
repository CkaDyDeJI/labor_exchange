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
    }
}
