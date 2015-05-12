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
        #region ShowKeyValue

        /// <summary>
        /// Displays a key-value pair.
        /// </summary>
        /// <param name="pairs">The key-value pairs to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowKeyValue( params KeyValuePair<string, string>[] pairs )
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

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="ExceptionInfo"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowException( ExceptionInfo exception )
        {
            return UI.InvokeAsync(() =>
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
            });
        }

        /// <summary>
        /// Displays the specified <paramref name="exception"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to show.</param>
        /// <returns>An object representing the asynchronous operation.</returns>
        public static Task ShowException( Exception exception )
        {
            return ShowException(exception.NullReference() ? null : ExceptionInfo.From(exception));
        }

        #endregion
    }
}
