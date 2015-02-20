using Newtonsoft.Json;
using System;

namespace Fluid.Storage
{
    public class JsonStorageProvider : StorageProvider
    {
        private JsonSerializerSettings m_Settings;

        /// <summary>
        /// Gets or Sets the storage logger
        /// </summary>
        public FluidLog Logger { get; set; }

        /// <summary>
        /// Saves data to storage
        /// </summary>
        /// <typeparam name="T">The type of data</typeparam>
        /// <param name="valueName">The value's name</param>
        /// <param name="value">The value</param>
        public override void Save<T>(string valueName, T value)
        {
            if (value == null)
            {
                Logger.Add(FluidLogCategory.Suggestion, "Check objects if they are null before you try to save them.");
                return;
            }

            string serializedJson = JsonConvert.SerializeObject(value, m_Settings);

            try
            {
                Write(valueName, serializedJson);
            }
            catch (Exception e)
            {
                if (Logger != null)
                {
                    Logger.Add(FluidLogCategory.Fail, string.Format("Error while saving data: {0}", e.Message));
                }
            }
        }

        /// <summary>
        /// Loads data from storage
        /// </summary>
        /// <typeparam name="T">The type of data</typeparam>
        /// <param name="valueName">The value's name</param>
        /// <returns>The type if loaded; otherwise the type default</returns>
        public override T Load<T>(string valueName)
        {
            string loadedData = null;
            try
            {
                loadedData = ReadValue(valueName);
            }
            catch (Exception e)
            {
                if (Logger != null)
                {
                    Logger.Add(FluidLogCategory.Fail, string.Format("Error while loaded data: {0}", e.Message));
                }

                return default(T);
            }

            if (loadedData != null)
            {
                return JsonConvert.DeserializeObject<T>(loadedData, m_Settings);
            }

            return default(T);
        }

        /// <summary>
        /// Handles json parsing errors
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The error</param>
        private void OnError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            e.ErrorContext.Handled = true;
            if (Logger != null)
            {
                Logger.Add(FluidLogCategory.Fail, string.Format("Json parsing error while parsing {0}, {1}", e.ErrorContext.Path, e.ErrorContext.Error.Message));
            }
        }

        /// <summary>
        /// Creates a json storage provider
        /// </summary>
        /// <param name="directory">The storage directory</param>
        public JsonStorageProvider(string directory)
            : base(directory)
        {
            m_Settings = new JsonSerializerSettings()
            {
                Error = OnError
            };
        }

        /// <summary>
        /// Creates a json storage provider
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="directory">The directory</param>
        public JsonStorageProvider(FluidLog logger, string directory)
            : this(directory)
        {
            Logger = logger;
            
        }
    }
}
