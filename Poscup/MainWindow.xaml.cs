using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Poscup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            posterText.FontSize = _maxSize;
            this.SizeChanged += new SizeChangedEventHandler(sizeChangedHandler);
            inputField.TextChanged += new TextChangedEventHandler(inputFieldHandler);
            inputField.SelectionChanged += new RoutedEventHandler(inputFieldSelChangedHandler);
            inputField.Focus();
        }

        private int _lineId = 0;
        private double _minSize = 32.0;
        private double _maxSize = 200.0;
        private double _degradeGap = 8.0;

        void sizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            container.Margin = new Thickness(Math.Max(this.ActualWidth * 0.05, 30));
        }

        void inputFieldHandler(object sender, EventArgs e)
        {
            UpdatePosterText();
        }

        void inputFieldSelChangedHandler(object sender, RoutedEventArgs e)
        {
            int lineId = inputField.GetLineIndexFromCharacterIndex(inputField.SelectionStart);
            if (lineId == _lineId) return;

            _lineId = lineId;
            UpdatePosterText();
        }

        void UpdatePosterText()
        {
            _lineId = Math.Min(_lineId, inputField.LineCount - 1);
            string text = inputField.GetLineText(_lineId);

            bool bold = text.StartsWith("**");
            bool italic = text.StartsWith("__");
            if (bold || italic) text = text.Substring(2);

            if (posterText.Text == text) return;

            bool scalingUp = (posterText.Text.Length - text.Length) * 1 > 0;
            posterText.Text = text;
            posterText.FontStyle = (italic) ? FontStyles.Italic : FontStyles.Normal;
            posterText.FontWeight = (bold) ? FontWeights.Bold : FontWeights.Normal;

            if (scalingUp)
            {
                while ((posterText.ActualWidth < container.ActualWidth) &&
                       (posterText.ActualHeight < container.ActualHeight))
                    if (posterText.FontSize > _maxSize) break;
                    else
                    {
                        posterText.FontSize += _degradeGap;
                        posterText.UpdateLayout();
                    }
                posterText.FontSize -= _degradeGap;
            }
            else
            {
                while ((posterText.ActualWidth >= container.ActualWidth) ||
                       (posterText.ActualHeight >= container.ActualHeight))
                    if (posterText.FontSize < _minSize) break;
                    else
                    {
                        posterText.FontSize -= _degradeGap;
                        posterText.UpdateLayout();
                    }
            }
        }
    }
}
