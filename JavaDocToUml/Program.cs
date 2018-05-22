using System;
using System.IO;
using JavaDocToUml.File;

namespace JavaDocToUml
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			if (args.Length != 3)
				Environment.Exit(0);
			string dirPath = args[0];
			Parser.ParseHtml parseHtml = new Parser.ParseHtml(dirPath);
			parseHtml.GetPackage();
			foreach (var packageName in parseHtml.package)
			{
				string path = packageName.Replace('.', '/') + "/";
				foreach (var htmlPath in Directory.GetFiles(dirPath + path, "*.html", SearchOption.TopDirectoryOnly))
				{
					string htmlName = htmlPath.Replace(dirPath + path, "");
					if (htmlName.Contains("package-"))
						continue;
					parseHtml.ParseClassHtml(htmlPath);
				}
			}
			parseHtml.ShowClass();
			if (args[2] == "0")
				parseHtml.GenerateUml(args[1]);
			else
				parseHtml.GenerateLatexTemplate(args[1]);
            
        }
    }
}
