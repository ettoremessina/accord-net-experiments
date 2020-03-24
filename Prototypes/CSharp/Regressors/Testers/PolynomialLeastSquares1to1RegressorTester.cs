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
            Run(OneVarRealFuncSynthDataSetGenerator.GeneratePolynomialDS, -10, 10, 0.01, 0.045, 3, false, "PLSR_01");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateSineDS, -2 * Math.PI, 2 * Math.PI, 0.01, 0.045, 10, false, "PLSR_02");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialDS, -5, 5, 0.01, 0.045, 10, false, "PLSR_03");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateSqrtAbsDS, -5, 5, 0.01, 0.045, 20, false, "PLSR_04");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateLog1PlusAbsDS, -5, 5, 0.01, 0.045, 16, false, "PLSR_05");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateArcTanDS, -5, 5, 0.01, 0.045, 10, false, "PLSR_06");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateExponentialSineDS, -2 * Math.PI, 2 * Math.PI, 0.01, 0.045, 14, false, "PLSR_07");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateTanhDS, -5, 5, 0.01, 0.045, 4, false, "PLSR_08");
            Run(OneVarRealFuncSynthDataSetGenerator.GenerateMuffleWavedDS, -20, 20, 0.01, 0.045, 40, false, "PLSR_09");
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
            string subFolder
        )
        {
            Console.Error.WriteLine($"Started test #{subFolder}");
            
            Console.Error.WriteLine("Generating learning dataset");
            IList<XtoY> dsLearn = dsGenerator(begin, end, learnDSStep).ToList();

            Console.Error.WriteLine("Generating test dataset");
            IList<XtoY> dsTest = dsGenerator(begin, end, testDSStep).ToList();

            PolynomialLeastSquares1to1Regressor r = new PolynomialLeastSquares1to1Regressor(degree, isRobust);
            Console.Error.WriteLine("Training");
            r.Learn(dsLearn);
            Console.Out.WriteLine($"Learned polynomial {r.StringfyLearnedPolynomial()}");

            Console.Out.WriteLine("Predicting");
            IEnumerable<double> xvaluesTest = dsTest.Select(i => i.X);
            IEnumerable<XtoY> prediction = r.Predict(xvaluesTest);
            Console.Out.WriteLine($"Error: {r.ComputeError(dsTest, prediction)}");

            string targetDir = Path.Combine("out", subFolder);
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);
            Console.Out.WriteLine($"Saving");
            string dsLearnFileName = Path.Combine(targetDir, "learnds.csv");
            string dsTestFileName = Path.Combine(targetDir, "testds.csv");
            string predictionFileName = Path.Combine(targetDir, "prediction.csv");
            string octaveScriptFilename = Path.Combine(targetDir, "plotgraph.m");
            ExportToCsvFile(dsLearn, dsLearnFileName);
            ExportToCsvFile(dsTest, dsTestFileName);
            ExportToCsvFile(prediction, predictionFileName);
            GenerateOctaveScript(dsTestFileName, predictionFileName, octaveScriptFilename);
            
            Console.Error.WriteLine($"Terminated test #{subFolder}");
            Console.Error.WriteLine();
        }
    }
}