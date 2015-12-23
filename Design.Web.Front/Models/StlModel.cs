using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design.Web.Front.Models
{
    public class StlModel
    {
        public virtual Metadata metadata { get; set; }
        public virtual string scale { get; set; }
        public virtual string[] materials { get; set; }
        public virtual string[] morphTargets { get; set; }
        public virtual string[] morphColors { get; set; }
        public virtual string[] normals { get; set; }
        public virtual string[] colors { get; set; }
        public virtual string[,] uvs { get; set; }
        public virtual int[] faces { get; set; }
        public virtual float[] vertices { get; set; }
        public virtual List<VertexM> VertexLst { get; set; }
    }

    public class Metadata
    {
        public virtual string formatVersion { get; set; }
        public virtual string generatedBy { get; set; }
    }

    public class VertexM
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public VertexM() { }

        public VertexM(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public class StlSize
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Volume { get; set; }
    }
}
