using WPFCommonLib.Views.WindowBaseControl;
using System.Windows.Controls;
using WPFCommonLib.Views.WindowContainerControl;

namespace WPFCommonLib.Views
{
    /// <summary>
    /// FrameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FrameWindow : BaseWindow
    {
        public FrameWindow()
        {
            InitializeComponent();
        }


        public WindowContainer WindowContainer { get { return this.windowContainer; } }
    }
}