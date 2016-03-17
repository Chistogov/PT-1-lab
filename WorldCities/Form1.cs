using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace WorldCities
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool pr = false;
            //Имя пользователя
            string uid = textBox1.Text;
            //пароль
            string pass = textBox2.Text;
            // Заполнение строки подключения
            string Connect = "Data Source=localhost;User Id=" + uid + ";Password=" + pass;
            //создание подключения к БД (используется заполненая строка подключения)
            MySqlConnection myConnection = new MySqlConnection(Connect);
            //Открытие подключения 
            try
            {
                myConnection.Open();
                myConnection.Close();
                pr = true;
            }
            catch (Exception t)
            {
                pr = false;
                MessageBox.Show("В доступе отказано!");
            }
            if (pr == true)
            {
                Form2 MyForm = new Form2(uid, pass);
                MyForm.Show();
                this.Hide();
            }
        }
    }
}
