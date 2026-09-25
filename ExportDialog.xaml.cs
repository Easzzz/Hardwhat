using System;
using System.Collections.Generic;
using System.IO;
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
using Hardwhat.Exporters;
using Hardwhat.Models;
using Hardwhat;
using Microsoft.Win32;

namespace Hardwhat
{
    /// <summary>
    /// ExportDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ExportDialog : Window
    {
        private readonly HardwareReport _report;
        public ExportDialog(HardwareReport _hardwareReport)
        {
            InitializeComponent();
            _report = _hardwareReport;
        }

        private void AsTableMd_Click(object sender, RoutedEventArgs e)
        {
            var exporter = new TableMarkdownExporter();
            string content = exporter.Export(_report);
            var dialog = new SaveFileDialog()
            {
                Filter = "Markdown File (*.md)|*.md|All Files (*.*)|*.*",
                FileName = $"Hardwhat_Report_{DateTime.Now:yyyyMMdd_HHmmss}.md"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
                MessageBox.Show($"Exported successfully at: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Export failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AsCsv_Click(object sender, RoutedEventArgs e)
        {
            var exporter = new CsvExporter();
            string content = exporter.Export(_report);
            var dialog = new SaveFileDialog()
            {
                Filter = "CSV File (*.csv)|*.md|All Files (*.*)|*.*",
                FileName = $"Hardwhat_Report_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content, new UTF8Encoding(true));
                MessageBox.Show($"Exported successfully at: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Export failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AsHeadingMarkdown_Click(object sender, RoutedEventArgs e)
        {
            var exporter = new HeadingMarkdownExporter();
            string content = exporter.Export(_report);
            var dialog = new SaveFileDialog()
            {
                Filter = "Markdown File (*.md)|*.md|All Files (*.*)|*.*",
                FileName = $"Hardwhat_Report_{DateTime.Now:yyyyMMdd_HHmmss}.md"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
                MessageBox.Show($"Exported successfully at: {dialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Export failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
