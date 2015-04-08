using System.IO;

namespace Fluid
{
    public static class ExternalResources
    {
        /// <summary>
        /// Find's a file in the file system
        /// </summary>
        /// <param name="filename">The name of the file to find</param>
        /// <returns>The file path</returns>
        public static string FindFile(string filename)
        {
            if (!filename.StartsWith("\\"))
            {
                filename = filename.Insert(0, "\\");
            }

            string currentDirectory = Directory.GetCurrentDirectory();
            string targetFile = currentDirectory + filename;
            string targetFileName = Path.GetFileName(targetFile);

            if (File.Exists(targetFile))
            {
                return targetFile;
            }

            DirectoryInfo parentDir = Directory.GetParent(currentDirectory);

            int roots = 0;
            while (roots < 2)
            {
                if (parentDir.Parent == null)
                {
                    return null;
                }

                parentDir = parentDir.Parent;
                roots++;
            }

            string[] dirs = Directory.GetDirectories(parentDir.FullName);
            for (int i = 0; i < dirs.Length; i++)
            {
                string dirName = Path.GetFileName(dirs[i]);
                if (string.Compare(dirName, "packages", false) == 0)
                {
                    //Found packages folder
                    string[] packages = Directory.GetDirectories(dirs[i]);
                    for (int j = 0; j < packages.Length; j++)
                    {
                        //Found fluid package
                        string packageName = Path.GetFileName(packages[j]);
                        if (packageName.Contains("Fluid"))
                        {
                            string[] fluidDirs = Directory.GetDirectories(packages[j]);
                            for (int k = 0; k < fluidDirs.Length; k++)
                            {
                                string fluidDirName = Path.GetFileName(fluidDirs[k]);
                                if (string.Compare(fluidDirName, "build", false) == 0)
                                {
                                    //Found fluid build folder
                                    string[] buildFiles = Directory.GetFiles(fluidDirs[k]);
                                    for (int l = 0; l < buildFiles.Length; l++)
                                    {
                                        string buildFileName = Path.GetFileName(buildFiles[l]);
                                        if (string.Compare(buildFileName, targetFileName, false) == 0)
                                        {
                                            return buildFiles[l];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}
