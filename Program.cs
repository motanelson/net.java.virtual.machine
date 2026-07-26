
using System;
using System.IO;

class MiniJVM
{
    static int[] stack = new int[256];
    static int[] locals_ = new int[256];

    static int sp = 0;


    static void Push(int x)
    {
        stack[sp++] = x;
    }


    static int Pop()
    {
        sp--;
        return stack[sp];
    }


    static void IAdd()
    {
        int b = Pop();
        int a = Pop();

        Push(a + b);
    }


    static void ISub()
    {
        int b = Pop();
        int a = Pop();

        Push(a - b);
    }


    static void IMul()
    {
        int b = Pop();
        int a = Pop();

        Push(a * b);
    }


    static void IDiv()
    {
        int b = Pop();
        int a = Pop();

        Push(a / b);
    }


    static void InvokeVirtual(string line)
    {
        if (line.Contains("PrintStream.println"))
        {
            Console.WriteLine(Pop());
        }
    }


    static void Execute(string[] code)
    {
        foreach (string s in code)
        {
            string line = s.Trim();

            line = line.Replace(";", "");


            //----------------------------------

            if (line.Contains("iconst_0"))
            {
                Push(0);
            }

            else if (line.Contains("iconst_1"))
            {
                Push(1);
            }

            else if (line.Contains("iconst_2"))
            {
                Push(2);
            }

            else if (line.Contains("iconst_3"))
            {
                Push(3);
            }

            else if (line.Contains("iconst_4"))
            {
                Push(4);
            }

            else if (line.Contains("iconst_5"))
            {
                Push(5);
            }

            //----------------------------------

            else if (line.Contains("bipush"))
            {
                int p = line.IndexOf("bipush");

                string value =
                    line.Substring(p + 7).Trim();

                Push(Int32.Parse(value));
            }

            //----------------------------------

            else if (line.Contains("iload_"))
            {
                int p = line.IndexOf("iload_");

                string value =
                    line.Substring(p + 6).Trim();

                int n = Int32.Parse(value);

                Push(locals_[n]);
            }

            //----------------------------------

            else if (line.Contains("istore_"))
            {
                int p = line.IndexOf("istore_");

                string value =
                    line.Substring(p + 7).Trim();

                int n = Int32.Parse(value);

                locals_[n] = Pop();
            }

            //----------------------------------

            else if (line.Contains("iadd"))
            {
                IAdd();
            }

            else if (line.Contains("isub"))
            {
                ISub();
            }

            else if (line.Contains("imul"))
            {
                IMul();
            }

            else if (line.Contains("idiv"))
            {
                IDiv();
            }

            //----------------------------------

            else if (line.Contains("invokevirtual"))
            {
                InvokeVirtual(line);
            }

            //----------------------------------

            else if (line.Contains("return"))
            {
                Console.WriteLine();
                Console.WriteLine("program finished.");
                return;
            }

        }
    }



    static string[] LoadMain(string file)
    {
        string[] body = File.ReadAllLines(file);

        string[] code = new string[1024];

        bool inside = false;

        int count = 0;

        foreach (string s in body)
        {
            string line = s.Trim();

            if (line.Contains("main"))
            {
                inside = true;
                continue;
            }


            if (inside)
            {
                if (line == "}")
                    break;

                if (line.Contains("{"))
                    continue;

                if (line.Contains("stack"))
                    continue;

                if (line.Length < 2)
                    continue;


                code[count++] = line;
            }
        }


        string[] result = new string[count];

        for (int i = 0; i < count; i++)
            result[i] = code[i];


        return result;
    }



    static void Main()
    {
        Console.Clear();
        Console.BackgroundColor=ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.WriteLine("give me file .jasm ?");
        Console.WriteLine();

        string file = Console.ReadLine();

        if (!File.Exists(file))
        {
            Console.WriteLine("file not found");
            return;
        }


        string[] code = LoadMain(file);


        Console.WriteLine();
        Console.WriteLine("----- EXECUTION -----");
        Console.WriteLine();


        Execute(code);
    }
}