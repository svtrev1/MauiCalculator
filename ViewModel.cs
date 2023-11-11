using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
namespace elme.ViewModel;

public class ViewModel : INotifyPropertyChanged
{
    private string _inputText;
    public ICommand AppendCommand { get; }
    public ICommand CalculateCommand { get; }
    public ObservableCollection<string> CalculationHistory { get; } 
    public ICommand ClearCommand { get; }
    public ICommand DeleteCommand { get; }
    public bool check;
    public bool c1;

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
    }

    public string InputText
    {
        get => _inputText;
        set
        {
            _inputText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InputText)));
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
        InputText = "";
    }

    private void Delete()
    {
        InputText = InputText.Remove(InputText.Length - 1);
    }

    private void Calculate(string value)
    {
        check = false;
        string temp;
        temp = InputText;
        if (double.TryParse(InputText, out _))
        {
            c1 = true;
            InputText = "Ошибка";
            AddToHistory(temp + '=' + InputText);
        }
        else
        {
            try
            {
                var calculator = new Calculator();
                calculator.s = InputText;
                temp = calculator.ProcE().ToString();
                c1 = calculator.c;
                if (calculator.c)
                {
                    InputText = "Ошибка";
                    AddToHistory(calculator.s + '=' + InputText);
                }
                else
                {
                    InputText = temp;
                    AddToHistory(calculator.s + '=' + temp);
                }
            }
            catch (Exception ex)
            {
                InputText = "Error: " + ex.Message;
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
