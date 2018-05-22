using System;
using System.Linq;
using HtmlAgilityPack;
using JavaDocToUml.File;
using System.Xml;
using System.Collections.Generic;

namespace JavaDocToUml.Parser
{
    public class ParseHtml
    {
		string dirPath;
		public List<string> package;
		List<ClassInfo> classs;

        public ParseHtml(string dirPath)
        {
			this.dirPath = dirPath;
			package = new List<string>();
			classs = new List<ClassInfo>();
        }

        public void GetPackage()
		{
			if (System.IO.File.Exists(dirPath + "overview-frame.html"))
			{
				var doc = new HtmlDocument();
                doc.LoadHtml(LoadHtml.GetHtml(dirPath + "overview-frame.html"));
                string xPath = "/html/body/div[2]/ul/li/a";
                var nodes = doc.DocumentNode.SelectNodes(xPath);
                foreach (var node in nodes)
                {
                    package.Add(node.InnerText);
                }
			}
			else
			{
				Console.WriteLine("overview-frame.htmlが見つからないので存在する全てのパッケージ名を,区切りで入力してください");
				Console.WriteLine("例 : hoge.fuga,hoge.hogehoge");
				var str = Console.ReadLine();
				string[] packages = str.Split(',');
				foreach(var name in packages)
				{
					package.Add(name);
				}
			}
		}
        
        public void ParseClassHtml(string path)
		{
			var doc = new HtmlDocument();
            doc.LoadHtml(LoadHtml.GetHtml(path));

            //クラス名取得
			ClassInfo classInfo = new ClassInfo();
			string xPath = "/html/body/div[3]/h2";
			var nodes = doc.DocumentNode.SelectNodes(xPath);
			foreach (var node in nodes)
            {
				var classArray = node.InnerText.Split(' ');
				switch(classArray[0])
				{
					case "クラス":
						classInfo.Type = ClassType.Default;
						break;
					case "インタフェース":
						classInfo.Type = ClassType.Interface;
						break;
				}
				classInfo.Name = classArray[1];
            }

			List<string[]> allList = new List<string[]>(); 
            //フィールド名とメソッド名を取得
			classInfo.Fields = new List<FieldInfo>();
			classInfo.Methods = new List<MethodInfo>();
         
			xPath = "//td[@class='colLast']/code";
			nodes = doc.DocumentNode.SelectNodes(xPath);
			foreach (var node in nodes)
            {
				allList.Add(new string[] { "", node.InnerText });
            }

            //フィールドとかメソッドのアクセス修飾子とか型とか
			xPath = "//td[@class='colFirst']/code";
            nodes = doc.DocumentNode.SelectNodes(xPath);
			int i = 0;
			foreach(var node in nodes)
			{
				allList[i][0] = node.InnerText;
				i++;
			}

            //データをclasssに追加
			foreach(var info in allList)
			{
				if (!info[1].Contains("("))
				{
					FieldInfo fieldInfo = new FieldInfo();
					var typeArray = info[0].Split(' ');
					fieldInfo.Name = info[1];
					fieldInfo.Type = typeArray[1];
					fieldInfo.AccessModifiers = typeArray[0];
                    classInfo.Fields.Add(fieldInfo);
				}
				else
				{
					MethodInfo methodInfo = new MethodInfo();
					methodInfo.Type = MethodType.Default;
					string[] typeArray;
					if (!info[0].Contains(" "))
						typeArray = new string[] { "public", info[0] };
					else if (info[0].Contains("abstract"))
					{
						string[] tmpArray = info[0].Split(' ');
						typeArray = new string[] { tmpArray[0], tmpArray[2] };
						methodInfo.Type = MethodType.Abstract;
						classInfo.Type = ClassType.Abstract;
					}
					else
						typeArray = info[0].Split(' ');

					int length = info[1].Length;
					string name = info[1].Substring(0, info[1].IndexOf('('));
					int nameLength = name.Length;
					methodInfo.reurnType = typeArray[1];

					methodInfo.Name = name;
					methodInfo.AccessModifiers = typeArray[0];

					methodInfo.Arguments = new List<string[]>();
					string argument = info[1].Replace(name, "");
					argument = argument.Replace("(", "");
					argument = argument.Replace(")", "");
                    
					if (argument != "")
					{
						argument = argument.Replace("&nbsp;", " ");
						argument = argument.Replace(Environment.NewLine, "");
						argument = argument.Replace("        ", "");
						var arguments = argument.Split(',');
                        foreach (var a in arguments)
                        {
							methodInfo.Arguments.Add(a.Split(' '));
                        }
					}
                    

					classInfo.Methods.Add(methodInfo);
				}
			}
			classs.Add(classInfo);
		}

        public void ShowClass()
		{
			foreach (var classInfo in classs)
			{
				Console.WriteLine(classInfo.Name);
				foreach (var fieldInfo in classInfo.Fields)
				{
					Console.WriteLine(fieldInfo.Type + fieldInfo.Name);
				}

				foreach (var methodInfo in classInfo.Methods)
				{
					Console.WriteLine(methodInfo.reurnType + methodInfo.Name);
				}
			}
		}

        public void GenerateUml(string path)
		{
			Uml.GenerateUxf.MakingUxfFile(classs, path);
		}
		public void GenerateLatexTemplate(string path)
        {
			Latex.GenerateReportTemplate.GenerateTemplate(classs, path);
        }
    }

	public class ClassInfo
	{
		public string Name { get; set; }
		public ClassType Type { get; set; }
		public List<FieldInfo> Fields;
		public List<MethodInfo> Methods;
	}

	public class FieldInfo
	{
		public string Name { get; set; }
		public string Type { get; set; }
        public string AccessModifiers { get; set; }
	}

    public class MethodInfo
	{
		public string Name { get; set; }
		public string AccessModifiers { get; set; }
		public List<string[]> Arguments;
		public string reurnType { get; set; }
		public MethodType Type { get; set; }
	}

	public enum ClassType
	{
        Default,
		Abstract,
        Interface
	}

    public enum MethodType
	{
        Default,
        Abstract
	}
}
