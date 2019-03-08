namespace FileManager.Models
{
    /// <summary>
    /// Represents a <see cref="FileSystemObject"/>
    /// </summary>
    public abstract class FileSystemObject
    {
        /// <summary>
        /// Gets or sets file system object full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets  file system object short name.
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullName))
                {
                    return string.Empty;
                }

                var pathArray = FullName.Split('\\');

                if (string.IsNullOrWhiteSpace(FullName.Split('\\')[pathArray.Length - 1]))
                {
                    return FullName;
                }

                return FullName.Split('\\')[pathArray.Length - 1];
            }
        }

        /// <summary>
        /// Get short representation of <see cref="FileSystemObject"/>
        /// </summary>
        /// <returns>Short representation of type <see cref="FileSystemObject"/></returns>
        public abstract string GetFileSystemType();
    }
}