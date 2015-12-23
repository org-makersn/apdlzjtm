using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using QuantumConcepts.Formats.StereoLithography;
using QuantumConcepts.Common.Extensions;
using System.Linq;
using QuantumConcepts.Common.IO;
using System.Diagnostics;
using Design.Web.Front.Models;
using Library.ObjParser;
using Makersn.Models;

namespace Design.Web.Front.Helper
{
    public class STLHelper
    {
        List<QuantumConcepts.Formats.StereoLithography.Vertex> VertexList = new List<QuantumConcepts.Formats.StereoLithography.Vertex>();
        private Extent Size { get; set; }

        public StlModel GetStlModel(string filename, string extension)
        {
            StlModel stlModel = new StlModel();
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

            if (extension == "stl")
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

                        //m.X = vertice.X;
                        //m.X = vertice.Y;
                        //m.X = vertice.Z;
                        //stlModel.VertexLst.Add(m);
                    }
                }
            }

            if (extension == "obj")
            {
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

                    //m.X = (float)objDoc.VertexList[i].X;
                    //m.X = (float)objDoc.VertexList[i].Y;
                    //m.X = (float)objDoc.VertexList[i].Z;
                    //stlModel.VertexLst.Add(m);
                }
            }

            //for (int i = 0; i < facets.Count(); i++)
            //{
            //    stlModel.faces[index] = 0;
            //    index++;

            //    for (int j = 0; j < 3; j++)
            //    {
            //        stlModel.faces[index] = indexF;

            //        stlModel.vertices[indexV] = facets.Facets[i].Vertices[j].X;
            //        indexV++;
            //        stlModel.vertices[indexV] = facets.Facets[i].Vertices[j].Y;
            //        indexV++;
            //        stlModel.vertices[indexV] = facets.Facets[i].Vertices[j].Z;
            //        indexV++;

            //        indexF++;
            //        index++;
            //    }
            //}

            return stlModel;
        }

        #region 3D파일 사이즈 구하기
        public StlSize GetSizeFor3DFile(string path, string ext)
        {
            StlSize getSize = new StlSize();

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
                case "stl":
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

                case "obj":
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
            getSize.Volume = Math.Round(volume, 1);
            //volume = slicing(path);
            //getSize.Volume = Math.Round(volume, 1); 

            return getSize;
        }
        #endregion

        public double slicing(string input)
        {
            Process procConvert = new Process();
            //string fileName = "ipad";
            //string workingFolder = "C:\\Users\\Administrator\\Desktop\\3D_Modeling\\";

            //AppDomain.CurrentDomain.BaseDirectory
            //string ini = "C:\\kimgs\\DefaultForMakers1.ini";
            string domainPath = AppDomain.CurrentDomain.BaseDirectory;
            string ini = domainPath + "/slic3r/DefaultForMakers1.ini";
            string output = changeExt(input,"gcode");
            //string input = workingFolder + fileName + ".stl";
            string log = changeExt(input, "log");

            procConvert.StartInfo.FileName = domainPath + "/slic3r/slic3r-mswin-x64-1-2-9a-stable/Slic3r/Slic3r.exe";



            procConvert.EnableRaisingEvents = true;
            StringBuilder sb = new StringBuilder();
            sb.Append("--load ");
            sb.Append(ini);
            sb.Append(" --print-center ");
            sb.Append("100");
            sb.Append(",");
            sb.Append("100");
            sb.Append(" -o ");
            sb.Append(output);
            sb.Append(" ");
            sb.Append(input);
            procConvert.StartInfo.Arguments = sb.ToString();
            procConvert.StartInfo.UseShellExecute = false;
            procConvert.StartInfo.RedirectStandardOutput = true;
            procConvert.StartInfo.RedirectStandardError = true;
            procConvert.Start();
            // Start the asynchronous read of the standard output stream.




            //procConvert.Start();

            //procConvert.BeginOutputReadLine();
            //procConvert.BeginErrorReadLine();

            var result = procConvert.StandardOutput.ReadToEnd();

            procConvert.WaitForExit();


            procConvert.StartInfo.UseShellExecute = false;
            procConvert.StartInfo.RedirectStandardOutput = true;

            File.WriteAllText(log, result);

            try
            {
                string[] a = result.Split('(');
                string b = a[1].Split(')')[0];
                return System.Convert.ToDouble(b.Split('c')[0]);
            }catch(Exception e){
                return 0;
            }

        }

        private string changeExt(string path,string ext) {

            //string[] temp = path.Split('.');

            string temp2 = path.Substring(0, path.LastIndexOf('.'));
            return temp2 + "." + ext;

        }


    }
}
