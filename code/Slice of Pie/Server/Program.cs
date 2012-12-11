using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller.GetInstance().AddDocument("TestName", 136, 214, "<FlowDocument PagePadding=\"5,0,5,0\" AllowDrop=\"True\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Paragraph><Run xml:lang=\"da-dk\">Hvis du kan se det her, så virker lortet!</Run></Paragraph></FlowDocument>");
        }
    }
}