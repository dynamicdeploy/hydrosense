using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class Links
    {
        private double[][][] X;
        private double[][][] Y;
        public double[][][] x { get { return X; } }
        public double[][][] y { get { return Y; } }

        public Links(double[][][] quantity, double[][][] value)
        {
            X = quantity;
            Y = value;
        }

        public double UpperLimit(int dNode, int sNode)
        {
            return x[dNode][sNode].Last();
        }

        public double LowerLimit(int dNode, int sNode)
        {
            return x[dNode][sNode].First();
        }

        public double LinkCostOrLoss(int dNode, int sNode, double quantity)
        {
            return Util.CalculateCost(x[dNode][sNode], y[dNode][sNode], quantity);
        }
    }
}
