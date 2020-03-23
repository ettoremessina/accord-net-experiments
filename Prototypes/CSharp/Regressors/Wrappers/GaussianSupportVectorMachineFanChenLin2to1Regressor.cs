using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Regressors.Entities;
using Regressors.Exceptions;

namespace Regressors.Wrappers
{
    public class GaussianSupportVectorMachineFanChenLin2to1Regressor
    {
        private double _tolerance;
        private bool _useKernelEstimation; 
        private bool _useComplexityHeuristic;
        private double _complexity;
        private SupportVectorMachine<Gaussian> _supportVectorMachine;

        public GaussianSupportVectorMachineFanChenLin2to1Regressor(
            double tolerance = 1e-5, bool useKernelEstimation = true, bool useComplexityHeuristic = true, double complexity = 10000.0)
        {
            _tolerance = tolerance;
            _useKernelEstimation = useKernelEstimation;
            _useComplexityHeuristic = useComplexityHeuristic;
            _complexity = complexity;
        }

        private void EnsureAlreadyTrained()
        {
            if (_supportVectorMachine == null)
                throw new NotTrainedException();
        }

        public void Learn(IList<XYtoZ> dsLearn)
        {            
            double [][] inputs = dsLearn.Select(i => new double[] {i.X, i.Y}).ToArray();
            double [] outputs = dsLearn.Select(i => i.Z).ToArray();

            var fclsvr = new FanChenLinSupportVectorRegression<Gaussian>()
            {
                Tolerance = _tolerance,
                UseKernelEstimation = _useKernelEstimation, 
                UseComplexityHeuristic = _useComplexityHeuristic,
                Complexity = _complexity,
                Kernel = new Gaussian()
            };

            _supportVectorMachine = fclsvr.Learn(inputs, outputs);
        }

        public IEnumerable<XYtoZ> Predict(IEnumerable<Tuple<double, double>> xvalues)
        {
            EnsureAlreadyTrained();

            double [][] xyvaluesArray = xvalues.Select(i => new double[] {i.Item1, i.Item2}).ToArray();
            double [] zvaluesArray = _supportVectorMachine.Score(xyvaluesArray);
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