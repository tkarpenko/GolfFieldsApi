namespace Golf.Fields.Shared
{
    public static class DirectoryPath
    {
        /// <summary>
        /// get cross-platform path
        /// </summary>
        /// <param name="currentBinPath"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetDirectoryPath(string currentBinPath, string relativePath)
        {
            var directoryInfo = new DirectoryInfo(currentBinPath);

            while (directoryInfo.Parent != null)
            {
                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, relativePath));

                if (projectDirectoryInfo.Exists)
                    return projectDirectoryInfo.FullName;

                directoryInfo = directoryInfo.Parent;
            }

            throw new DirectoryPathException(Resources.Messages.DirectoryWithRelativePathWasNotFound.Replace("{0}", relativePath));
        }


    }
}

