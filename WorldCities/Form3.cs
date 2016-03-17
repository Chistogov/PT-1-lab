using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace WorldCities
{
    public partial class Form3 : Form
    {
        public Form3(string user, string password)
        {
            InitializeComponent();
            User = user;
            Password = password;
            //Заполнение ComboBox
            RefreshComboBox("countries", comboBox1);
            comboBox1.SelectedIndex = 0;
            RefreshComboBox("mainlands", comboBox2);
            comboBox2.SelectedIndex = 0;
        }
        string User = "";
        string Password = "";
        string SelectedBD = "worldcities";

        //Заполнение ComboBox
        private void RefreshComboBox(string selectedTable, ComboBox box)
        {
            string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
            MySqlConnection myConnection = new MySqlConnection(Connect);
            string CommandText = "SELECT *FROM `" + selectedTable + "`";
            MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myCommand.ExecuteReader();           
            while (reader.Read())
            {
                box.Items.Add(reader.GetString(1));
            }
            reader.Close();            
            myConnection.Close();
        }
        
        //Добавление данных в таблицу "Страны"
        private void button2_Click(object sender, EventArgs e)
        {
            string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
            MySqlConnection myConnection = new MySqlConnection(Connect);
            string country = textBox2.Text;
            //проверки//
            string CommandText = "INSERT INTO  `worldcities`.`countries` (`Country_ID` ,`Name`)VALUES(NULL, '" + country + "');";
            MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myCommand.ExecuteReader();
            reader.Close();
            myConnection.Close();
            //Обновление Combobox
            RefreshComboBox("countries", comboBox1);
            comboBox1.SelectedIndex = 1;
            RefreshComboBox("mainlands", comboBox2);
            comboBox2.SelectedIndex = 1;
        }

        //Добавление записи в таблицу "Города"
        private void button1_Click(object sender, EventArgs e)
        {
            string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
            MySqlConnection myConnection = new MySqlConnection(Connect);
            string city = textBox1.Text;
            int population = 0;
            string countryID = Convert.ToString(comboBox1.SelectedIndex+1);
            string mainlandID = Convert.ToString(comboBox2.SelectedIndex+1);
            //проверки//
            if (city.Length > 20 || countryID.Length == 0 || mainlandID.Length == 0) { MessageBox.Show("Даные указаны некорректно!"); return; }
            try { population = Convert.ToInt32(textBox4.Text); } catch (Exception y) { MessageBox.Show("Неверно указано население!"); return; }
            string CommandText = "INSERT INTO  `worldcities`.`cities` (`ID` ,`City` ,`Population` ,`Country_ID` ,`Mainland_ID` ,`is_del`)VALUES(NULL, '"+ city + "', '"+ population + "', '"+ countryID + "', '"+ mainlandID + "', '0');";
            MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myCommand.ExecuteReader();
            reader.Close();
            myConnection.Close();
            MessageBox.Show("Город " + city + " успешно добавлен!");
        }
    }
}
