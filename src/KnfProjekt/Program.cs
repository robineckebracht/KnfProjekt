using System;
using AngouriMath;
using KnfProjekt.Logic;
using KnfProjekt.Examples;

class Program
{
    static void Main()
    {
        Entity expr = SampleFormulas.Basic;

        Console.WriteLine("Input:");
        Console.WriteLine(expr);

        var knf = KnfPipeline.ToKnf(expr);

        Console.WriteLine("\nKNF:");
        Console.WriteLine(knf);
    }
}
