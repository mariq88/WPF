using System;
using System.Windows.Controls;

namespace Spammer.Views
{
    /// <summary>
    /// Interaction logic for ucChart.xaml
    /// </summary>
    public partial class ChartColumnView : UserControl
    {
      private static ChartColumnView _ucCS;
      public ChartColumnView()
        {
            InitializeComponent();
            _ucCS = this;
        }
      public static ChartColumnView getInstance()
      {
          if (_ucCS == null)
          {
              _ucCS = new ChartColumnView();
          }
          return _ucCS;
      }
    }
}
