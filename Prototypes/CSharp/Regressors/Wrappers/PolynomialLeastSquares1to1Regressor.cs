using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Math.Optimization.Losses;
using Regressors.Entities;
using Regressors.Exceptions;

namespace Regressors.Wrappers
{
    public class PolynomialLeastSquares1to1Regressor
    {
        private int _degree;
        private bool _isRobust;
        private PolynomialRegression _polynomialRegression;

        public PolynomialLeastSquares1to1Regressor(int degree, bool isRobust = false)
        {
            _degree = degree;
            _isRobust = isRobust;
        }

        public double[] Weights
        {
            get
            {
                AssertAlreadyLearned();
                return _polynomialRegression.Weights;
            }
        }

        public double Intercept
        {
            get
            {
                AssertAlreadyLearned();
                return _polynomialRegression.Intercept;
            }
        }
        private void AssertAlreadyLearned()
        {
            if (_polynomialRegression == null)
                throw new NotTrainedException();
        }

        public string StringfyLearnedPolynomial(string format = "e")
        {
            if (_polynomialRegression == null)
                return string.Empty;
            
            return _polynomialRegression.ToString(format, CultureInfo.InvariantCulture);
        }

        public void Learn(IList<XtoY> dsLearn)
        {            
            double [] inputs = dsLearn.Select(i => i.X).ToArray();
            double [] outputs = dsLearn.Select(i => i.Y).ToArray();
            var pls = new PolynomialLeastSquares() { Degree = _degree, IsRobust = _isRobust };
            _polynomialRegression = pls.Learn(inputs, outputs);
        }

        public IEnumerable<XtoY> Predict(IEnumerable<double> xvalues)
        {
            AssertAlreadyLearned();

            double []xvaluesArray = xvalues.ToArray();
            double [] yvaluesArray = _polynomialRegression.Transform(xvaluesArray);
            for(int i = 0; i < xvaluesArray.Length; ++i)
            {
                yield return new XtoY() {X = xvaluesArray[i], Y = yvaluesArray[i]};
            }
        }

        public static double ComputeError(IEnumerable<XtoY> ds, IEnumerable<XtoY> predicted)
        {
            double [] outputs = ds.Select(i => i.Y).ToArray();
            double [] preds = predicted.Select(i => i.Y).ToArray();
            double error = new SquareLoss(outputs).Loss(preds);
            return error;
        }
    }
}