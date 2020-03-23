using System;
using System.Collections.Generic;
using Regressors.Entities;

namespace Regressors.DataSets
{
    public static class TwoVarsRealFuncSynthDataSetGenerator
    {
        public static IEnumerable<XYtoZ> GenerateEllipticParaboloidDS(
            double xbegin, double xend, 
            double ybegin, double yend, 
            double step)
        {
            for(double x = xbegin; x <= xend; x += step)
            {
                for(double y = ybegin; y <= yend; y += step)
                {
                    double z = x*x + y*y;
                    yield return new XYtoZ() {XY = new Tuple<double, double>(x, y), Z=z};
                }
            }
        }

        public static IEnumerable<XYtoZ> GenerateHyperbolicParaboloidDS(
            double xbegin, double xend, 
            double ybegin, double yend, 
            double step)
        {
            for(double x = xbegin; x <= xend; x += step)
            {
                for(double y = ybegin; y <= yend; y += step)
                {
                    double z = x*x - y*y;
                    yield return new XYtoZ() {XY = new Tuple<double, double>(x, y), Z=z};
                }
            }
        }
        
        public static IEnumerable<XYtoZ> GenerateArcTanDS(
            double xbegin, double xend, 
            double ybegin, double yend, 
            double step)
        {
            for(double x = xbegin; x <= xend; x += step)
            {
                for(double y = ybegin; y <= yend; y += step)
                {
                    double z = Math.Atan(x+y);
                    yield return new XYtoZ() {XY = new Tuple<double, double>(x, y), Z=z};
                }
            }
        }
    }
}