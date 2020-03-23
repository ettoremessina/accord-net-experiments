using System;
using Regressors.Testers;
namespace Regressors
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                ExecutePolynomialLeastSquaresRegressor1to1Tester();
                //ExecuteLinearOrdinaryLeastSquaresRegressor1to1Tester();
                //ExecuteMultipleLinearOrdinaryLeastSquares2to1RegressorTester();
                //ExecutePolynomialSupportVectorMachineFanChenLin2to1RegressorTester();
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().FullName + " " + ex.Message);
                return -1;
            }
        }

        private static void ExecutePolynomialLeastSquaresRegressor1to1Tester()
        {
            PolynomialLeastSquares1to1RegressorTester tester = new PolynomialLeastSquares1to1RegressorTester();
            tester.RunTest();
        }

        private static void ExecuteLinearOrdinaryLeastSquaresRegressor1to1Tester()
        {
            SimpleLinearOrdinaryLeastSquares1to1RegressorTester tester = new SimpleLinearOrdinaryLeastSquares1to1RegressorTester();
            tester.RunTest();
        }

        private static void ExecuteMultipleLinearOrdinaryLeastSquares2to1RegressorTester()
        {
            MultipleLinearOrdinaryLeastSquares2to1RegressorTester tester = new MultipleLinearOrdinaryLeastSquares2to1RegressorTester();
            tester.RunTest();
        }

        private static void ExecutePolynomialSupportVectorMachineFanChenLin2to1RegressorTester()
        {
            PolynomialSupportVectorMachineFanChenLin2to1RegressorTester tester = new PolynomialSupportVectorMachineFanChenLin2to1RegressorTester();
            tester.RunTest();
        }
    }
}
