using System;
using System.Windows.Forms;

using MySql.Data.MySqlClient;


namespace labor_exchange
{
    public partial class LoginForm : Form
    {
        private Form1 opener;
        private bool isLogged = false;

        public LoginForm(Form1 parent)
        {   
            InitializeComponent();
            opener = parent;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "admin" && textBox1.Text != "client")
            {
                MessageBox.Show("Такой пользователь не существует", "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            string temp = Information.connString;
            temp = temp.Replace("password=erti56caru", "password=" + textBox2.Text).Replace("user id=root", "user id=" + textBox1.Text);

            var testConnection = new MySqlConnection(temp);

            try
            {
                testConnection.Open();
                testConnection.Close();

                MessageBox.Show("Вы авторизировались", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Information.currentUser = textBox1.Text;
                isLogged = true;

                this.Dispose();
            }
            catch
            {
                MessageBox.Show("Неверные данные", "Неудача", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void AuthorizationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLogged == false)
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
        }
    }
}