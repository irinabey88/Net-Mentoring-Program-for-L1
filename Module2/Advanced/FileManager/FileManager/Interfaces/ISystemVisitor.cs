namespace FileManager.Interfaces
{
    /// <summary>
    /// Represents an <see cref="ISystemVisitor"/> interface.
    /// </summary>
    public interface ISystemVisitor
    {
        /// <summary>
        /// Search files or directories by given path.
        /// </summary>
        /// <param name="path">The path for search.</param>
        void Search(string path);
    }
}