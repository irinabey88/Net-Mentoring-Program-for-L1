namespace FileManager.Models
{
    /// <summary>
    /// Represents a <see cref="FileObject"/>.
    /// </summary>
    public class FileObject : FileSystemObject
    {
        /// <summary>
        /// Get short representation of <see cref="FileObject"/>
        /// </summary>
        /// <returns>Short representation of type <see cref="FileObject"/></returns>
        public override string GetFileSystemType() => "-F-";
    }
}