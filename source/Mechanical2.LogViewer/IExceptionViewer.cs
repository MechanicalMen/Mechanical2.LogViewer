using System;
using System.Threading.Tasks;

namespace Mechanical.LogViewer
{
    /// <summary>
    /// Presents information about the specified exception to the user.
    /// </summary>
    public interface IExceptionViewer
    {
        /// <summary>
        /// Displays the specified exception for the user.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        Task ShowAsync( Exception exception );
    }
}
