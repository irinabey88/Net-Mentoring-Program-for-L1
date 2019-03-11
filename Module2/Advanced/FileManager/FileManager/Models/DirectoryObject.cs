namespace FileManager.Models
{
    /// <summary>
    /// Represents a <see cref="DirectoryObject"/>.
    /// </summary>
    public class DirectoryObject : FileSystemObject
    {
        /// <summary>
        /// Get short representation of <see cref="DirectoryObject"/>
        /// </summary>
        /// <returns>Short representation of type <see cref="DirectoryObject"/></returns>
        public override string GetFileSystemType() => "-D-";
    }
}