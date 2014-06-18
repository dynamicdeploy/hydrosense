using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydroSense
{
    class LinkLoss : Link
    {
        public LinkLoss(double[][][] quantity, double[][][] value)
            : base(quantity, value)
        {

        }

        public double LossInverse(int dNode, int sNode, double quantity)
        {
            return Util.CalculateCost(y[dNode][sNode], x[dNode][sNode], quantity);
        }
    }
}
