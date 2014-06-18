using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class Links
    {
        public double[][][] x { get; set; }
        public double[][][] y { get; set; }

        public Links(double[][][] quantity, double[][][] value)
        {
            x = quantity;
            y = value;
        }

        public double Limit(int dNode, int sNode)
        {
            return x[dNode][sNode].Last();
        }

        public double LinkCostOrLoss(int dNode, int sNode, double quantity)
        {
            return Util.CalculateCost(x[dNode][sNode], y[dNode][sNode], quantity);
        }
    }
}
