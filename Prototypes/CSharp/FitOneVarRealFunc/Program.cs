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
                TestOneSineCurve();
                TestTwoExpCurve();
                TestThreeSqrtAbsCurve();
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
            string dstestFileName = Path.Combine("out", "testds1.csv");
            string predictionFileName = Path.Combine("out", "prediction1.csv");
            string octaveScriptFilename = Path.Combine("out", "octave1.m");
            ExportToCsvFile(dsTest, dstestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dstestFileName, predictionFileName, octaveScriptFilename);
            
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
            string dstestFileName = Path.Combine("out", "testds2.csv");
            string predictionFileName = Path.Combine("out", "prediction2.csv");
            string octaveScriptFilename = Path.Combine("out", "octave2.m");
            ExportToCsvFile(dsTest, dstestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dstestFileName, predictionFileName, octaveScriptFilename);
            
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
            string dstestFileName = Path.Combine("out", "testds3.csv");
            string predictionFileName = Path.Combine("out", "prediction3.csv");
            string octaveScriptFilename = Path.Combine("out", "octave3.m");
            ExportToCsvFile(dsTest, dstestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dstestFileName, predictionFileName, octaveScriptFilename);
            Console.Error.WriteLine("Terminated");
        }

        private static void GenerateOctaveScript(string dstestFileName, string predictionFileName, string octaveScriptFilename)
        {
            string octaveScript = @$"testds=csvread('{Path.GetFileName(dstestFileName)}', 1, 0);

coltest1 = testds(:, 1);
coltest2 = testds(:, 2);

prediction=csvread('{Path.GetFileName(predictionFileName)}', 1, 0);
colpred1 = prediction(:, 1);
colpred2 = prediction(:, 2);

plot(coltest1, coltest2, 'linewidth', 2, colpred1, colpred2, 'linewidth', 2);

pause;";

        File.WriteAllText(octaveScriptFilename, octaveScript);
        }
   }
}
