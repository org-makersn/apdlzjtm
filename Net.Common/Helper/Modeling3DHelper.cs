using Library.ObjParser;
using Net.Common.Model;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Net.Common.Helper
{
    public class Modeling3DHelper
    {
        private List<QuantumConcepts.Formats.StereoLithography.Vertex> VertexList = null;
        private Extent Size { get; set; }

        #region _3DModel 변환
        /// <summary>
        /// _3DModel 변환
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public _3DModel Get3DModel(string filename, string extension)
        {
            _3DModel stlModel = new _3DModel();
            Metadata metadata = new Metadata();
            metadata.formatVersion = "3";
            metadata.generatedBy = "Mersh";
            stlModel.metadata = metadata;
            stlModel.scale = "1.0";
            stlModel.materials = new string[] { };
            stlModel.morphTargets = new string[] { };
            stlModel.morphColors = new string[] { };
            stlModel.normals = new string[] { };
            stlModel.colors = new string[] { };
            stlModel.uvs = new string[,] { };

            int indexF = 0;
            int indexV = 0;

            if (extension == ".stl")
            {
                STLDocument facets = STLDocument.Open(filename);

                int index = 0;

                stlModel.faces = new int[facets.Count() * 4];
                stlModel.vertices = new float[facets.Count() * 9];
                foreach (var facet in facets)
                {
                    stlModel.faces[index] = 0;

                    index++;
                    foreach (var vertice in facet.Vertices)
                    {
                        VertexM m = new VertexM();
                        stlModel.faces[index] = indexF;

                        stlModel.vertices[indexV] = vertice.X;
                        indexV++;
                        stlModel.vertices[indexV] = vertice.Y;
                        indexV++;
                        stlModel.vertices[indexV] = vertice.Z;
                        indexV++;

                        indexF++;
                        index++;
                    }
                }
            }

            if (extension == ".obj")
            {
                VertexList = new List<QuantumConcepts.Formats.StereoLithography.Vertex>();
                OBJDocument objDoc = new OBJDocument().LoadObj(filename);

                stlModel.faces = new int[objDoc.FaceList.Count() * 4];
                stlModel.vertices = new float[objDoc.VertexList.Count() * 9];

                for (int i = 0; i < objDoc.FaceList.Count; i++)
                {
                    stlModel.faces[indexF] = 0;
                    indexF++;
                    stlModel.faces[indexF] = objDoc.FaceList[i].VertexIndexList[0] - 1;
                    indexF++;
                    stlModel.faces[indexF] = objDoc.FaceList[i].VertexIndexList[1] - 1;
                    indexF++;
                    stlModel.faces[indexF] = objDoc.FaceList[i].VertexIndexList[2] - 1;
                    indexF++;
                }

                for (int i = 0; i < objDoc.VertexList.Count; i++)
                {
                    VertexM m = new VertexM();
                    stlModel.vertices[indexV] = (float)objDoc.VertexList[i].X;
                    indexV++;
                    stlModel.vertices[indexV] = (float)objDoc.VertexList[i].Y;
                    indexV++;
                    stlModel.vertices[indexV] = (float)objDoc.VertexList[i].Z;
                    indexV++;
                }
            }

            return stlModel;
        } 
        #endregion

        #region 3D파일 사이즈 구하기
        /// <summary>
        /// 3D파일 사이즈 구하기
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public ModelingSize GetSizeFor3DFile(string path, string ext)
        {
            ModelingSize getSize = new ModelingSize();

            double volume = 0;
            double x1 = 0;
            double y1 = 0;
            double z1 = 0;
            double x2 = 0;
            double y2 = 0;
            double z2 = 0;
            double x3 = 0;
            double y3 = 0;
            double z3 = 0;

            switch (ext)
            {
                case ".stl":
                    STLDocument facets = STLDocument.Open(path);
                    //stl
                    Size = new Extent
                    {
                        XMax = facets.Facets.Max(f => f.Vertices.Max(v => v.X)),
                        XMin = facets.Facets.Min(f => f.Vertices.Min(v => v.X)),
                        YMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Y)),
                        YMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Y)),
                        ZMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Z)),
                        ZMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Z))
                    };

                    for (int i = 0; i < facets.Facets.Count; i++)
                    {
                        x1 = facets.Facets[i].Vertices[0].X;
                        y1 = facets.Facets[i].Vertices[0].Y;
                        z1 = facets.Facets[i].Vertices[0].Z;

                        x2 = facets.Facets[i].Vertices[1].X;
                        y2 = facets.Facets[i].Vertices[1].Y;
                        z2 = facets.Facets[i].Vertices[1].Z;

                        x3 = facets.Facets[i].Vertices[2].X;
                        y3 = facets.Facets[i].Vertices[2].Y;
                        z3 = facets.Facets[i].Vertices[2].Z;

                        volume +=
                            (-x3 * y2 * z1 +
                            x2 * y3 * z1 +
                            x3 * y1 * z2 -
                            x1 * y3 * z2 -
                            x2 * y1 * z3 +
                            x1 * y2 * z3) / 6;
                    }
                    break;

                case ".obj":
                    VertexList = new List<QuantumConcepts.Formats.StereoLithography.Vertex>();
                    OBJDocument objDoc = new OBJDocument().LoadObj(path);
                    int[] idx = new int[3];


                    Size = new Extent
                    {
                        XMax = objDoc.VertexList.Where(w => (int)w.X != 0).Max(v => v.X),
                        XMin = objDoc.VertexList.Where(w => (int)w.X != 0).Min(v => v.X),
                        YMax = objDoc.VertexList.Where(w => (int)w.Y != 0).Max(v => v.Y),
                        YMin = objDoc.VertexList.Where(w => (int)w.Y != 0).Min(v => v.Y),
                        ZMax = objDoc.VertexList.Where(w => (int)w.Z != 0).Max(v => v.Z),
                        ZMin = objDoc.VertexList.Where(w => (int)w.Z != 0).Min(v => v.Z)
                    };

                    int vertexCnt = objDoc.VertexList.Count();
                    for (int i = 0; i < objDoc.FaceList.Count; i++)
                    {
                        idx[0] = objDoc.FaceList[i].VertexIndexList[0] - 1;
                        idx[1] = objDoc.FaceList[i].VertexIndexList[1] - 1;
                        idx[2] = objDoc.FaceList[i].VertexIndexList[2] - 1;
                        if (idx[0] > 0 && idx[1] > 0 && idx[2] > 0)
                        {
                            if (idx[0] > vertexCnt && idx[1] > vertexCnt && idx[2] > vertexCnt)
                            {
                                //log 로 남겨보기
                            }
                            else
                            {
                                x1 = objDoc.VertexList[idx[0]].X;
                                y1 = objDoc.VertexList[idx[0]].Y;
                                z1 = objDoc.VertexList[idx[0]].Z;

                                x2 = objDoc.VertexList[idx[1]].X;
                                y2 = objDoc.VertexList[idx[1]].Y;
                                z2 = objDoc.VertexList[idx[1]].Z;

                                x3 = objDoc.VertexList[idx[2]].X;
                                y3 = objDoc.VertexList[idx[2]].Y;
                                z3 = objDoc.VertexList[idx[2]].Z;

                                volume +=
                                    (-x3 * y2 * z1 +
                                    x2 * y3 * z1 +
                                    x3 * y1 * z2 -
                                    x1 * y3 * z2 -
                                    x2 * y1 * z3 +
                                    x1 * y2 * z3) / 6;
                            }
                        }
                        else
                        {
                            //log 로 남겨보기
                        }
                    }
                    break;
            }
            volume = volume < 0 ? volume * -1 : volume;

            getSize.X = Math.Round(Size.XSize, 1);
            getSize.Y = Math.Round(Size.YSize, 1);
            getSize.Z = Math.Round(Size.ZSize, 1);
            getSize.ObjectVolume = Math.Round(volume, 1) / 1000;

            return getSize;
        }
        #endregion

        #region 슬라이싱
        /// <summary>
        /// 슬라이싱 slic3r
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public double Slicing(string filePath, string workingDir)
        {
            double ret = 0;
            string config = "config.ini";

            string gcode = new FileHelper().ChangeExt(filePath, "gcode");
            string log = new FileHelper().ChangeExt(filePath, "log");

            StringBuilder command = new StringBuilder();
            command.Append("--load ");
            command.Append(config);
            command.Append(" --print-center ");
            command.Append("100,");
            command.Append("100");
            command.Append(" -o ");
            command.Append(gcode);
            command.Append(" ");
            command.Append(filePath);

            using (Process proc = new Process())
            {
                //proc.EnableRaisingEvents = true;
                proc.StartInfo.WorkingDirectory = workingDir;
                proc.StartInfo.FileName = string.Format(@"{0}\{1}", workingDir, "slic3r-console.exe");
                proc.StartInfo.Arguments = command.ToString();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                proc.WaitForExit();

                //proc.BeginOutputReadLine();
                //proc.BeginErrorReadLine();

                string output = proc.StandardOutput.ReadToEnd();
                if (!string.IsNullOrEmpty(output))
                {
                    new FileHelper().FileWriteAllText(log, output);

                    string[] strArr = output.Split('(');
                    string str = strArr[1].Split(')')[0];

                    ret = Convert.ToDouble(str.Split('c')[0]);
                }

                string error = proc.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                {
                    new FileHelper().FileWriteAllText(log, error);
                }
            }

            return ret;
        } 
        #endregion
    }
}
