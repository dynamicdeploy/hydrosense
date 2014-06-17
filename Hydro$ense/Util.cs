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
            int k = 0;
            for (int i = 0; i < quantities.Length; i++)
            {
                if (quantities[i] <= quantity)
                {
                    k += 1;
                }
            }

            double slope = (costs[k] - costs[k - 1]) / (quantities[k] - quantities[k - 1]);
            return costs[k - 1] + (quantity - quantities[k - 1]) * slope;
        }
    }
}
