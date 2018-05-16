using System;
using JavaDocToUml.Parser;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JavaDocToUml.Uml
{
    public class GenerateUxf
    {
		public static void MakingUxfFile(List<ClassInfo> classInfos, string path)
		{
			using (var writer = new StreamWriter(path))
			{
				writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>");
				writer.WriteLine("<diagram program=\"umlet\" version=\"14.2\">");
				writer.WriteLine("<zoom_level>10</zoom_level>");
				foreach(var classInfo in classInfos)
				{
					writer.WriteLine("<element>");
					writer.WriteLine("<id>UMLClass</id>");
					writer.WriteLine("<coordinates>");
					writer.WriteLine("<x>20</x>");
					writer.WriteLine("<y>560</y>");
					writer.WriteLine("<w>680</w>");
					writer.WriteLine("<h>210</h>");
					writer.WriteLine("</coordinates>");
					writer.Write("<panel_attributes>");
					if (classInfo.Type == ClassType.Interface)
						writer.WriteLine("&lt;&lt;interface&gt;&gt;");
					if (classInfo.Type != ClassType.Abstract)
						writer.WriteLine(classInfo.Name);
					else
						writer.WriteLine("/" + classInfo.Name + "/");
					writer.WriteLine("--");

					foreach(var fieldInfo in classInfo.Fields)
					{
						if (fieldInfo.AccessModifiers == "public")
							writer.Write("+ ");
						else if (fieldInfo.AccessModifiers == "private")
							writer.Write("- ");
						else if (fieldInfo.AccessModifiers == "protected")
							writer.Write("# ");

						writer.Write(fieldInfo.Name + ": ");
						writer.WriteLine(fieldInfo.Type);
					}
					writer.WriteLine("--");
                    
					foreach(var methodInfo in classInfo.Methods)
					{
						if (methodInfo.Type == MethodType.Abstract)
							writer.Write("/");

						if (methodInfo.AccessModifiers == "public")
                            writer.Write("+ ");
						else if (methodInfo.AccessModifiers == "private")
                            writer.Write("- ");
						else if (methodInfo.AccessModifiers == "protected")
                            writer.Write("# ");

						writer.Write(methodInfo.Name + "(");
						for (int i = 0; i < methodInfo.Arguments.Count; i++)
                        {
							writer.Write(methodInfo.Arguments[i][1] + " " + methodInfo.Arguments[i][0]);
							if (i != methodInfo.Arguments.Count - 1)
								writer.Write(", ");
                        }
						writer.Write(")");

						if (methodInfo.reurnType != null)
							writer.Write(": " + methodInfo.reurnType);

						if (methodInfo.Type == MethodType.Abstract)
							writer.WriteLine("/");
						else
							writer.WriteLine("");
					}

					writer.WriteLine("</panel_attributes>");
					writer.WriteLine("<additional_attributes/>");
					writer.WriteLine("</element>");
				}
				writer.WriteLine("</diagram>");
			}
		}
    }
}
