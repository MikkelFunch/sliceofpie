﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            String email = "heyho";
            Console.Out.WriteLine(Controller.GetInstance().GetUser(email).email);
            Console.In.ReadLine();
        }
    }
}