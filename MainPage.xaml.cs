namespace elme;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        
    }

    Calculator _calculator = new Calculator();

    void Input_Click(object sender, System.EventArgs e)
    {
        MainLabel.Text += (sender as Button).Text;
    }

    void Delete_Click(object sender, System.EventArgs e)
    {
        MainLabel.Text = "";
    }

    void Result_Click(object sender, System.EventArgs e)
    {
        _calculator.s = MainLabel.Text;
        _calculator.i = 0;
        double result = _calculator.ProcE();
        MainLabel.Text = result.ToString();
    }

    class Calculator
    {
        public double c = 0;
        public string s = "5+2";
        public int i = 0;

        public double ProcE() //результат
        {
            s += '\0';
            double x = ProcT();
            while (s[i] == '+' || s[i] == '-')
            {
                char p = s[i];
                i++;
                if (p == '+')
                {
                    x += ProcT();
                }
                else
                {
                    x -= ProcT();
                }
            }
            return x;
        }

        public double ProcT()
        {
            double x = ProcM();
            while (s[i] == '*' || s[i] == '/')
            {
                char p = s[i];
                i++;
                if (p == '*')
                {
                    x *= ProcM();
                }
                else
                {
                    x /= ProcM();
                }
            }
            return x;
        }

        public double ProcM()
        {
            double x = 0;
            if (s[i] == '(')
            {
                i++;
                x = ProcE();
                if (s[i] != ')')
                {
                    Console.WriteLine("missing ')'");
                }
                i++;
            }
            else
            {
                if (s[i] == '-')
                {
                    i++;
                    x -= ProcM();
                }
                else
                {
                    if (s[i] >= '0' && s[i] <= '9')
                    {
                        x = ProcC();
                    }
                    else
                    {
                        Console.WriteLine("Syntex error.");
                    }
                }
            }
            return x;
        }

        public double ProcC()
        {
            double x = 0;
            while (s[i] >= '0' && s[i] <= '9')
            {
                x *= 10;
                x += s[i] - '0';
                i++;
            }
            int j = 0;
            if (s[i] == ',')
            {
                i++;
                while (s[i] >= '0' && s[i] <= '9')
                {
                    x *= 10;
                    x += s[i] - '0';
                    i++;
                    j++;
                }
                if (j != 0)
                    x /= Math.Pow(10,j);
            }
            return x;
        }
    }
}


