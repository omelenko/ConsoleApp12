using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using Microsoft.Data.Sqlite;

internal class Program
{
    static void Main()
    {
        string connectionString = "Data source=:memory:";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Пiдключено до бази даних");
                CreateTable(connection);
                DisplayAllStorage(connection);
                DisplayAllProducts(connection);
                DisplayAllTypes(connection);
                DisplayAllSuppliers(connection);
                DisplayMaxQuantity(connection);
                DisplayMinQuantity(connection);
                DisplayMaxCostPrice(connection);
                DisplayMinCostPrice(connection);
                DisplayAvgQuantityByType(connection);
                int n;
                Console.Write("Iнформацiя про товар: Введiть iд: ");
                while (!int.TryParse(Console.ReadLine(), out n)) Console.WriteLine("Помилка.");
                DisplayAllInfoOfAProduct(connection, n);
                string a, b;
                Console.Write("Iнформацiя про товар: Введiть тип: ");
                a = Console.ReadLine()!;
                DisplayItemsByType(connection, a);
                Console.Write("Iнформацiя про товар: Введiть постачальника: ");
                b = Console.ReadLine()!;
                DisplayItemsBySupplier(connection, b);
                DisplayOldestItemInStorage(connection);
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка пiдключення до бази даних: " + ex.Message);
            }
        }
    }

    static void CreateTable(SqliteConnection connection)
    {
        SqliteCommand c = new SqliteCommand("CREATE TABLE Products (\r\n    ProductId INTEGER PRIMARY KEY AUTOINCREMENT,\r\n    Name TEXT NOT NULL,\r\n    Type TEXT NOT NULL,\r\n    Cost REAL NOT NULL,\r\n    Supplier TEXT NOT NULL\r\n);", connection);
        c.ExecuteNonQuery();
        SqliteCommand c1 = new SqliteCommand("CREATE TABLE Storage (\r\n    StorageId INTEGER PRIMARY KEY AUTOINCREMENT,\r\n    ProductId INTEGER,\r\n    Quantity INTEGER NOT NULL,\r\n    CostPrice REAL NOT NULL,\r\n    SupplyDate DATE NOT NULL,\r\n    FOREIGN KEY (ProductId) REFERENCES Products (ProductId)\r\n);", connection);
        c1.ExecuteNonQueryAsync();

        SqliteCommand ce = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Яблуко', 'Фрукт', 10.5, 'Постачальник1');", connection);
        ce.ExecuteNonQueryAsync();
        SqliteCommand ce1 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Груша', 'Фрукт', 15.2, 'Постачальник1');", connection);
        ce1.ExecuteNonQueryAsync();
        SqliteCommand ce2 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Огірок', 'Овоч', 15.2, 'Постачальник2');", connection);
        ce2.ExecuteNonQueryAsync();
        SqliteCommand ce3 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Помідор', 'Овоч', 15.2, 'Постачальник2');", connection);
        ce3.ExecuteNonQueryAsync();
        SqliteCommand ce4 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Корега', 'Інше', 15.2, 'Постачальник3');", connection);
        ce4.ExecuteNonQueryAsync();
        SqliteCommand ce5 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Кросівки', 'Інше', 15.2, 'Постачальник3');", connection);
        ce5.ExecuteNonQueryAsync();
        SqliteCommand ce6 = new SqliteCommand("INSERT INTO Products(ProductId, Name, Type, Cost, Supplier) VALUES(NULL, 'Зубна щітка', 'Інше', 15.2, 'Постачальник3');", connection);
        ce6.ExecuteNonQueryAsync();

        SqliteCommand c1e = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 1, 100, 10.0, '2023-01-01');", connection);
        c1e.ExecuteNonQueryAsync();
        SqliteCommand c1e2 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 2, 50, 12.5, '2023-02-01');", connection);
        c1e2.ExecuteNonQueryAsync();
        SqliteCommand c1e3 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 3, 75, 9.75, '2022-11-20');", connection);
        c1e3.ExecuteNonQueryAsync();
        SqliteCommand c1e4 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 4, 80, 15.5, '2023-03-10');", connection);
        c1e4.ExecuteNonQueryAsync();
        SqliteCommand c1e5 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 5, 40, 20.9, '2020-02-01');", connection);
        c1e5.ExecuteNonQueryAsync();
        SqliteCommand c1e6 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 6, 150, 30.1, '2022-10-12');", connection);
        c1e6.ExecuteNonQueryAsync();
        SqliteCommand c1e7 = new SqliteCommand("INSERT INTO Storage(StorageId, ProductId, Quantity, CostPrice, SupplyDate) VALUES(NULL, 7, 90, 20.4, '2023-04-21');", connection);
        c1e7.ExecuteNonQueryAsync();

    }

    static void DisplayAllStorage(SqliteConnection connection)
    {
        string query = $"SELECT * FROM Storage;";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("StorageId\tProductId\tQuantity\tCostPrice\tSupplyDate\t");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["StorageId"]}\t\t {row["ProductId"]}\t\t {row["Quantity"]}\t\t {row["CostPrice"]}\t\t {row["SupplyDate"]}");
        }
    }
    static void DisplayAllProducts(SqliteConnection connection)
    {
        string query = $"SELECT * FROM Products;";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("ProductId\tName\t\tType\t\tCost\t\tSupplier");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["ProductId"]}\t\t {row["Name"]}\t\t {row["Type"]}\t\t {row["Cost"]}\t\t {row["Supplier"]}");
        }
    }

    static void DisplayAllTypes(SqliteConnection connection)
    {
        //Відображення всіх типів товарів.
        Console.WriteLine($"Всi типи товарiв: ");
        string query = "SELECT Type FROM Products;";
        SqliteCommand command = new SqliteCommand(query, connection);
        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(reader["Type"]);
        }

        reader.Close();
    }

    static void DisplayAllSuppliers(SqliteConnection connection)
    {
        //Відображення всіх постачальників.
        Console.WriteLine("Всi постачальники: ");
        string query = "SELECT Supplier FROM Products;";
        SqliteCommand command = new SqliteCommand(query, connection);
        SqliteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(reader["Supplier"]);
        }

        reader.Close();
    }

    static void DisplayMaxQuantity(SqliteConnection connection)
    {
        //Показати товар з максимальною кількістю.
        SqliteCommand maxQuantityCommand = new SqliteCommand("SELECT MAX(Quantity) FROM Storage", connection);
        Int64 maxQuantity = (Int64)maxQuantityCommand.ExecuteScalar()!;
        Console.WriteLine($"Tовар з максимальною кiлькiстю: {maxQuantity}");
    }
    static void DisplayMinQuantity(SqliteConnection connection)
    {
        //Показати товар з мінімальною кількістю.
        SqliteCommand minQuantityCommand = new SqliteCommand("SELECT MIN(Quantity) FROM Storage", connection);
        Int64 minQuantity = (Int64)minQuantityCommand.ExecuteScalar()!;
        Console.WriteLine($"Tовар з мiнiмальною кiлькiстю: {minQuantity}");
    }
    static void DisplayMaxCostPrice(SqliteConnection connection)
    {
        //Показати товар з максимальною собівартістю.
        SqliteCommand maxCostPriceCommand = new SqliteCommand("SELECT MAX(CostPrice) FROM Storage", connection);
        double maxCostPrice = (double)maxCostPriceCommand.ExecuteScalar()!;
        Console.WriteLine($"Tовар з максимальною собiвартiстю: {maxCostPrice}");
    }
    static void DisplayMinCostPrice(SqliteConnection connection)
    {
        //Показати товар з мінімальною собівартістю.
        SqliteCommand minCostPriceCommand = new SqliteCommand("SELECT MIN(CostPrice) FROM Storage", connection);
        double minCostPrice = (double)minCostPriceCommand.ExecuteScalar()!;
        Console.WriteLine($"Tовар з мiнiмальною собiвартiстю: {minCostPrice}");
    }
    static void DisplayAvgQuantityByType(SqliteConnection connection)
    {
        //Показати середню кількість товарів за кожним типом товару.
        SqliteCommand avgQuantityCommand = new SqliteCommand("SELECT AVG(Quantity) FROM Storage WHERE ProductId IN ( SELECT ProductId FROM Products WHERE Type='Фрукт');", connection);
        double avgQuantity = (double)avgQuantityCommand.ExecuteScalar()!;
        SqliteCommand avgQuantityCommand1 = new SqliteCommand("SELECT AVG(Quantity) FROM Storage WHERE ProductId IN ( SELECT ProductId FROM Products WHERE Type='Овоч');", connection);
        double avgQuantity1 = (double)avgQuantityCommand.ExecuteScalar()!;
        SqliteCommand avgQuantityCommand2 = new SqliteCommand("SELECT AVG(Quantity) FROM Storage WHERE ProductId IN ( SELECT ProductId FROM Products WHERE Type='Інше');", connection);
        double avgQuantity3 = (double)avgQuantityCommand.ExecuteScalar()!;
        Console.WriteLine($"Cередня кiлькiсть товарiв за кожним типом товару:\nФрукт: {avgQuantity}\nОвоч: {avgQuantity1}\nІнше: {avgQuantity3}");
    }
    static void DisplayAllInfoOfAProduct(SqliteConnection connection, int productid)
    {
        //Відображення всієї інформації про товар.
        Console.WriteLine($"Iнформацiя про товар з iд: {productid}");
        string query = $"SELECT * FROM Products WHERE ProductId = {productid};";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("ProductId\tName\t\tType\t\tCost\t\tSupplier");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["ProductId"]}\t\t {row["Name"]}\t\t {row["Type"]}\t\t {row["Cost"]}\t\t {row["Supplier"]}");
        }
    }

    static void DisplayItemsByType(SqliteConnection connection, string type)
    {
        //Показати товари заданої категорії.
        Console.WriteLine($"Iнформацiя про товар з таким типом: '{type}'");
        string query = $"SELECT * FROM Products WHERE Type = '{type}';";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("ProductId\tName\t\tType\t\tCost\t\tSupplier");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["ProductId"]}\t\t {row["Name"]}\t\t {row["Type"]}\t\t {row["Cost"]}\t\t {row["Supplier"]}");
        }
    }
    static void DisplayItemsBySupplier(SqliteConnection connection, string supplier)
    {
        //Показати товари заданого постачальника.
        Console.WriteLine($"Iнформацiя про товар з постачальником: '{supplier}'");
        string query = $"SELECT * FROM Products WHERE Supplier = '{supplier}';";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("ProductId\tName\t\tType\t\tCost\t\tSupplier");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["ProductId"]}\t\t {row["Name"]}\t\t {row["Type"]}\t\t {row["Cost"]}\t\t {row["Supplier"]}");
        }
    }
    static void DisplayOldestItemInStorage(SqliteConnection connection)
    {
        //Показати товар, який знаходиться на складі найдовше з усіх.
        Console.WriteLine($"Найстарiший товар в сховищi:");
        string query1 = "SELECT MIN(SupplyDate) FROM Storage;";
        SqliteCommand command1 = new SqliteCommand(query1, connection);
        SqliteDataReader reader1 = command1.ExecuteReader();
        string temp = "";
        while (reader1.Read())
        {
            temp += reader1["min(SupplyDate)"];
        }

        reader1.Close();

        string query = $"SELECT * FROM Products WHERE ProductId IN ( SELECT ProductId FROM Storage WHERE SupplyDate = '{temp}');";
        DataTable dataTable = new DataTable();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = query;
        SqliteDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            { dataTable.Columns.Add(new DataColumn(reader.GetName(i))); }
        }
        else
        {
            Console.WriteLine("Error");
        }

        int j = 0;
        while (reader.Read())
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);

            for (int i = 0; i < reader.FieldCount; i++)
                dataTable.Rows[j][i] = (reader.GetValue(i));

            j++;
        }
        Console.WriteLine("ProductId\tName\t\tType\t\tCost\t\tSupplier");
        foreach (DataRow row in dataTable.Rows)
        {
            Console.WriteLine($"{row["ProductId"]}\t\t {row["Name"]}\t\t {row["Type"]}\t\t {row["Cost"]}\t\t {row["Supplier"]}");
        }
    }
}