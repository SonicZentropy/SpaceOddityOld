﻿// /** 
//  * FileOps.cs
//  * Dylan Bailey
//  * 20161209
// */

namespace Zenobit.Editor.Utils
{
    #region Dependencies

    using System.IO;

    #endregion

    public static class FileOps
    {
        public static string GetDirectoryFromPath(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static string AddFileToDirectoryPath(string path, string filename)
        {
            return Path.Combine(path, filename);
        }

        public static FileInfo[] FindAllFilesRecursively(string basePath, string fileExtension = null)
        {
            var files = new FileInfo[1];
            if (string.IsNullOrEmpty(basePath))
                return files;

            var dirPath = Path.GetDirectoryName(basePath);

            return string.IsNullOrEmpty(dirPath)
                ? files
                : new DirectoryInfo(basePath).GetFiles("*" + fileExtension, SearchOption.AllDirectories);
        }

        public static string ReturnFileAsString(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var strdata = reader.ReadToEnd();
                return ReplaceLineEndings(strdata);
            }
        }

	    public static void WriteStringToFile(string fileData, string filePath)
	    {
			using (FileStream raFile = File.Open(filePath, FileMode.Create))
			{
				using (var writer = new StreamWriter(raFile))
				{
					writer.Write(fileData);
				}
			}
		}

	    public static void ReplaceTextInFile(string filePath, string TextToReplace, string ReplacementText)
	    {
			string fileData = ReturnFileAsString(filePath);
			fileData = fileData.Replace(TextToReplace, ReplacementText);
			WriteStringToFile(fileData, filePath);
		}

	    public static string GetStringAfterLastSlash(string inString)
	    {
			int pos = inString.LastIndexOf("/") + 1;
			return inString.Substring(pos, inString.Length - pos);
		}

	    public static string ReplaceLineEndings(string inString)
		{
		    return inString.Replace("\r\n", "\n").Replace("\r", "\n");
		}
    }
}