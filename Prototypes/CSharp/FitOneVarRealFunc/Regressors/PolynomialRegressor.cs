using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Linear;
using FitOneVarRealFunc.Entities;

namespace FitOneVarRealFunc.Regressors
{
    public class PolynomialRegressor
    {
        private int _degree;
        PolynomialRegression _polynomialRegression;

        public PolynomialRegressor(int degree)
        {
            _degree = degree;
        }

        //string str = pr.ToString("N1"); // "y(x) = 1.0x^2 + 0.0x^1 + 0.0"

        //double[] weights = poly.Weights;
        //double intercept = poly.Intercept;

        public void Train(IList<XYItem> dsTrain)
        {
            double [] inputs = dsTrain.Select(i => i.X).ToArray();
            double [] outputs = dsTrain.Select(i => i.Y).ToArray();
            var pls = new PolynomialLeastSquares() { Degree = _degree };
            _polynomialRegression = pls.Learn(inputs, outputs);
        }
        public IEnumerable<XYItem> Predict(IEnumerable<double> xvalues)
        {
            double []xvaluesArray = xvalues.ToArray();
            double [] yvaluesArray = _polynomialRegression.Transform(xvaluesArray);
            for(int i = 0; i < xvaluesArray.Length; ++i)
            {
                yield return new XYItem() {X = xvaluesArray[i], Y = yvaluesArray[i]};
            }
        }

    }
}