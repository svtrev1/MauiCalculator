using System;
namespace elme
{
    public class Calculator
    {
        public string s = "";
        public int i = 0;
        public bool c = false;

        public double ProcE() // результат
        {
            s += '\0';
            double x = 0;
            try
            {
                x = ProcT();
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

            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Error in ProcE: " + ex.Message);
            }
            return x;
        }

        public double ProcT()
        {
            double x = 0;
            try
            {
                x = ProcM();
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
            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Error in ProcT: " + ex.Message);
            }
            return x;
        }

        public double ProcM()
        {
            double x = 0;
            try
            {
                if (s[i] == '(')
                {
                    i++;
                    x = ProcE();
                    if (s[i] != ')')
                    {
                        throw new Exception("Missing ')'");
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
                            throw new Exception("Syntax error");
                        }
                    }
                }

                // Проверяем на наличие двух операций подряд
                if (i > 0 && IsOperator(s[i]) && IsOperator(s[i - 1]))
                {
                    throw new Exception("Two operators in a row");
                }


            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Error in ProcM: " + ex.Message);
            }
            return x;
        }

        public double ProcC()
        {
            double x = 0;
            try
            {
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
                        x /= Math.Pow(10, j);
                }
                
            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Error in ProcC: " + ex.Message);
            }
            return x;
        }
        private bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }
    }

}

