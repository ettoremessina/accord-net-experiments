using System;
using System.Collections.Generic;
using Regressors.Entities;

namespace Regressors.DataSets
{
    public static class OneVarRealFuncSynthDataSetGenerator
    {
        public static IEnumerable<XtoY> GeneratePolynomialDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = 0.5 * Math.Pow(x, 3) - 2 * Math.Pow(x, 2) - 3 * x - 1;
                yield return new XtoY() {X=x, Y=y};
            }            
        }
        public static IEnumerable<XtoY> GenerateSineDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Sin(x);
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateExponentialDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Exp(x);
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateSqrtAbsDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Sqrt(Math.Abs(x));
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateLog1PlusAbsDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Log(1 + Math.Abs(x));
                yield return new XtoY() {X=x, Y=y};
            }
        }        

        public static IEnumerable<XtoY> GenerateArcTanDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Atan(x);
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateExponentialSineDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Exp(Math.Sin(x));
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateTanhDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Tanh(x);
                yield return new XtoY() {X=x, Y=y};
            }
        }

        public static IEnumerable<XtoY> GenerateMuffleWavedDS(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Sin(2 * x) / Math.Exp(x / 5.0);
                yield return new XtoY() {X=x, Y=y};
            }
        }
    }
}