using System;
using System.IO;
using System.Collections.Generic;

namespace JavaDocToUml.File
{
    public class LoadHtml
    {
		string dirPath;
		List<string> packageList;

        public LoadHtml(string dirPath)
        {
			this.dirPath = dirPath;
			System.Diagnostics.Process.Start(dirPath);
			string[] files = Directory.GetFiles(dirPath, "*", SearchOption.TopDirectoryOnly);
			//string[] files = Directory.GetDirectories(dirPath);
			foreach (var name in files)
				Console.WriteLine(name);
        }

		public string[] SearchDirectory(string path)
		{
			foreach(string rootPath in packageList)
			{
				if (rootPath == "index-files")
					continue;
				foreach(string name in Directory.GetDirectories(rootPath))
				{
					if (path == "class-use")
						continue;
					//foreach
				}
			}
			return null;

			//string[] dirs = Directory.GetDirectories(currentPath);
		}

		public static string GetHtml(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			string html = streamReader.ReadToEnd();
			streamReader.Close();
			return html;
		}

		public static string[] GetDirHtml(string path)
		{
			return Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
		}


    }
}
