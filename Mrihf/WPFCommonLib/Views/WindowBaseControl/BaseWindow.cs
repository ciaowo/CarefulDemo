using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using CommonLib;

namespace WPFCommonLib.Views.WindowBaseControl
{

    public class BaseWindow : Window, INotifyPropertyChanged
    {
        ResourceDictionary style1;
        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }
        public BaseWindow()
        {

            style1 = new ResourceDictionary();
            style1.Source = new Uri("WPFCommonLib;component/Views/WindowBaseControl/BaseWindowStyle.xaml", UriKind.Relative);
            this.Style = (System.Windows.Style)style1["BaseWindowStyle"];

            //style1 = this.Style.Resources;

            this.DataContext = this;
            this.IsChildWindow = false;
            this.Closing += BaseWindow_Closing;
            this.StateChanged += BaseWindow_StateChanged;
        }


        private void BaseWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.MinWidth = 1015;
            }
        }

        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!IsChildWindow)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                e.Cancel = true;
            }
            
            return;
        }


        public bool IsChildWindow
        {
            get { return (bool)GetValue(IsChildWindowProperty); }
            set { SetValue(IsChildWindowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChildWindow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildWindowProperty =
            DependencyProperty.Register("IsChildWindow", typeof(bool), typeof(BaseWindow));


        #region 遮罩
        public bool IsMask
        {
            get { return (bool)GetValue(IsMaskProperty); }
            set { SetValue(IsMaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsMask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMaskProperty =
            DependencyProperty.Register("IsMask", typeof(bool), typeof(BaseWindow), new PropertyMetadata(OnIsMaskChanged));

        static Border border = null;
        private static void OnIsMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (border == null)
                return;
            if ((e.NewValue as bool?) == true)
            {
                border.Visibility = Visibility.Visible;
            }
            else
            {
                border.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            if (!IsChildWindow)
                InitializeEvent();
        }

        WindowState ws = WindowState.Normal;
        Rect rcnormal = new Rect();
        /// <summary>
        /// 初始化主窗体功能按钮事件加载
        /// </summary>
        private void InitializeEvent()
        {
            this.MouseMove += BaseWindow_MouseMove;
            ControlTemplate baseWindowTemplate = (ControlTemplate)style1["BaseWindowControlTemplate"];

            #region 窗体操作

            Button minBtn = (Button)baseWindowTemplate.FindName("btnMin", this);
            minBtn.Click += delegate
            {
                this.WindowState = WindowState.Minimized;
            };
            //Button maxBtn = (Button)baseWindowTemplate.FindName("btnMax", this);

            Grid grid = (Grid)baseWindowTemplate.FindName("grid", this);
            //maxBtn.Click += delegate
            //{

            //    if (ws == WindowState.Maximized)
            //    {
            //        ws = WindowState.Normal;
            //        this.Left = rcnormal.Left;
            //        this.Top = rcnormal.Top;
            //        this.Width = rcnormal.Width;
            //        this.Height = rcnormal.Height;
            //        grid.Margin = new Thickness(10);
            //        this.ResizeMode = System.Windows.ResizeMode.CanResize;
            //    }
            //    else
            //    {
            //        ws = WindowState.Maximized;
            //        rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
            //        Rect rc = SystemParameters.WorkArea;//获取工作区大小
            //                                            //获取任务栏停靠位置
            //        int width, height;
            //        Windows32Operation.WindowsLocation location = Windows32Operation.GetWindowsBarLocation(out width, out height);
            //        switch (location)
            //        {
            //            case Windows32Operation.WindowsLocation.bottom:
            //                this.Left = 0;//设置位置
            //                this.Top = 0;
            //                this.Width = rc.Width;
            //                this.Height = rc.Height;
            //                break;
            //            case Windows32Operation.WindowsLocation.left:
            //                this.Left = width;
            //                this.Top = 0;
            //                this.Width = rc.Width;
            //                this.Height = rc.Height;
            //                break;
            //            case Windows32Operation.WindowsLocation.right:
            //                this.Left = 0;//设置位置
            //                this.Top = 0;
            //                this.Width = rc.Width;
            //                this.Height = rc.Height;
            //                break;
            //            case Windows32Operation.WindowsLocation.top:
            //                this.Left = 0;
            //                this.Top = height;
            //                this.Width = rc.Width;
            //                this.Height = rc.Height;
            //                break;
            //        }
            //        grid.Margin = new Thickness(0);
            //        this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //    }
            //};

            Button btnClose = (Button)baseWindowTemplate.FindName("btnClose", this);
            btnClose.Click += delegate
            {
                //this.Close();


                Application.Current.Shutdown();
            };

            Border borderTitle = (Border)baseWindowTemplate.FindName("borderTitle", this);

            borderTitle.MouseMove += delegate (object sender, MouseEventArgs e)
            {
                WindowMove(e);
            };
            borderTitle.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
            {
                WindowMouseLeftDown(e);
            };

            #endregion
        }


        public void WindowMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //this.WindowState = WindowState.Normal;
                if (ws != WindowState.Maximized)
                        this.DragMove();
                //NativeMethods.SetWindowRectanle(this);
            }
        }
        public void WindowMouseLeftDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (ws == WindowState.Maximized)
                {
                    ws = WindowState.Normal;
                    this.Left = rcnormal.Left;
                    this.Top = rcnormal.Top;
                    this.Width = rcnormal.Width;
                    this.Height = rcnormal.Height;
                    this.ResizeMode = System.Windows.ResizeMode.CanResize;
                }
                else
                {
                    ws = WindowState.Maximized;
                    rcnormal = new Rect(this.Left, this.Top, this.Width, this.Height);//保存下当前位置与大小
                    Rect rc = SystemParameters.WorkArea;//获取工作区大小
                                                        //获取任务栏停靠位置
                    int width, height;
                    Windows32Operation.WindowsLocation location = Windows32Operation.GetWindowsBarLocation(out width, out height);
                    switch (location)
                    {
                        case Windows32Operation.WindowsLocation.bottom:
                            this.Left = 0;//设置位置
                            this.Top = 0;
                            this.Width = rc.Width;
                            this.Height = rc.Height;
                            break;
                        case Windows32Operation.WindowsLocation.left:
                            this.Left = width;
                            this.Top = 0;
                            this.Width = rc.Width;
                            this.Height = rc.Height;
                            break;
                        case Windows32Operation.WindowsLocation.right:
                            this.Left = 0;//设置位置
                            this.Top = 0;
                            this.Width = rc.Width;
                            this.Height = rc.Height;
                            break;
                        case Windows32Operation.WindowsLocation.top:
                            this.Left = 0;
                            this.Top = height;
                            this.Width = rc.Width;
                            this.Height = rc.Height;
                            break;
                    }
                    this.ResizeMode = System.Windows.ResizeMode.NoResize;
                }
                //NativeMethods.SetWindowRectanle(this);
            }
        }

        #region 窗体拖拽缩放

        private HwndSource hwndSource;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public void BaseWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.ResizeMode == ResizeMode.NoResize)
                return;
            FrameworkElement fe = e.OriginalSource as FrameworkElement;
            if (fe == null)
                return;
            
            string strTemp = fe.Name;
            switch (strTemp)
            {
                case "topRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61443), IntPtr.Zero);
                    break;
                case "botRec":
                    this.Cursor = Cursors.SizeNS;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61446), IntPtr.Zero);
                    break;
                case "rightRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61442), IntPtr.Zero);
                    break;
                case "leftRec":
                    this.Cursor = Cursors.SizeWE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61441), IntPtr.Zero);
                    break;
                case "nwseUpRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61444), IntPtr.Zero);
                    break;
                case "nwseDownRec":
                    this.Cursor = Cursors.SizeNWSE;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61448), IntPtr.Zero);
                    break;
                case "neswDownRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61447), IntPtr.Zero);
                    break;
                case "neswUpRec":
                    this.Cursor = Cursors.SizeNESW;
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                        SendMessage(hwndSource.Handle, 0x112, (IntPtr)(61445), IntPtr.Zero);
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;

            }
        }

        #endregion
        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}
