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
            while (quantities[i] <= quantity && i < quantities.Length - 1)
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
    }
}
