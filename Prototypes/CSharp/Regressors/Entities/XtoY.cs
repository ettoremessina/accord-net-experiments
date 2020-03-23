using System;
using System.Globalization;

namespace Regressors.Entities
{
    public class XtoY
    {
        public double X {get; set;}
        public double Y { get; set; }

        public override string ToString()
        {
            return $"<{X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.InvariantCulture)}>";
        }
    }
}