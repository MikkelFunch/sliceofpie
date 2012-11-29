using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slice_of_Pie
{
    class Program
    {
        static void Main(string[] args)
        {
            //Remember dem comments!
            Console.Out.WriteLine("Works bitches");
            string[] original = { "A", "B", "C", "D" };
            string[] latest = { "A", "B", "C", "D", "E" };
            string[] actual = Model.GetInstance().MergeDocuments(original, latest);

            foreach (string s in actual)
            {
                Console.Out.WriteLine(s);
            }

            Console.In.ReadLine();

        }
    }
}
