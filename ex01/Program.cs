using System;

string[] namevoc;
try {
    namevoc = System.IO.File.ReadAllLines("us.txt");
} catch {
    Console.WriteLine("Error reading us.txt.");
    return;
}

Console.WriteLine(">Enter name:");
string name = Console.ReadLine();
if (name == "") {
    Console.WriteLine("Your name was not found.");
    return;
}

bool ValidChar(char c) {
    return (char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
}

foreach (char c in name)
    if (!ValidChar(c)) {
        Console.WriteLine("Name may contain letters, whitespaces and hyphens.");
        return;
    }

foreach (string vname in namevoc)
    if (name == vname) {
        Console.WriteLine($"Hello, {name}!");
        return;
    }

int Levenstein(string a, string b, int curr) {
    if (curr >= 3)
        return curr;
    if (a.Length == 0)
        return b.Length;
    if (b.Length == 0)
        return a.Length;
    if (a[0] == b[0])
        return Levenstein(a.Substring(1), b.Substring(1), curr);
    return 1 + Math.Min(Levenstein(a.Substring(1), b, curr + 1),
         Math.Min(Levenstein(a, b.Substring(1), curr + 1),
         Levenstein(a.Substring(1), b.Substring(1), curr + 1)));
}

int[] lev = new int[namevoc.Length];
int l = 1;
while (l < 3) {
    int i = 0;
    while (i < namevoc.Length) {
        if (lev[i] == 0)
            lev[i] = Levenstein(name, namevoc[i], 0);
        if (lev[i] == l) {
            Console.WriteLine($">Did you mean “{namevoc[i]}”? Y/N");
            string answer = Console.ReadLine();
            if (answer.ToUpper() == "Y") {
                Console.WriteLine($"Hello, {namevoc[i]}!");
                return;
            }
        }
        i++;
    }
    l++;
}

Console.WriteLine("Your name was not found.");