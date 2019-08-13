using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConvertationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TextDecorationCollection decorations = new TextDecorationCollection();
            decorations.Add(TextDecorations.Baseline);
            decorations.Add(TextDecorations.Underline);

            TextDecorationCollectionConverter converter = new TextDecorationCollectionConverter();

            string[] strDecorations = new string[decorations.Count];

            for (int i = 0; i < decorations.Count; i++)
            {
                strDecorations[i] = decorations[i].Location.ToString();
            }

            TextDecorationCollection readDecorations = new TextDecorationCollection();

            for (int i = 0; i < strDecorations.Length; i++)
            {
                readDecorations.Add(TextDecorationCollectionConverter.ConvertFromString(strDecorations[i]));
            }

            Console.ReadKey();
        }
    }
}