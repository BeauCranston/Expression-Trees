using System;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Expression_Trees
{
    class Program
    {
        static void Main(string[] args)
        {
            RpnExpressionVisitor visitor = new RpnExpressionVisitor();
            string input = Console.ReadLine();
            
            visitor.ReadString(input);

            Console.WriteLine(visitor.GetResult());

        }


    }
}


