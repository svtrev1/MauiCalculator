using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;


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
                        AddToHistory("     " + calculator.s + '=' + InputText);
                    }
                    else
                    {
                        InputText = temp;
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
