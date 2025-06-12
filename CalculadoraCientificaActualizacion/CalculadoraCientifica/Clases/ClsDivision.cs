using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculadoraCientifica.Clases
{
    internal class ClsDivision
    {
        public double Dividir(double n1, double n2)
        {
            if (n2 == 0)
            {
                throw new DivideByZeroException("Error: División por cero no permitida.");
            }

            return n1 / n2;
        }
    }

}
