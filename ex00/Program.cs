using System;

namespace ex00
{
    class Program
    {
        static void Main(string[] args) {
            double sum;
            double rate;
            int term;
            int selectedMonth;
            double payment;

            if (args.Length < 5 || !double.TryParse(args[0], out sum) || sum < 0
                || !double.TryParse(args[1], out rate) || rate < 0
                || !int.TryParse(args[2], out term) || term < 1
                || !int.TryParse(args[3], out selectedMonth) || selectedMonth < 0
                || !double.TryParse(args[4], out payment) || payment < 0) {
                Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
                return;
            }

        }
    }
}
