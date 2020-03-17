using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using FitOneVarRealFunc.Entities;
using FitOneVarRealFunc.DataSets;
using FitOneVarRealFunc.Regressors;

namespace FitOneVarRealFunc
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                //TestOneSineCurve();
                //TestTwoExpCurve();
                TestThreeSqrtAbsCurve();

                /*
                    https://chart-studio.plot.ly
                */
                return 0;
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().FullName + " " + ex.Message);
                return -1;
            }
        }

        private static void ExportToCsvFile(IEnumerable<XYItem> ds, string pathToFile)
        {
            using (var writer = new StreamWriter(pathToFile, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(ds);    
            }
        }

        private static void TestOneSineCurve()
        {
            double begin =  -2 * Math.PI;
            double end =  2 * Math.PI;

            Console.Error.WriteLine("Generating training dataset");
            IList<XYItem> dsTrain = OneVarRealFuncSynthDataSetGenerator.GenerateSineCurve(begin, end, 0.01).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XYItem> dsTest = OneVarRealFuncSynthDataSetGenerator.GenerateSineCurve(begin, end, 0.045).ToList();

            PolynomialRegressor r = new PolynomialRegressor(5);
            Console.Error.WriteLine("Training");
            r.Train(dsTrain);

            Console.Error.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XYItem> prediction = r.Predict(xvaluesTest);

            Console.Error.WriteLine("Saving");
            ExportToCsvFile(dsTest, Path.Combine("out", "testds1.csv"));
            ExportToCsvFile(prediction, Path.Combine("out", "prediction1.csv"));
            
            Console.Error.WriteLine("Terminated");
        }

        private static void TestTwoExpCurve()
        {
            double begin =  -5.0;
            double end =  5.0;

            Console.Error.WriteLine("Generating training dataset");
            IList<XYItem> dsTrain = OneVarRealFuncSynthDataSetGenerator.GenerateExponentialCurve(begin, end, 0.01).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XYItem> dsTest = OneVarRealFuncSynthDataSetGenerator.GenerateExponentialCurve(begin, end, 0.045).ToList();

            PolynomialRegressor r = new PolynomialRegressor(5);
            Console.Error.WriteLine("Training");
            r.Train(dsTrain);

            Console.Error.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XYItem> prediction = r.Predict(xvaluesTest);

            Console.Error.WriteLine("Saving");
            ExportToCsvFile(dsTest, Path.Combine("out", "testds2.csv"));
            ExportToCsvFile(prediction, Path.Combine("out", "prediction2.csv"));
            
            Console.Error.WriteLine("Terminated");
        }

        private static void TestThreeSqrtAbsCurve()
        {
            double begin =  -5.0;
            double end =  5.0;

            Console.Error.WriteLine("Generating training dataset");
            IList<XYItem> dsTrain = OneVarRealFuncSynthDataSetGenerator.GenerateSqrtAbsCurve(begin, end, 0.01).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XYItem> dsTest = OneVarRealFuncSynthDataSetGenerator.GenerateSqrtAbsCurve(begin, end, 0.045).ToList();

            PolynomialRegressor r = new PolynomialRegressor(7);
            Console.Error.WriteLine("Training");
            r.Train(dsTrain);

            Console.Error.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XYItem> prediction = r.Predict(xvaluesTest);

            Console.Error.WriteLine("Saving");
            ExportToCsvFile(dsTest, Path.Combine("out", "testds3.csv"));
            ExportToCsvFile(prediction, Path.Combine("out", "prediction3.csv"));
            
            Console.Error.WriteLine("Terminated");
        }
   }
}
