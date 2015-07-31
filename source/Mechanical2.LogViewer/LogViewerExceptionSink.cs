using System;
using Mechanical.Bootstrap;
using Mechanical.Core;

namespace Mechanical.LogViewer
{
    /// <summary>
    /// Uses the exception details window of the log viewer, to display the exception caught.
    /// </summary>
    public class LogViewerExceptionSink : IExceptionSink
    {
        /// <summary>
        /// Processes unhandled exceptions.
        /// </summary>
        /// <param name="exception">The unhandled exception to process.</param>
        public void Handle( Exception exception )
        {
            if( exception.NullReference() )
                return;

            LogViewerGUI.ShowException(exception);
        }
    }

    //// TODO: display log up to this point
    //// TODO: option to break, continue, terminate (un-cancelable shut down event?)
}
