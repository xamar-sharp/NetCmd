using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class FunctionEntry : IEntry
    {
        public string CommandName { get; } = "func";
        public string HelpText { get; } = "Draws mathematic function to console(x,y).\n| is position(0,0), made for viewing of function offset.\nSyntax is 'x[+,-,/,*,%]{integer value}' !!!\nIt has 1 parameter:\n 1 - formula(syntax for function)";
        public int ParameterCount { get; } = 1;
        public ConsoleColor HelpColor { get; } = ConsoleColor.DarkGreen;
        public void React(string[] args)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(int), "x");
            ConstantExpression constant = Expression.Constant(int.Parse(args[0][2..]));
            BinaryExpression formula = null;
            switch (args[0][1])
            {
                case '+':
                    formula = Expression.Add(parameter, constant);
                    break;
                case '-':
                    formula = Expression.Subtract(parameter, constant);
                    break;
                case '/':
                    formula = Expression.Divide(parameter, constant);
                    break;
                case '*':
                    formula = Expression.Multiply(parameter, constant);
                    break;
                case '%':
                    formula = Expression.Modulo(parameter, constant);
                    break;
                default:
                    IStartup.Current.ReportError(CommandName + ":" + $" Unsupported operator {args[0][1]}");
                    StartupProgresser.Current.Notify(CommandName, 2);
                    throw new Exception();
            }
            (int, int) savedPos = (Console.CursorLeft, Console.CursorTop);
            try
            {
                Func<int, int> schema = Expression.Lambda<Func<int, int>>(formula, parameter).Compile(true);
                Console.SetCursorPosition(0, savedPos.Item2 + 70);
                Print("|");
                for (int x = 0; x < 20; x++)
                {
                    int y = schema.Invoke(x);
                    Console.SetCursorPosition(x, savedPos.Item2 + (70 - y));
                    Print("#");
                }
            }
            catch (Exception ex)
            {
            }
            Console.SetCursorPosition(0, savedPos.Item2 + 80);
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(msg);
            Console.ResetColor();
        }
    }
}
