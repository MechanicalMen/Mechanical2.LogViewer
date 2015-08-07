using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mechanical.Common;
using Mechanical.Core;
using Mechanical.Events;
using Mechanical.Logs;
using Mechanical.LogViewer.ViewModels;
using Mechanical.MVVM;

//// TODO: open multiple log files: append to current, or create new tab (ask in MessageBox)
//// TODO: automatically load advanced logger directory (open in tabs, merge where necessary)
//// TODO: string pattern filtering (log message at first) (above datagrid)
//// TODO: toolbar (below datagrid) for opening single of multiple files

namespace Mechanical.LogViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "XAML is correctly spelled.")]
    public partial class MainWindow : MahApps.Metro.Controls.MetroWindow,
                                      IEventHandler<EventQueueClosingEvent>
    {
        #region Construction, Destruction

        public MainWindow()
        {
            // instead of the default logger, we set up a simple, one-file logger
            if( MechanicalApp.InitializeSafeMode() )
            {
                // this is the first time this constructor is running
                var directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                directory = System.IO.Path.GetFullPath(directory);
                MechanicalApp.Log = LogEntrySerializer.ToXmlFile(System.IO.Path.Combine(directory, "LogViewer_log.xml"));
                MechanicalApp.InitializeWindow(App.Current, this, exceptionGuiSink: new Mechanical.Common.Bootstrap.MessageBoxExceptionSink());
            }

            this.InitializeComponent();
        }

        private void MetroWindow_Loaded( object sender, RoutedEventArgs e )
        {
            try
            {
                AppCore.EventQueue.Subscribe<EventQueueShuttingDownEvent>(this);
                this.DataContext = new MainViewModel();
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        public Task Handle( EventQueueShuttingDownEvent evnt, IEventHandlerQueue queue )
        {
            var vm = this.VM;
            if( vm.NotNullReference() )
            {
                UI.Invoke(() => this.DataContext = null);
                vm.Dispose();
            }

            return null;
        }

        #endregion

        private MainViewModel VM
        {
            get
            {
                MainViewModel result = null;
                UI.Invoke(() => result = (MainViewModel)this.DataContext);
                return result;
            }
        }

        private async void DataGrid_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            try
            {
                var dataGrid = (DataGrid)sender;
                var logEntryVM = (MainViewModel.LogEntryVM)dataGrid.SelectedItem;
                if( logEntryVM.NotNullReference()
                 && logEntryVM.LogEntry.Exception.NotNullReference() )
                    await LogViewerGUI.ShowExceptionAsync(logEntryVM.LogEntry.Exception);
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        private void MetroWindow_Drop( object sender, DragEventArgs e )
        {
            if( e.Data.GetDataPresent(DataFormats.FileDrop) )
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //// TODO: nag to the user about multiple files (or zero?!)

                this.VM.LoadXmlFile(files[0]);
            }
        }
    }
}
