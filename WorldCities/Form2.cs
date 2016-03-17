using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace WorldCities
{
    public partial class Form2 : Form
    {
        public Form2(string user, string password)
        {
            InitializeComponent();
            User = user;
            Password = password;
            RefreshTable();
        }
        string User = "";
        string Password = "";
        string SelectedBD = "worldcities";
        int page = 0;
        string selectedTable = "cities";

        //Обновление данных в таблице
        private void RefreshTable()
        {
            string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
            MySqlConnection myConnection = new MySqlConnection(Connect);
            string CommandText = "";
            if (selectedTable != "cities")            
                CommandText = "SELECT *FROM `" + selectedTable + "` LIMIT " + Convert.ToString((numericUpDown1.Value * page)) + "," + Convert.ToString(numericUpDown1.Value);
           
            else
                CommandText = "SELECT cities.ID, cities.City, cities.Population, countries.Name as Country, mainlands.Name as Mainland, cities.is_del FROM  `cities` ,  `countries` ,  `mainlands` WHERE(cities.Country_ID = countries.Country_ID) && (cities.Mainland_ID = mainlands.Mainland_ID) && (cities.is_del = 0) LIMIT " + Convert.ToString(numericUpDown1.Value * page) + "," + Convert.ToString(numericUpDown1.Value);
           // MessageBox.Show(CommandText);
            MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
            myConnection.Open();            
            MySqlDataReader reader = myCommand.ExecuteReader();
            int i = reader.VisibleFieldCount;
            if (i > 0 && reader.HasRows)
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                int row = 0;
                if (selectedTable != "cities")
                {
                    for (int l = 0; l < i; l++)
                        dataGridView.Columns.Add(reader.GetName(l), reader.GetName(l));
                }
                else
                {
                    for (int l = 0; l < i - 1; l++)
                        dataGridView.Columns.Add(reader.GetName(l), reader.GetName(l));
                }

                while (reader.Read())
                {
                    if (reader.GetString(i - 1) == "0" && selectedTable == "cities")
                    {
                        dataGridView.Rows.Add();
                        for (int cell = 0; cell < i - 1; cell++)
                            try
                            {
                                dataGridView.Rows[row].Cells[cell].Value = (reader.GetString(cell));
                            }
                            catch { dataGridView.Rows[row].Cells[cell].Value = ""; }
                        row++;
                    }

                    if (selectedTable != "cities")
                    {
                        dataGridView.Rows.Add();
                        for (int cell = 0; cell < i; cell++)
                            try
                            {
                                dataGridView.Rows[row].Cells[cell].Value = (reader.GetString(cell));
                            }
                            catch { dataGridView.Rows[row].Cells[cell].Value = ""; }
                        row++;
                    }
                }
                reader.Close();
            }
            else
                page--;
            label2.Text = "Страница: " + page;
            myConnection.Close();
            dataGridView.Columns[0].Visible = false;
        }
        //Кнопка "Добавить"
        private void добавитьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Form3 MyForm = new Form3(User, Password);
            MyForm.Show();
        }
        //Кнопка "Города"
        private void button1_Click(object sender, System.EventArgs e)
        {
            selectedTable = "cities";
            page = 0;
            label2.Text = "Страница: " + page;
            RefreshTable();
            удалитьВыделенныеToolStripMenuItem.Enabled = true;
        }
        //Кнопка "Страны"
        private void button2_Click(object sender, System.EventArgs e)
        {
            selectedTable = "countries";
            page = 0;
            label2.Text = "Страница: " + page;
            RefreshTable();
            удалитьВыделенныеToolStripMenuItem.Enabled = false;
        }
        //Кнопка "Материки"
        private void button3_Click(object sender, System.EventArgs e)
        {
            selectedTable = "mainlands";
            page = 0;
            label2.Text = "Страница: " + page;
            RefreshTable();
            удалитьВыделенныеToolStripMenuItem.Enabled = false;
        }
        //Удаление выделенных записей
        private void удалитьВыделенныеToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                string id = Convert.ToString(row.Cells[0].Value);
             //   MessageBox.Show(id);
                string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
                MySqlConnection myConnection = new MySqlConnection(Connect);               
                string CommandText = "UPDATE  `worldcities`.`cities` SET  `is_del` = '1' WHERE  `cities`.`ID` = " + id + ";";
                MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
                myConnection.Open();                
                MySqlDataReader reader = myCommand.ExecuteReader();               
                reader.Close();                
                myConnection.Close();
                MessageBox.Show("Запись удалена, код:" + id);
                RefreshTable();
            }

        }
        //Кнопка "Назад"
        private void button7_Click(object sender, EventArgs e)
        {
            if (page > 0)
            {
                page--;
                label2.Text = "Страница: " + page;
                RefreshTable();
            }
        }
        //Кнопка "Вперед"
        private void button6_Click(object sender, EventArgs e)
        {
            page++;
            label2.Text = "Страница: " + page;
            RefreshTable();
        }
        //Выход из программы
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        //Кнопка "Переход на первую страницу"
        private void button5_Click(object sender, EventArgs e)
        {
            page = 0;
            label2.Text = "Страница: " + page;
            RefreshTable();
        }
        //Кнопка "Переход на последнюю страницу"
        private void button4_Click(object sender, EventArgs e)
        {
            int count = 0;
            string Connect = "Database=" + SelectedBD + ";Data Source=localhost;User Id=" + User + ";Password=" + Password;
            MySqlConnection myConnection = new MySqlConnection(Connect);
            string CommandText = "";
            if(SelectedBD != "cities")
                CommandText = "SELECT COUNT(*) FROM `" + selectedTable + "`" ;
            else
                CommandText = "SELECT COUNT(*) FROM `" + selectedTable + "` WHERE is_del = 0";
            MySqlCommand myCommand = new MySqlCommand(CommandText, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                count = Convert.ToInt32(reader.GetString(0));
            }
            reader.Close();
            myConnection.Close();

            page = Convert.ToInt32(count / numericUpDown1.Value);
            label2.Text = "Страница: "+page;
            RefreshTable();
        }
    }
}
