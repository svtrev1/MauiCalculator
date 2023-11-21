using System;
namespace elme
{
    public class Calculator
    {
        public string s = "";
        public int i = 0;
        public bool c = false;

        public double Cal()
        {
            i = 0;
            s = s.Trim();  // удаляем пробелы в начале и в конце строки
            

            // начинаем парсить выражение с проверкой на корректную структуру
            try
            {
                int openBrackets = 0;
                int closeBrackets = 0;

                foreach (char ch in s)
                {
                    if (ch == '(')
                    {
                        openBrackets++;
                    }
                    else if (ch == ')')
                    {
                        closeBrackets++;
                    }
                    if (closeBrackets > openBrackets)
                    {
                        throw new Exception("Unbalanced brackets: Closing bracket without corresponding opening bracket");
                    }
                }

                // проверяем, чтобы количество открывающих и закрывающих скобок совпадало
                if (openBrackets != closeBrackets)
                {
                    throw new Exception("Unbalanced brackets: Number of opening and closing brackets is not equal");
                }

                double result = ProcE();

                // проверяем, были ли ошибки во время парсинга
                if (c)
                {
                    throw new Exception("An error occurred during parsing");
                }

                return Math.Round(result, 10);
            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Calculation error: " + ex.Message);
                return double.NaN;  // возвращаем NaN в случае ошибки
            }
           
        }

        public double ProcE()
        {
            s += '\0';
            double x = 0;
            try
            {
                if (s[i] == ')')
                {
                    throw new Exception("Unbalanced brackets: Unexpected closing bracket");
                }

                x = ProcT();
                if (s[i] == ',')
                    throw new Exception("Using , without number");
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

                if (i < s.Length && s[i] == '(')
                {
                    throw new Exception("Unbalanced brackets: Unexpected opening bracket");
                }
            }
            catch (Exception ex)
            {
                c = true;
                Console.WriteLine("Error in ProcE: " + ex.Message);
            }
            return Math.Round(x, 10);
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

