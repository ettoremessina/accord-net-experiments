using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using Regressors.Entities;
using Regressors.DataSets;
using Regressors.Wrappers;

namespace Regressors.Testers
{
    public class PolynomialLeastSquares1to1RegressorTester
    {
        public void RunTest()
        {
            //Run(OneVarRealFuncSynthDataSetGenerator.GeneratePolynomialDS, -10, 10, 0.01, 0.045, 3, false, "PLSR_01_");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateSineDS, -2 * Math.PI, 2 * Math.PI, 0.01, 0.045, 10, false, "PLSR_02_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialDS, -5, 5, 0.01, 0.045, 10, false, "PLSR_03_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateSqrtAbsDS, -5, 5, 0.01, 0.045, 20, false, "PLSR_04_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateLog1PlusAbsDS, -5, 5, 0.01, 0.045, 16, false, "PLSR_07_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateArcTanDS, -5, 5, 0.01, 0.045, 10, false, "PLSR_06_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialSineDS, -2 * Math.PI, false, 2 * Math.PI, 0.01, 0.045, 14, "PLSR_07_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateTanhDS, -5, 5, 0.01, 0.045, 4, false, "PLSR_08_");
            //Run(OneVarRealFuncSynthDataSetGenerator.GenerateMuffleWavedDS, -20, 20, 0.01, 0.045, 40, false, "PLSR_09_");
        }

        private static void ExportToCsvFile(IEnumerable<XtoY> ds, string pathToFile)
        {
            using (var writer = new StreamWriter(pathToFile, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(ds);    
            }
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
set(gca, 'linewidth', 1.5, 'fontsize', 20)
";

            File.WriteAllText(octaveScriptFilename, octaveScript);
        }

        private static void Run
        (
            Func<double, double, double, IEnumerable<XtoY>> dsGenerator,
            double begin, double end, double learnDSStep, double testDSStep,
            int degree, bool isRobust,
            string prefix
        )
        {
            Console.Error.WriteLine("Generating learning dataset");
            IList<XtoY> dsLearn = dsGenerator(begin, end, learnDSStep).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XtoY> dsTest = dsGenerator(begin, end, testDSStep).ToList();

            PolynomialLeastSquares1to1Regressor r = new PolynomialLeastSquares1to1Regressor(degree, isRobust);
            Console.Error.WriteLine("Training");
            r.Learn(dsLearn);
            Console.Error.WriteLine($"Learned polynomial {r.StringfyLearnedPolynomial()}");

            Console.Error.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XtoY> prediction = r.Predict(xvaluesTest);
            Console.Error.WriteLine($"Error: {r.ComputeError(dsTest, prediction)}");

            Console.Error.WriteLine("Saving");
            string dstestFileName = Path.Combine("out", $"{prefix}testd.csv");
            string predictionFileName = Path.Combine("out", $"{prefix}prediction.csv");
            string octaveScriptFilename = Path.Combine("out", $"{prefix}octave.m");
            ExportToCsvFile(dsTest, dstestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dstestFileName, predictionFileName, octaveScriptFilename);
            
            Console.Error.WriteLine("Terminated");
        }
    }
}