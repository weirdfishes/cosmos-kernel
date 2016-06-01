using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Sys = Cosmos.System;
using System.Text.RegularExpressions;

namespace A1Cosmos
{
    public class Kernel : Sys.Kernel
    {
        float[] varArray = new float[20];

        List<Variable> variables = new List<Variable>();
        List<File> files = new List<File>();
        //int fileCount = 0;

        protected override void BeforeRun()
        {
            Console.WriteLine("Cosmos booted successfully.");
        }

        protected override void Run()
        {
            Console.Write("Input: ");
            string input = Console.ReadLine();
            run(input);
        }

        void run(string line)
        {

            char[] delim = { ' ' };
            string[] parse = line.Split(delim, 2);
            string cmd = parse[0];
            string[] buff;

            switch (cmd)
            {
                case "echo":
                    if (parse[1] == null)
                        Console.WriteLine("ECHO is on.");
                    else
                        echo(parse[1]);
                    break;
                case "create":
                    createFile(parse[1]);
                    break;
                case "dir":
                    dir();
                    break;
                case "set":
                    buff = parse[1].Split(' ');
                    set(buff[0], Int32.Parse(buff[1]));
                    break;
                case "add":
                    buff = parse[1].Split(' ');
                    add(buff[0], buff[1], buff[2]);
                    break;
                case "sub":
                    buff = parse[1].Split(' ');
                    sub(buff[0], buff[1], buff[2]);
                    break;
                case "mul":
                    buff = parse[1].Split(' ');
                    mul(buff[0], buff[1], buff[2]);
                    break;
                case "div":
                    buff = parse[1].Split(' ');
                    div(buff[0], buff[1], buff[2]);
                    break;
                case "exit":
                    Stop();
                    break;
                case "reboot":
                    Cosmos.System.Power.Reboot();
                    break;
                case "run":
                    runFile(parse[1]);
                    break;
                case "runall":
                    buff = parse[1].Split(' ');
                    //fileCount = buff.Length;
                    runAll(buff);
                    break;
            }
        }

        void runFile(string name)
        {
            int index = indexOf(files, name);
            File f = null;

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].name == name)
                    f = files[i];
            }

            for (int i = 0; i < f.contents.Count; i++)
            {
                Console.WriteLine(f.contents[i]);
                run(f.contents[i]);
            }

        }

        void runAll(string[] fNames)
        {
            for (int i = 0; i < fNames.Length; i++)
            {
                runFile(fNames[i]);
            }
        }

        void createFile(string filename)
        {

            File file = new File(filename);

            string line = "";

            while ((line = Console.ReadLine()) != "SAVE")
            {
                file.addLine(line);
            }

            files.Add(file);
        }

        void dir()
        {
            Console.WriteLine("Filename \tExt \t Date \t Size");
            indexOfPrint(files);
        }

        void echo(string str)
        {
            int index = indexOf(variables, str);
            if (index != -1)
            {
                Console.WriteLine(str + " = " + variables[index].data);
            }

            else
            {
                Console.WriteLine(str);
            }
        }

        void set(string name, int data)
        {
            int index = indexOf(variables, name);
            if (index == -1)
            {
                variables.Add(new Variable(name, data));
            }

            else
            {
                variables[index].data = data;
            }
        }

        void add(string s1, string s2, string s3)
        {
            if (contains(variables, s1) && contains(variables, s2))
            {
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data + variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data + variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (!contains(variables, s1) && contains(variables, s2))
            {
                int val = Int32.Parse(s1);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val + variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, val + variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (contains(variables, s1) && !contains(variables, s2))
            {
                int val = Int32.Parse(s2);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data + val;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data + val);
                    variables.Add(v);
                }


            }

            else
            {
                int val1 = Int32.Parse(s1), val2 = Int32.Parse(s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val1 + val2;
                }

                else
                {
                    Variable v = new Variable(s3, val1 + val2);
                    variables.Add(v);
                }


            }
        }


        void sub(string s1, string s2, string s3)
        {

            if (contains(variables, s1) && contains(variables, s2))
            {
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data - variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data - variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (!contains(variables, s1) && contains(variables, s2))
            {
                int val = Int32.Parse(s1);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val - variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, val - variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (contains(variables, s1) && !contains(variables, s2))
            {
                int val = Int32.Parse(s2);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data - val;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data - val);
                    variables.Add(v);
                }


            }

            else
            {
                int val1 = Int32.Parse(s1), val2 = Int32.Parse(s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val1 - val2;
                }

                else
                {
                    Variable v = new Variable(s3, val1 - val2);
                    variables.Add(v);
                }


            }
        }


        void mul(string s1, string s2, string s3)
        {

            if (contains(variables, s1) && contains(variables, s2))
            {
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data * variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data * variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (!contains(variables, s1) && contains(variables, s2))
            {
                int val = Int32.Parse(s1);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val * variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, val * variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (contains(variables, s1) && !contains(variables, s2))
            {
                int val = Int32.Parse(s2);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data * val;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data * val);
                    variables.Add(v);
                }


            }

            else
            {
                int val1 = Int32.Parse(s1), val2 = Int32.Parse(s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val1 * val2;
                }

                else
                {
                    Variable v = new Variable(s3, val1 * val2);
                    variables.Add(v);
                }


            }
        }


        void div(string s1, string s2, string s3)
        {
            //string num_regex = @"^\d+$";
            //bool x_num, y_num;
            //int x, y;

            if (contains(variables, s1) && contains(variables, s2))
            {
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data / variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data / variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (!contains(variables, s1) && contains(variables, s2))
            {
                int val = Int32.Parse(s1);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val / variables[index2].data;
                }

                else
                {
                    Variable v = new Variable(s3, val / variables[index2].data);
                    variables.Add(v);
                }


            }

            else if (contains(variables, s1) && !contains(variables, s2))
            {
                int val = Int32.Parse(s2);
                int index1 = indexOf(variables, s1), index2 = indexOf(variables, s2);
                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = variables[index1].data / val;
                }

                else
                {
                    Variable v = new Variable(s3, variables[index1].data / val);
                    variables.Add(v);
                }


            }

            else
            {
                int val1 = Int32.Parse(s1), val2 = Int32.Parse(s2);

                if (contains(variables, s3))
                {
                    int index = indexOf(variables, s3);
                    variables[index].data = val1 / val2;
                }

                else
                {
                    Variable v = new Variable(s3, val1 / val2);
                    variables.Add(v);
                }


            }
        }

        int indexOf(List<Variable> vars, string name)
        {
            for (int i = 0; i < vars.Count; i++)
            {
                if (vars[i].name == name)
                {
                    return i;
                }
            }

            return -1;
        }
        Boolean contains(List<Variable> vars, string name)
        {
            return indexOf(vars, name) != -1;
        }

        Boolean contains(List<File> vars, string name)
        {
            return indexOf(files, name) != -1;
        }

        int indexOf(List<File> files, string name)
        {
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        void indexOfPrint(List<File> files)
        {
            char[] fdelim = { '.' };
            for (int i = 0; i < files.Count; i++)
            {
                string[] fparse = files[i].name.Split(fdelim, 2);
                string tempContents = "";
                Console.Write(files[i].name + "\t" + fparse[1] + "\t Today\t" );
                for (int j = 0; j < files[i].contents.Count; j++)
                {
                    tempContents = tempContents + (files[i].contents[j]);
                }
                Console.Write((tempContents.Length * 2) + "Bytes");
                Console.WriteLine(" ");
            }
        }

        public class Variable
        {
            public string name;
            public int data;

            public Variable(string name, int data)
            {
                this.name = name;
                this.data = data;
            }

            public void setName(string name)
            {
                this.name = name;
            }

            public void setData(int data)
            {
                this.data = data;
            }

        }


        public class File
        {
            public string name;
            public List<String> contents;

            public File(string name)
            {
                this.name = name;
                contents = new List<String>();
            }

            public void setName(string name)
            {
                this.name = name;
            }

            public void addLine(string line)
            {
                contents.Add(line);
            }
        }
    }
}