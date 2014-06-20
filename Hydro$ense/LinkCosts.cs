using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class LinkCosts : Links
    {
        private double[][][] YC;
        public double[][][] yc { get { return YC; } }

        public LinkCosts(double[][][] quantity, double[][][] value)
            : base(quantity, value)
        {
            YC = new double[x.Length][][];

            double val;
            for (int i = 0; i < YC.Length; i++)
            {
                YC[i] = new double[x[i].Length][];
                for (int j = 0; j < YC[i].Length; j++)
                {
                    val = 0.0;
                    YC[i][j] = new double[x[i][j].Length];
                    for (int k = 0; k < YC[i][j].Length; k++)
                    {
                        if (k > 0)
                        {
                            val += ((y[i][j][k] + y[i][j][k - 1]) / 2.0) * (x[i][j][k] - x[i][j][k - 1]);
                        }
                        YC[i][j][k] = val;
                    }
                }
            }
        }

        public double IntegratedCost(int dNode, int sNode, double quantity)
        {
            return Util.CalculateCost(x[dNode][sNode], yc[dNode][sNode], quantity);
        }

    }
}
