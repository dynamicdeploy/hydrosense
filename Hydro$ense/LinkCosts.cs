using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class LinkCosts : Links
    {
        public double[][][] yc { get; set; }

        public LinkCosts(double[][][] quantity, double[][][] value)
            : base(quantity, value)
        {
            yc = new double[x.Length][][];

            double val;
            for (int i = 0; i < yc.Length; i++)
            {
                yc[i] = new double[x[i].Length][];
                for (int j = 0; j < yc[i].Length; j++)
                {
                    val = 0.0;
                    yc[i][j] = new double[x[i][j].Length];
                    for (int k = 0; k < yc[i][j].Length; k++)
                    {
                        if (k > 0)
                        {
                            val += ((y[i][j][k] + y[i][j][k - 1]) / 2.0) * (x[i][j][k] - x[i][j][k - 1]);
                        }
                        yc[i][j][k] = val;
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
