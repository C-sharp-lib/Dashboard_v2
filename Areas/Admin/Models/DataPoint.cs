using System.Runtime.Serialization;

namespace Dash.Areas.Admin.Models;

[DataContract]
public class DataPoint
{
    public DataPoint(double x, double y, string indexLabel)
    {
        this.X = x;
        this.Y = y;
        this.IndexLabel = indexLabel;
    }

    [DataMember(Name = "x")] public Nullable<double> X = null;
    [DataMember(Name = "y")] public Nullable<double> Y = null;
    [DataMember(Name = "indexLabel")] public string IndexLabel = "";

}