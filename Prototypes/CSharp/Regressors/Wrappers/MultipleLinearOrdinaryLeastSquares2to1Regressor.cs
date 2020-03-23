using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Models.Regression.Linear;
using Regressors.Entities;
using Regressors.Exceptions;

namespace Regressors.Wrappers
{
    public class MultipleLinearOrdinaryLeastSquares2to1Regressor
    {
        private bool _isRobust;
        private MultipleLinearRegression _multipleLinearRegression;
        public MultipleLinearOrdinaryLeastSquares2to1Regressor(bool isRobust = false)
        {
            _isRobust = isRobust;
        }

        private void EnsureAlreadyTrained()
        {
            if (_multipleLinearRegression == null)
                throw new NotTrainedException();
        }

        public void Learn(IList<XYtoZ> dsLearn)
        {
            double [][] inputs = dsLearn.Select(i => new double[] {i.X, i.Y}).ToArray();
            double [] outputs = dsLearn.Select(i => i.Z).ToArray();
            var ols = new OrdinaryLeastSquares() { IsRobust = _isRobust };
            _multipleLinearRegression = ols.Learn(inputs, outputs);
        }

        public IEnumerable<XYtoZ> Predict(IEnumerable<Tuple<double, double>> xvalues)
        {
            EnsureAlreadyTrained();

            double [][] xyvaluesArray = xvalues.Select(i => new double[] {i.Item1, i.Item2}).ToArray();
            double [] zvaluesArray = _multipleLinearRegression.Transform(xyvaluesArray);
            for(int i = 0; i < xyvaluesArray.Length; ++i)
            {
                yield return new XYtoZ()
                {
                    XY = new Tuple<double, double>(xyvaluesArray[i][0], xyvaluesArray[i][1]), 
                    Z = zvaluesArray[i]};
            }
        }
    }
}