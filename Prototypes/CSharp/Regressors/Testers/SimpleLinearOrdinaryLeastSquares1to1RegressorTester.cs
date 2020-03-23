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
    public class SimpleLinearOrdinaryLeastSquares1to1RegressorTester
    {
        public void RunTest()
        {
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateArcTanDS, -5, 5, 0.01, 0.045, "LOLSR_05_");
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
            string prefix
        )
        {
            Console.Error.WriteLine("Generating learning dataset");
            IList<XtoY> dsLearn = dsGenerator(begin, end, learnDSStep).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XtoY> dsTest = dsGenerator(begin, end, testDSStep).ToList();

            LinearOrdinaryLeastSquares1to1Regressor r = new LinearOrdinaryLeastSquares1to1Regressor();
            Console.Error.WriteLine("Training");
            r.Learn(dsLearn);

            Console.Error.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XtoY> prediction = r.Predict(xvaluesTest);

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