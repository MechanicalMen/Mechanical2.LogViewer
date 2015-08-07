using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Mechanical.Common;
using Mechanical.Conditions;
using Mechanical.Core;
using Mechanical.LogViewer.ViewModels;
using Mechanical.MVVM;

namespace Mechanical.LogViewer.Views
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "XAML is correctly spelled.")]
    public partial class ExceptionWindow : MahApps.Metro.Controls.MetroWindow
    {
        public ExceptionWindow()
        {
            this.InitializeComponent();
        }

        private ExceptionViewModel VM
        {
            get
            {
                ExceptionViewModel result = null;
                UI.Invoke(() => result = (ExceptionViewModel)this.DataContext);
                return result;
            }
        }

        private void TreeView_SelectedItemChanged( object sender, RoutedPropertyChangedEventArgs<object> e )
        {
            try
            {
                var treeView = (TreeView)sender;
                var keyValueVM = (KeyValueViewModel)treeView.SelectedItem;
                this.VM.SelectedNode = keyValueVM; // may be null (technically)
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        private void TreeView_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            try
            {
                this.flyout.IsOpen = true;
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        public void OpenFlyout( KeyValueViewModel itemToShow )
        {
            Ensure.That(itemToShow).NotNull();

            try
            {
                this.VM.SelectedNode = itemToShow;
                this.flyout.IsOpen = true;
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        private void MetroWindow_SizeChanged( object sender, SizeChangedEventArgs e )
        {
            try
            {
                this.flyout.Width = Math.Round(this.ActualWidth * 0.66d);
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }

        private void TextWrappingToggleSwitch_IsCheckedChanged( object sender, EventArgs e )
        {
            try
            {
                this.flyoutTextBox.TextWrapping = ((MahApps.Metro.Controls.ToggleSwitch)sender).IsChecked.Value ? TextWrapping.Wrap : TextWrapping.NoWrap;
            }
            catch( Exception ex )
            {
                MechanicalApp.HandleException(ex);
            }
        }
    }
}
