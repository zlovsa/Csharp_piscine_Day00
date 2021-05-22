using System;

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

double TotalPay(double sum, double rate, int term, int selectedMonth, double payment, bool decreasePayment) {
    double sumPayments = 0;
    double monthlyPayment = MonthlyPayment(sum, rate, term);
    DateTime dt0 = DateTime.Now;
    dt0 = new DateTime(dt0.Year, dt0.Month, 1, 12, 0, 0);
    int n = 1;

    while (sum > 0) {
        DateTime dt1 = dt0.AddMonths(1);
        double deltaDays = dt1.Subtract(dt0).TotalDays;
        double percents = sum * rate / 100 * deltaDays / DaysInYear(dt0.Year);
        double debtpayment = Math.Min(monthlyPayment - percents, sum);
        sum -= debtpayment;
        sumPayments += debtpayment + ((sum > 0) ? percents : 0);
        if (n == selectedMonth) {
            sum -= payment;
            sumPayments += payment;
            if (decreasePayment)
                monthlyPayment = MonthlyPayment(sum, rate, term - selectedMonth);
        }
        dt0 = dt1;
        n++;
    }
    
    return (sumPayments);
}

double decreasePaymentOverpay = TotalPay(sum, rate, term, selectedMonth, payment, true) - sum;
double decreaseTermOverpay = TotalPay(sum, rate, term, selectedMonth, payment, false) - sum;
Console.WriteLine("Переплата при уменьшении платежа: {0:N2}р.", decreasePaymentOverpay);
Console.WriteLine("Переплата при уменьшении срока: {0:N2}р.", decreaseTermOverpay);
if (decreaseTermOverpay < decreasePaymentOverpay)
    Console.WriteLine("Уменьшение срока выгоднее уменьшения платежа на {0:N2}р.", decreasePaymentOverpay - decreaseTermOverpay);
else if (decreaseTermOverpay > decreasePaymentOverpay)
    Console.WriteLine("Уменьшение платежа выгоднее уменьшения срока на {0:N2}р.", decreaseTermOverpay - decreasePaymentOverpay);
else
    Console.WriteLine("Переплата одинакова в обоих вариантах.");
