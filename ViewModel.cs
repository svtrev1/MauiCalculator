using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using Parser;


namespace elme.ViewModel;
public interface IMemory
{
    void Add(string input, string result);
    string[] Load();
}

public class DBMemory : IMemory
{
    private string input;
    public string Input
    {
        get => input;
        set
        {
            input = value;

        }
    }
    private string result;
    public string Result
    {
        get => result;
        set
        {
            result = value;

        }
    }
    
    public string connStr = "server=92.246.214.15;port=3306;user=ais_revcova1889_calculator;database=ais_revcova1889_calculator;password=RCHdGQHPOEzMz1KDirqWlTMV;";
    public void Add(string input, string result)
    {
        MySqlConnection conn = new MySqlConnection(connStr);
        conn.Open();
        string sql = "INSERT INTO Calculator(date, input, result) VALUES(CURDATE(), @input, @result)";
        MySqlCommand cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@input", input);
        cmd.Parameters.AddWithValue("@result", result);
        cmd.ExecuteScalar();
        conn.Close();
    }
    public string[] Load()
    {
        string[] arr = new string[5];
        int i = 0;
        try
        {
            string sql1 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 4,1;";
            string sql2 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 4,1;";
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            object DBResult1 = cmd1.ExecuteScalar();
            cmd1.ExecuteScalar();
            MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
            object DBInput1 = cmd2.ExecuteScalar();
            cmd2.ExecuteScalar();
            arr[i] = "     " + DBInput1.ToString() + '=' + DBResult1.ToString();
            i++;
            string sql3 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 3,1;";
            string sql4 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 3,1;";
            MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
            object DBResult2 = cmd3.ExecuteScalar();
            cmd1.ExecuteScalar();
            MySqlCommand cmd4 = new MySqlCommand(sql4, conn);
            object DBInput2 = cmd4.ExecuteScalar();
            cmd2.ExecuteScalar();
            arr[i] = "     " + DBInput2.ToString() + '=' + DBResult2.ToString();
            i++;
            string sql5 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 2,1;";
            string sql6 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 2,1;";
            MySqlCommand cmd5 = new MySqlCommand(sql5, conn);
            object DBResult3 = cmd5.ExecuteScalar();
            cmd1.ExecuteScalar();
            MySqlCommand cmd6 = new MySqlCommand(sql6, conn);
            object DBInput3 = cmd6.ExecuteScalar();
            cmd2.ExecuteScalar();
            arr[i] = "     " + DBInput3.ToString() + '=' + DBResult3.ToString();
            i++;
            string sql7 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 1,1;";
            string sql8 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 1,1;";
            MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
            object DBResult4 = cmd7.ExecuteScalar();
            cmd1.ExecuteScalar();
            MySqlCommand cmd8 = new MySqlCommand(sql8, conn);
            object DBInput4 = cmd8.ExecuteScalar();
            cmd2.ExecuteScalar();
            arr[i] = "     " + DBInput4.ToString() + '=' + DBResult4.ToString();
            i++;
            string sql9 = "SELECT result FROM Calculator ORDER BY id DESC LIMIT 1;";
            string sql10 = "SELECT input FROM Calculator ORDER BY id DESC LIMIT 1;";
            MySqlCommand cmd9 = new MySqlCommand(sql9, conn);
            object DBResult5 = cmd9.ExecuteScalar();
            cmd1.ExecuteScalar();
            MySqlCommand cmd10 = new MySqlCommand(sql10, conn);
            object DBInput5 = cmd10.ExecuteScalar();
            cmd2.ExecuteScalar();
            arr[i] = "     " + DBInput5.ToString() + '=' + DBResult5.ToString();
        }
        catch (Exception)
        {

        }
        return arr;
        
    }

}

class FileMemory : IMemory
{
    string filePath = "/Users/svtrev/Desktop/file/fileTRY.txt";
    public void Add(string input, string result)
    {
        
        if (File.Exists(filePath))
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.WriteLine(input + '=' + result);
            }
        }
        else
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                writer.WriteLine(input + '=' + result);
            }
        }
    }
    public string[] Load()
    {
        int i = 0;
        string[] arr = new string[5];
        if (File.Exists(filePath))
        {
            var lastLines = File.ReadLines(filePath).Reverse().Take(5);
            foreach (var line in lastLines.Reverse())
            {
          
                arr[i] = "     " + line;
                i++;
            }
        }
       

        return arr;
    }
}

class RamMemory : IMemory
{
    public void Add(string input, string result) { }
    public string[] Load() { string[] arr = new string[10]; return arr; }
}

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
    private readonly IMemory _memory;


    public ViewModel(IMemory memory)
    {

        _memory = memory;
        
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
        string[] arr = new string [5];
        arr = memory.Load();
        try
        {
            if (arr[0] != null)
                AddToHistory(arr[0]);
            if (arr[1] != null)
                AddToHistory(arr[1]);
            if (arr[2] != null)
                AddToHistory(arr[2]);
            if (arr[3] != null)
                AddToHistory(arr[3]);
            if (arr[4] != null)
                AddToHistory(arr[4]);
            
        }
        catch (Exception)
        { }
        
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
               
                _memory.Add(temp, InputText);
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
                       
                        _memory.Add(calculator.s, temp);
                        AddToHistory("     " + calculator.s + '=' + InputText);
                    }
                    else
                    {
                        InputText = temp;
                        
                        _memory.Add(calculator.s, temp);
                        AddToHistory("     " + calculator.s + '=' + InputText);
                    }
                }
                catch (Exception ex)
                {
                    InputText = "Error: " + ex.Message;
                }
            }
        }
    }


    public void AddToHistory(string calculation)
    {
        
        CalculationHistory.Add(calculation);

        if (CalculationHistory.Count > 5)
        {
            CalculationHistory.RemoveAt(0);
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
}
