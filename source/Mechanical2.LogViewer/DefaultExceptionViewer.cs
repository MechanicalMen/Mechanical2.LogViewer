using System;
using System.Threading.Tasks;
using Mechanical.Conditions;

namespace Mechanical.LogViewer
{
    /// <summary>
    /// Uses a log viewer window to display exceptions.
    /// </summary>
    public class DefaultExceptionViewer : IExceptionViewer
    {
        /// <summary>
        /// Displays the specified exception for the user.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public Task ShowAsync( Exception exception )
        {
            throw new NotImplementedException().StoreFileLine();
        }

        //// TODO: exception sink?!
        //// TODO: exception viewer
        //// TODO: log viewer
        //// TODO: open specified log stream, scroll to last exception, and open it
    }
}
