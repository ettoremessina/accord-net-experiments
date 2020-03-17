using System;
using System.Collections.Generic;
using FitOneVarRealFunc.Entities;

namespace FitOneVarRealFunc.DataSets
{
    public static class OneVarRealFuncSynthDataSetGenerator
    {
        public static IEnumerable<XYItem> GenerateSineCurve(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Sin(x);
                yield return new XYItem() {X=x, Y=y};
            }
        }

        public static IEnumerable<XYItem> GenerateExponentialCurve(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Exp(x);
                yield return new XYItem() {X=x, Y=y};
            }
        }

        public static IEnumerable<XYItem> GenerateSqrtAbsCurve(double begin, double end, double step)
        {
            for(double x = begin; x <= end; x += step)
            {
                double y = Math.Sqrt(Math.Abs(x));
                yield return new XYItem() {X=x, Y=y};
            }
        }        
    }
}