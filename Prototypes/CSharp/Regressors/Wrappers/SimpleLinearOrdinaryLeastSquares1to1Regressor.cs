using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Models.Regression.Linear;
using Regressors.Entities;
using Regressors.Exceptions;

namespace Regressors.Wrappers
{
    public class LinearOrdinaryLeastSquares1to1Regressor
    {
        private bool _isRobust;
        private SimpleLinearRegression _simpleLinearRegression;
        public LinearOrdinaryLeastSquares1to1Regressor(bool isRobust = false)
        {
            _isRobust = isRobust;
        }

        private void EnsureAlreadyTrained()
        {
            if (_simpleLinearRegression == null)
                throw new NotTrainedException();
        }

        public void Learn(IList<XtoY> dsLearn)
        {            
            double [] inputs = dsLearn.Select(i => i.X).ToArray();
            double [] outputs = dsLearn.Select(i => i.Y).ToArray();
            var ols = new OrdinaryLeastSquares() { IsRobust = _isRobust };
            _simpleLinearRegression = ols.Learn(inputs, outputs);
        }

        public IEnumerable<XtoY> Predict(IEnumerable<double> xvalues)
        {
            EnsureAlreadyTrained();

            double []xvaluesArray = xvalues.ToArray();
            double [] yvaluesArray = _simpleLinearRegression.Transform(xvaluesArray);
            for(int i = 0; i < xvaluesArray.Length; ++i)
            {
                yield return new XtoY() {X = xvaluesArray[i], Y = yvaluesArray[i]};
            }
        }
    }
}