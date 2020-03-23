using System;
using System.Globalization;
using CsvHelper.Configuration;

namespace Regressors.Entities
{
    public class XYtoZ
    {
        public Tuple<double, double> XY { get; set; }
        public double X { get {return XY.Item1;} }
        public double Y { get {return XY.Item2;} }
        public double Z { get; set; }

        public override string ToString()
        {
            return @"<<{X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.InvariantCulture)}>, {Z.ToString(CultureInfo.InvariantCulture)}>";
        }
    }

    public sealed class XYtoZClassMap : ClassMap<XYtoZ>
    {
        public XYtoZClassMap()
        {
            Map(m => m.X).Index(0).Name("X");
            Map(m => m.Y).Index(1).Name("Y");
            Map(m => m.Z).Index(2).Name("Z");

            Map(m => m.XY).Ignore();
        }
    }
}
