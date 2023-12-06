using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.IO;



namespace elme.ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    private string _inputText;
    private double _fontSize;
    public ICommand AppendCommand { get; }
    public ICommand CalculateCommand { get; }
    public ObservableCollection<string> CalculationHistory { get; }
    public ICommand ClearCommand { get; }
    public ICommand DeleteCommand { get; }
    public bool check;
    public bool c1;
    private int sizeT;
    private int sizeF;
    public string inputDB;
    public string resultDB;
    string connStr = "server=92.246.214.15;port=3306;user=ais_revcova1889_calculator;database=ais_revcova1889_calculator;password=RCHdGQHPOEzMz1KDirqWlTMV;";
    string filePath = "/Users/svtrev/Desktop/file/fileTRY.txt";


    public ViewModel()
    {
        
        c1 = false;
        check = true;
        InputText = "";
        CalculationHistory = new ObservableCollection<string>();
        AppendCommand = new Command<string>(Append);
        CalculateCommand = new Command<string>(Calculate);
        ClearCommand = new Command(Clear);
        DeleteCommand = new Command(Delete);
        FontSize = 80;
        sizeT = 13;
        sizeF = 60;
        firstDB();
        


    }

    public void firstDB()
    { 
        string sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 4,1;";
        string sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 4,1;";
        loadDB(sql1, sql2);
        sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 3,1;";
        sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 3,1;";
        loadDB(sql1, sql2);
        sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 2,1;";
        sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 2,1;";
        loadDB(sql1, sql2);
        sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 1,1;";
        sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 1,1;";
        loadDB(sql1, sql2);
        sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 1;";
        sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 1;";
        loadDB(sql1, sql2);

    }

    private void loadDB(string sql1, string sql2)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        conn.Open();
        MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
        object resultDB = cmd1.ExecuteScalar();
        cmd1.ExecuteScalar();
        MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
        object inputDB = cmd2.ExecuteScalar();
        cmd2.ExecuteScalar();
        AddToHistory("     " + inputDB + '=' + resultDB);
    }

    private void UpdateFontSize()
    {
        if (InputText.Length > sizeT)
        {
            FontSize = sizeF;
            sizeT += 4;
            sizeF = 1040 / (sizeT+4) - 1;
        }
    }

    public double FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
        }
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputText)));
            UpdateFontSize();
        }
    }

    private void Append(string value)
    {
        
        if (c1)
        {
            InputText = value;
            check = true;
            c1 = false;
        }
        else
        {
            if (check)
                
                InputText += value;
            else
            {
                string temp = value;
                if (int.TryParse(temp, out _))
                {
                    InputText = value;
                    check = true;
                }
                else
                {
                    InputText += value;
                    check = true;
                }
            }
        }
    }

    private void Clear()
    {
        FontSize = 80;
        sizeT = 13;
        sizeF = 60;
        InputText = "";
    }

    private void Delete()
    {
        InputText = InputText.Remove(InputText.Length - 1);
    }

    private void Calculate(string value)
    {
        FontSize = 80;
        sizeT = 13;
        sizeF = 60;
        if (InputText != "" && InputText != "Ошибка")
        {
            check = false;
            string temp;
            temp = InputText;
            if (double.TryParse(InputText, out _))
            {
                c1 = true;
                InputText = "Ошибка";
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                string sql = "INSERT INTO Calculator(isError, date, input, result) VALUES(1, CURDATE(), @input, 'Ошибка')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@input", temp);
                cmd.ExecuteScalar();
                AddToFile(temp + '=' + InputText);
                AddToHistory("     " + temp + '=' + InputText);
            }
            else
            {
                try
                {
                    var calculator = new Calculator();
                    calculator.s = InputText;
                    temp = calculator.Cal().ToString();
                    c1 = calculator.c;
                    if (calculator.c)
                    {
                        InputText = "Ошибка";
                        MySqlConnection conn = new MySqlConnection(connStr);
                        conn.Open();
                        string sql = "INSERT INTO Calculator(isError, date, input, result) VALUES(1, CURDATE(), @input, 'Ошибка')";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@input", calculator.s);
                        cmd.ExecuteScalar();
                        AddToFile(calculator.s + '=' + InputText);
                        AddToHistory("     " + calculator.s + '=' + InputText);
                    }
                    else
                    {
                        InputText = temp;
                        MySqlConnection conn = new MySqlConnection(connStr);
                        conn.Open();
                        string sql = "INSERT INTO Calculator(isError, date, input, result) VALUES(0, CURDATE(), @input, @result)";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@input", calculator.s);
                        cmd.Parameters.AddWithValue("@result", temp);
                        cmd.ExecuteScalar();
                        AddToFile(calculator.s + '=' + temp + "  ");
                        AddToHistory("     " + calculator.s + '=' + temp);
                    }
                }
                catch (Exception ex)
                {
                    InputText = "Error: " + ex.Message;
                }
            }
        }
    }

    private void AddToFile(string calculation)
    {
        if (File.Exists(filePath))
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine(calculation);
            }
        }
        else
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine(calculation);
            }
        }
    }

    private void AddToHistory(string calculation)
    {
        
        CalculationHistory.Add(calculation);

        if (CalculationHistory.Count > 5)
        {
            CalculationHistory.RemoveAt(0);
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
