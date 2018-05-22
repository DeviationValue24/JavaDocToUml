using System;
using System.IO;
using JavaDocToUml.Parser;
using System.Collections.Generic;

namespace JavaDocToUml.Latex
{
    public class GenerateReportTemplate
    {
		public static void GenerateTemplate(List<ClassInfo> classInfos, string path)
		{
			using (var writer = new StreamWriter(path))
			{
				foreach(var classInfo in classInfos)
				{
					writer.WriteLine("\\subsection*{" + classInfo.Name + "クラス}");
					writer.WriteLine("\\begin{itemize}");
					writer.WriteLine("\\item フィールド");
					writer.WriteLine("\\begin{itemize}");
					foreach(var fieldinfo in classInfo.Fields)
					{
						writer.WriteLine("\\item " + fieldinfo.Name + ": " + fieldinfo.Type + " ");
					}
					writer.WriteLine("\\end{itemize}");
					writer.WriteLine("\\item コンストラクタ");
					writer.WriteLine("\\begin{itemize}");
					writer.WriteLine("\\item ");
					writer.WriteLine("\\end{itemize}");
					writer.WriteLine("\\item メソッド");
					writer.WriteLine("\\begin{itemize}");
					foreach(var methodInfo in classInfo.Methods)
					{
						writer.Write("\\item " + methodInfo.Name + "(");
						for (int i = 0; i < methodInfo.Arguments.Count; i++)
                        {
                            writer.Write(methodInfo.Arguments[i][0] + " " + methodInfo.Arguments[i][1]);
                            if (i != methodInfo.Arguments.Count - 1)
                                writer.Write(", ");
                        }
                        writer.Write(")");
						writer.WriteLine("");
					}
					writer.WriteLine("\\end{itemize}");
					writer.WriteLine("\\end{itemize}");


				}
			}
		}
    }
}
