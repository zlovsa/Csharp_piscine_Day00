using System;

double sum;
double rate;
int term;
int selectedMonth;
double payment;

if (args.Length < 5 || !double.TryParse(args[0], out sum) || sum < 0
    || !double.TryParse(args[1], out rate) || rate < 0
    || !int.TryParse(args[2], out term) || term < 1
    || !int.TryParse(args[3], out selectedMonth) || selectedMonth < 1 || selectedMonth + 1 > term
    || !double.TryParse(args[4], out payment) || payment < 0) {
    Console.WriteLine("Ошибка ввода. Проверьте входные данные и повторите запрос.");
    return;
}

double MonthlyPayment(double sum, double rate, int term) {
    double i = rate / 12 / 100;
    double onePlusIinPwrOfTerm = Math.Pow(1 + i, term);
    double payment = sum * i * onePlusIinPwrOfTerm / (onePlusIinPwrOfTerm - 1);
    return (payment);
}

double DaysInYear(int Year) {
    DateTime dt0 = new DateTime(Year, 1, 1, 12, 0, 0);
    DateTime dt1 = dt0.AddYears(1);
    return dt1.Subtract(dt0).TotalDays;
}

double OverPay(double sum, double rate, int term, int selectedMonth, double payment, bool decreasePayment, bool print) {
    double sumPercents = 0;
    double monthlyPayment = MonthlyPayment(sum, rate, term);
    DateTime dt0 = DateTime.Now;
    dt0 = new DateTime(dt0.Year, dt0.Month, 1, 12, 0, 0);
    int n = 1;

    if (print) {
        Console.WriteLine(decreasePayment ? "График платежей с уменьшением платежа:" : "График платежей с уменьшением срока:");
        Console.WriteLine("   Дата             Платеж                ОД          Проценты        Остаток долга");
        double totalOverpay = OverPay(sum, rate, term, selectedMonth, 0, false, false);
        Console.WriteLine($"{"",-11:d}{monthlyPayment,15:N2} р.{sum,15:N2} р.{totalOverpay,15:N2} р.{sum,18:N2} р.");
    }
    while (sum > 0) {
        DateTime dt1 = dt0.AddMonths(1);
        double deltaDays = dt1.Subtract(dt0).TotalDays;
        double percents = sum * rate / 100 * deltaDays / DaysInYear(dt0.Year);
        double debtpayment = Math.Min(monthlyPayment - percents, sum);
        sum -= debtpayment;
        sumPercents += ((sum > 0) ? percents : 0);
        if (n == selectedMonth) {
            sum -= payment;
            if (decreasePayment)
                monthlyPayment = MonthlyPayment(sum, rate, term - selectedMonth);
        }
        if (sum > 0 && print)
            Console.WriteLine($"{dt1,-11:d}{monthlyPayment,15:N2} р.{debtpayment,15:N2} р.{percents,15:N2} р.{sum,18:N2} р.");
        dt0 = dt1;
        n++;
    }
    if (print)
        Console.WriteLine();

    return (sumPercents);
}

double decreasePaymentOverpay = OverPay(sum, rate, term, selectedMonth, payment, true, true);
double decreaseTermOverpay = OverPay(sum, rate, term, selectedMonth, payment, false, true);
Console.WriteLine($"Переплата при уменьшении платежа: {decreasePaymentOverpay:N2} р.");
Console.WriteLine($"Переплата при уменьшении срока: {decreaseTermOverpay:N2} р.");
if (decreaseTermOverpay < decreasePaymentOverpay)
    Console.WriteLine($"Уменьшение срока выгоднее уменьшения платежа на {decreasePaymentOverpay - decreaseTermOverpay:N2} р.");
else if (decreaseTermOverpay > decreasePaymentOverpay)
    Console.WriteLine($"Уменьшение платежа выгоднее уменьшения срока на {decreaseTermOverpay - decreasePaymentOverpay:N2} р.");
else
    Console.WriteLine("Переплата одинакова в обоих вариантах.");
