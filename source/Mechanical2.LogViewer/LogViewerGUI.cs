using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mechanical.Collections;
using Mechanical.Core;
using Mechanical.DataStores;
using Mechanical.LogViewer.ViewModels;
using Mechanical.LogViewer.Views;
using Mechanical.MVVM;

namespace Mechanical.LogViewer
{
    /// <summary>
    /// Provides access to the most common functionalities.
    /// </summary>
    public static class LogViewerGUI
    {
        #region ShowKeyValues

        /// <summary>
        /// Displays key-value pairs.
        /// </summary>
        /// <param name="pairs">The key-value pairs to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowKeyValuesAsync( params KeyValuePair<string, string>[] pairs )
        {
            return UI.InvokeAsync(() =>
            {
                if( !pairs.NullOrEmpty() )
                {
                    var kvvms = pairs.Select(p => new KeyValueViewModel(p.Key, p.Value)).ToArray();
                    using( var vm = new ExceptionViewModel(kvvms) )
                    {
                        var wnd = new ExceptionWindow();
                        wnd.DataContext = vm;
                        if( kvvms.Length == 1 )
                            wnd.OpenFlyout(kvvms[0]);
                        wnd.ShowDialog();
                    }
                }
            });
        }

        #endregion

        #region ShowException

        private static void ShowExceptionCore( ExceptionInfo exception )
        {
            if( exception.NotNullReference() )
            {
                using( var vm = ExceptionViewModel.From(exception) )
                {
                    var wnd = new ExceptionWindow();
                    wnd.DataContext = vm;
                    wnd.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="ExceptionInfo"/> to show.</param>
        public static void ShowException( ExceptionInfo exception )
        {
            UI.Invoke(() => ShowExceptionCore(exception));
        }

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="ExceptionInfo"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowExceptionAsync( ExceptionInfo exception )
        {
            return UI.InvokeAsync(() => ShowExceptionCore(exception));
        }

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to show.</param>
        public static void ShowException( Exception exception )
        {
            ShowException(exception.NullReference() ? null : ExceptionInfo.From(exception));
        }

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowExceptionAsync( Exception exception )
        {
            return ShowExceptionAsync(exception.NullReference() ? null : ExceptionInfo.From(exception));
        }

        #endregion

        //// TODO: display log entries
    }
}
