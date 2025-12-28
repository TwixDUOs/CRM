using System;
using System.Collections.Generic;
using System.Data.SQLite; // Переконайся, що ця бібліотека підключена
using System.Windows;
using Microsoft.Win32; // Для OpenFileDialog
using System.IO;       // Для File.ReadAllLines
using System.Linq;

namespace Management_SYS
{
    public partial class AnalyticsWindow : Window
    {

        // Обробник натискання кнопки "Імпорт CSV"
        private void ImportCsv_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    ImportDataFromCsv(filePath);

                    // Оновлюємо таблицю сегментації
                    CalculateRfm_Click(null, null);

                    // ВАЖЛИВО: Оновлюємо список клієнтів у випадаючому списку!
                    LoadClientsForCombo();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка імпорту: " + ex.Message);
                }
            }
        }

        // Основна логіка парсингу та збереження в БД
        private void ImportDataFromCsv(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Використовуємо транзакцію! Це прискорює запис в 100 разів при великих об'ємах
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (string line in lines)
                    {
                        // Пропускаємо порожні рядки
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Розбиваємо рядок за комою
                        var parts = line.Split(',');

                        // Очікуємо формат: Name, Date, Amount (3 колонки)
                        if (parts.Length < 3) continue;

                        string clientName = parts[0].Trim();
                        string dateStr = parts[1].Trim();
                        string amountStr = parts[2].Trim();

                        // 1. Знаходимо або створюємо клієнта
                        int customerId = GetOrCreateCustomer(connection, clientName);

                        // 2. Додаємо запис про продаж
                        string insertSaleQuery = "INSERT INTO Sales (customer_id, sale_date, amount) VALUES (@cid, @date, @amt)";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertSaleQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@cid", customerId);
                            cmd.Parameters.AddWithValue("@date", dateStr);
                            cmd.Parameters.AddWithValue("@amt", Convert.ToDouble(amountStr));
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit(); // Зберігаємо всі зміни разом
                }
            }
            MessageBox.Show($"Успішно імпортовано {lines.Length} рядків!");
        }

        // Допоміжний метод: повертає ID клієнта. Якщо клієнта немає - створює його.
        private int GetOrCreateCustomer(SQLiteConnection conn, string name)
        {
            // Спробуємо знайти клієнта
            string checkSql = "SELECT customerID FROM Customers WHERE nameOfCustomer = @name";
            using (SQLiteCommand cmd = new SQLiteCommand(checkSql, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
            }

            // Якщо не знайшли - створюємо нового
            string insertSql = "INSERT INTO Customers (nameOfCustomer, phoneNumberOfCustomer, lastContact, nextContact) VALUES (@name, 'Unknown', @date, 'Unknown')";
            using (SQLiteCommand cmd = new SQLiteCommand(insertSql, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }

            // Повертаємо ID щойно створеного клієнта
            string getLastIdSql = "SELECT last_insert_rowid()";
            using (SQLiteCommand cmd = new SQLiteCommand(getLastIdSql, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        // ШЛЯХ ДО БАЗИ ДАНИХ - ЗМІНИ НА СВІЙ
        // Для WPF зазвичай використовують відносний шлях, якщо БД лежить біля .exe
        string connectionString = @"Data Source=BD_file.db;Version=3;";

        public AnalyticsWindow()
        {
            InitializeComponent();
            LoadClientsForCombo();
            LoadClientsForCombo();
        }

        private void CalculateRfm_Click(object sender, RoutedEventArgs e)
        {
            List<RfmClientData> rawData = LoadDataFromDb();

            if (rawData.Count == 0)
            {
                MessageBox.Show("Немає даних про продажі для аналізу.");
                return;
            }

            // Викликаємо наш математичний алгоритм
            // Створюємо копію для відображення, бо алгоритм нормалізує (змінює) числа
            var analysisResult = KMeansClustering.PerformClustering(rawData, 3);

            RfmGrid.ItemsSource = analysisResult;
            MessageBox.Show("Кластеризацію завершено успішно!");
        }

        private List<RfmClientData> LoadDataFromDb()
        {
            var list = new List<RfmClientData>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Складний SQL запит, який збирає дані з трьох таблиць:
                    // 1. Рахує суму покупок (Sales)
                    // 2. Рахує кількість контактів (Contact_story)
                    // 3. Знаходить дату останнього контакту
                    string sql = @"
                        SELECT 
                            c.customerID, 
                            c.nameOfCustomer,
                            IFNULL(SUM(s.amount), 0) as TotalMoney,
                            COUNT(DISTINCT s.sale_id) as Frequency,
                            MAX(cs.time_of_contact) as LastContactDate
                        FROM Customers c
                        LEFT JOIN Sales s ON c.customerID = s.customer_id
                        LEFT JOIN Contact_story cs ON c.customerID = cs.id_of_customer
                        GROUP BY c.customerID";

                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var client = new RfmClientData();
                        client.CustomerId = Convert.ToInt32(reader["customerID"]);
                        client.Name = reader["nameOfCustomer"].ToString();
                        client.Monetary = Convert.ToDouble(reader["TotalMoney"]);

                        // Розрахунок Recency (днів з останнього контакту)
                        string dateStr = reader["LastContactDate"].ToString();
                        if (!string.IsNullOrEmpty(dateStr))
                        {
                            // Спробуємо розпарсити дату. Припускаємо формат, який у тебе в БД.
                            // Якщо в БД просто текст, може знадобитися DateTime.Parse
                            if (DateTime.TryParse(dateStr, out DateTime lastDate))
                            {
                                client.Recency = (DateTime.Now - lastDate).TotalDays;
                            }
                            else
                            {
                                client.Recency = 365; // Якщо дата крива, вважаємо що давно
                            }
                        }
                        else
                        {
                            client.Recency = 365; // Ніколи не контактували
                        }

                        // Frequency - поєднуємо покупки і контакти, або беремо тільки покупки
                        client.Frequency = Convert.ToDouble(reader["Frequency"]);

                        list.Add(client);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка БД: " + ex.Message);
                }
            }
            return list;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Викликати це в конструкторі: public AnalyticsWindow() { ... LoadClientsForCombo(); }
        private void LoadClientsForCombo()
        {
            var clients = new List<RfmClientData>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "SELECT customerID, nameOfCustomer FROM Customers";
                    SQLiteCommand command = new SQLiteCommand(sql, connection);
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        clients.Add(new RfmClientData
                        {
                            CustomerId = Convert.ToInt32(reader["customerID"]),
                            Name = reader["nameOfCustomer"].ToString()
                        });
                    }
                }
                catch { }
            }
            ClientComboBox.ItemsSource = clients;
        }

        // Обробка кнопки прогнозування
        private void Predict_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedValue == null)
            {
                MessageBox.Show("Будь ласка, оберіть клієнта зі списку.");
                return;
            }

            int customerId = (int)ClientComboBox.SelectedValue;
            List<double> salesHistory = GetSalesHistory(customerId);

            if (salesHistory.Count == 0)
            {
                PredictionResultText.Text = "Недостатньо даних для прогнозу (немає продажів).";
                return;
            }

            // ВИКЛИК НОВОЇ МАТЕМАТИЧНОЇ МОДЕЛІ
            double predictedAmount = PredictionModel.PredictNextMonthSales(salesHistory);

            string trend = predictedAmount > salesHistory.Last() ? "Зростаючий 📈" : "Спадаючий 📉";

            PredictionResultText.Text = $"Аналіз історії ({salesHistory.Count} угод). \n" +
                                        $"Прогноз на наступний період: {predictedAmount:C2} ({trend})";
        }

        // Отримання історії продажів для одного клієнта (для регресії)
        private List<double> GetSalesHistory(int customerId)
        {
            var history = new List<double>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Беремо продажі, відсортовані за датою
                string sql = "SELECT amount FROM Sales WHERE customer_id = @id ORDER BY sale_date";
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", customerId);

                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    history.Add(Convert.ToDouble(reader["amount"]));
                }
            }
            return history;
        }
    }
}