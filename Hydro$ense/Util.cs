using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    static class Util
    {
        public static double CalculateCost(double[] quantities, double[] costs, double quantity)
        {
            if (quantity < 0)
                return 0.0;

            int i = 0;
            while (quantities[i] < quantity && i < quantities.Length - 1)
            {
                i++;
            }

            // if first value in quantities is greater than quantity, use the
            // first two quantities and costs. ASSUMES at least two values.
            if (i == 0)
                i++;

            // don't divide by zero
            if (quantities[i] == 0 && quantities[i - 1] == 0)
                return 0.0;

            double slope = (costs[i] - costs[i - 1]) / (quantities[i] - quantities[i - 1]);
            return costs[i - 1] + (quantity - quantities[i - 1]) * slope;
        }

        public static void PrintMatrixToConsole(double[][] quant)
        {
            for (int i = 0; i < quant.Length; i++)
            {
                Console.Write("[");
                for (int j = 0; j < quant[i].Length; j++)
                {
                    Console.Write(quant[i][j]);
                    if (j < quant[i].Length - 1)
                        Console.Write(", ");
                }
                Console.Write("]");
                Console.WriteLine();
            }
        }

        public static void PrintArrayToConsole(double[][] quant)
        {
            Console.Write("[");
            for (int i = 0; i < quant.Length; i++)
            {
                for (int j = 0; j < quant[i].Length; j++)
                {
                    Console.Write(quant[i][j]);
                    if (i != quant.Length - 1 || j != quant[i].Length - 1)
                        Console.Write(", ");
                }
            }
            Console.WriteLine("]");
        }

        // taken from here:
        // http://stackoverflow.com/questions/4670720/extremely-fast-way-to-clone-the-values-of-a-jagged-array-into-a-second-array
        public static double[][] CopyArray(double[][] source)
        {
            double[][] rval = new double[source.Length][];

            for (int i = 0; i < source.Length; i++)
            {
                double[] row = new double[source[i].Length];

                Array.Copy(source[i], row, source[i].Length);
                rval[i] = row;
            }

            return rval;

        }

        public static double SumColumn(double[][] Q, int col)
        {
            double rval = 0.0;
            for (int i = 0; i < Q.Length; i++)
            {
                rval += Q[i][col];
            }
            return rval;
        }

        public static double SumRow(double[][] Q, int row)
        {
            double rval = 0.0;
            for (int i = 0; i < Q[row].Length; i++)
            {
                rval += Q[row][i];
            }
            return rval;
        }

        public static void InitializeArrayToZero(double[][] arr, int numRows, int numColumns)
        {
            for (int i = 0; i < numRows; i++)
            {
                arr[i] = new double[numColumns];
                for (int j = 0; j < numColumns; j++)
                {
                    arr[i][j] = 0.0;
                }
            }
        }

        public static void PrintResultsToConsole(double OF, double[][] quantS, double[][] quantD, int iter, double delta)
        {
            Console.WriteLine(string.Format("k = {0}, Delta = {1}, OF = {2}", iter, delta, OF));
            Console.Write("Supply = ");
            Util.PrintArrayToConsole(quantS);
            Console.Write("Demand = ");
            Util.PrintArrayToConsole(quantD);
        }
    }
}
