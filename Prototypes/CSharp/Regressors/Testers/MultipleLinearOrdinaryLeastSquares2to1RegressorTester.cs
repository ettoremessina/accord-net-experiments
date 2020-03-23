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
    public class MultipleLinearOrdinaryLeastSquares2to1RegressorTester
    {
        public void RunTest()
        {
            Run(TwoVarsRealFuncSynthDataSetGenerator.GenerateEllipticParaboloidDS, -2.0, 2.0, -2.0, 2, 0.01, 0.045, false, "MLOLSR_01_");
            //Run(TwoVarsRealFuncSynthDataSetGenerator.GenerateHyperbolicParaboloidDS, -2.0, 2.0, -2.0, 2, 0.01, 0.045, false, "MLOLSR_02_");
            //Run(TwoVarsRealFuncSynthDataSetGenerator.GenerateArcTanDS, -5, 5, -5, 5, 0.01, 0.045, false, "MLOLSR_05_");
        }

        private static void ExportToCsvFile(IEnumerable<XYtoZ> ds, string pathToFile)
        {
            using (var writer = new StreamWriter(pathToFile, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<XYtoZClassMap>();
                csv.WriteRecords(ds);    
            }
        }

        private static void GenerateOctaveScript(string dstestFileName, string predictionFileName, string octaveScriptFilename)
        {
            string octaveScript = @$"testds=csvread('{Path.GetFileName(dstestFileName)}', 1, 0);
coltest1 = testds(:, 1);
coltest2 = testds(:, 2);
coltest3 = testds(:, 3);

prediction=csvread('{Path.GetFileName(predictionFileName)}', 1, 0);
colpred1 = prediction(:, 1);
colpred2 = prediction(:, 2);
colpred3 = prediction(:, 3);

scatter3(coltest1, coltest2, coltest3, 1, 'filled');
hold on
scatter3(colpred1, colpred2, colpred3, 2, 'filled')
hold off

set(gca, 'linewidth', 1.5, 'fontsize', 20)
";

            File.WriteAllText(octaveScriptFilename, octaveScript);
        }

        private static void Run
        (
            Func<double, double, double, double, double, IEnumerable<XYtoZ>> dsGenerator,
            double xbegin, double xend,
            double ybegin, double yend,
            double learnDSStep, double testDSStep,
            bool isRobust,
            string prefix
        )
        {
            Console.Error.WriteLine("Generating learning dataset");
            IList<XYtoZ> dsLearn = dsGenerator(xbegin, xend, ybegin, yend, learnDSStep).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XYtoZ> dsTest = dsGenerator(xbegin, xend, ybegin, yend, testDSStep).ToList();

            MultipleLinearOrdinaryLeastSquares2to1Regressor r = new MultipleLinearOrdinaryLeastSquares2to1Regressor(isRobust);
            Console.Error.WriteLine("Training");
            r.Learn(dsLearn);

            Console.Error.WriteLine("Predicting");
            IEnumerable<Tuple<double, double>> xvyaluesTest = dsTest.Select(i => new Tuple<double, double>(i.X, i.Y));
            IEnumerable<XYtoZ> prediction = r.Predict(xvyaluesTest);

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