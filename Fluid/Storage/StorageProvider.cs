using System.IO;

namespace Fluid.Storage
{
    public abstract class StorageProvider
    {
        /// <summary>
        /// Gets the storage's directory
        /// </summary>
        public string Directory { get; private set; }

        /// <summary>
        /// Checks if the directory exists; if it doesn't tries to create the directory
        /// </summary>
        /// <returns>True if successful; otherwise false</returns>
        protected bool CheckDirectory()
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(Directory);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a file write stream for a value
        /// </summary>
        /// <param name="valueName">The value name</param>
        /// <returns>The stream if found; otherwise null</returns>
        protected Stream GetWriteStream(string valueName)
        {
            if (!CheckDirectory())
            {
                return null;
            }

            try
            {
                string filePath = Path.Combine(Directory, valueName + ".data");
                if (File.Exists(filePath))
                {
                    return File.Open(filePath, FileMode.Create);
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Gets a file read stream for a value
        /// </summary>
        /// <param name="valueName">The value name</param>
        /// <returns>The stream if found; otherwise null</returns>
        protected Stream GetReadStream(string valueName)
        {
            if (!CheckDirectory())
            {
                return null;
            }

            try
            {
                string filePath = Path.Combine(Directory, valueName + ".data");
                if (File.Exists(filePath))
                {
                    return File.Open(filePath, FileMode.Open);
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Reads raw file data for a value
        /// </summary>
        /// <param name="valueName">The value name</param>
        /// <returns>The raw file data if found; otherwise null</returns>
        protected string ReadValue(string valueName)
        {
            if (!CheckDirectory())
            {
                return null;
            }

            string filePath = Path.Combine(Directory, valueName + ".data");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return null;
        }

        /// <summary>
        /// Writes raw file data to a file
        /// </summary>
        /// <param name="valueName">The value's name</param>
        /// <param name="data">The data</param>
        protected void Write(string valueName, string data)
        {
            if (!CheckDirectory())
            {
                return;
            }

            string filePath = Path.Combine(Directory, valueName + ".data");
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// Saves data to storage
        /// </summary>
        /// <typeparam name="T">The type of data</typeparam>
        /// <param name="valueName">The value's name</param>
        /// <param name="value">The value</param>
        public abstract void Save<T>(string valueName, T value);

        /// <summary>
        /// Loads data from storage
        /// </summary>
        /// <typeparam name="T">The type of data</typeparam>
        /// <param name="valueName">The value's name</param>
        /// <returns>The type if loaded; otherwise the type default</returns>
        public abstract T Load<T>(string valueName);

        /// <summary>
        /// Creates a new storage provider
        /// </summary>
        public StorageProvider(string directory)
        {
            Directory = directory;
        }
    }
}
