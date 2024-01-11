using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
/* 解决 using System.Windows.Forms;报错，打开项目 .csproj 文件，添加文本 <UseWindowsForms>true</UseWindowsForms> */
using System.Windows.Forms;
using System.ComponentModel;

namespace Mouse_FX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotifyIcon notifyIcon = new NotifyIcon();/* 托盘图标 */
        ParticlesWindow pw = new ParticlesWindow();/* 创建粒子窗口 */
        public MainWindow()
        {
            InitializeComponent();
            InitNotifyIcon();
        }

        /* 更新粒子发射器 */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int maxCount = Convert.ToInt32(textbox_MaxCount.Text);
            int r = Convert.ToInt32(textbox_R.Text);
            int g = Convert.ToInt32(textbox_G.Text);
            int b = Convert.ToInt32(textbox_B.Text);
            Color color = Color.FromRgb(Convert.ToByte(r),Convert.ToByte(g),Convert.ToByte(b));
            double mindx = Convert.ToDouble(textbox_Mindx.Text);
            double maxdx = Convert.ToDouble(textbox_Maxdx.Text);
            double mindy = Convert.ToDouble(textbox_Mindy.Text);
            double maxdy = Convert.ToDouble(textbox_Maxdy.Text);
            double minLife = Convert.ToDouble(textbox_MinLife.Text);
            double maxLife = Convert.ToDouble(textbox_MaxLife.Text);
            double minAlpha = Convert.ToDouble(textbox_MinAlpha.Text);
            double maxAlpha = Convert.ToDouble(textbox_MaxAlpha.Text);
            double minSize = Convert.ToDouble(textbox_MinSize.Text);
            double maxSize = Convert.ToDouble(textbox_MaxSize.Text);

            /* 调用函数以设置值 */
            pw.SetParticleSpawnValues(
                color, maxCount, mindx, maxdx, mindy, maxdy, minLife, maxLife, minAlpha, maxAlpha, minSize, maxSize
                );
        }

        private void checkbox_DisplayParticleWindow_Click(object sender, RoutedEventArgs e)
        {
            if (checkbox_DisplayParticleWindow.IsChecked == true)
            {
                pw.Background.Opacity = 0.15;
            }
            else
            {
                pw.Background.Opacity = 0.00;
            }
        }

        private void checkbox_RandomColor_Click(object sender, RoutedEventArgs e)
        {
            if(checkbox_RandomColor.IsChecked == true)
            {
                pw.SetParticleSpawnRandomColor(true);
            }
            else
            {
                pw.SetParticleSpawnRandomColor(false);
            }
        }

        /* 当点击最小化按钮 - 时 */
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        
        /* 当按下关闭按钮 x 时 */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /* 取消关闭，隐藏当前窗口 */
            e.Cancel = true;
            this.Hide();
        }

        #region 托盘图标

        /* 托盘图标初始化 */
        private void InitNotifyIcon()
        {
            /* 托盘各种属性 */
            notifyIcon.Icon = new System.Drawing.Icon("Mouse_FX.ico");
            notifyIcon.Text = "Mouse_FX";
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            notifyIcon.Visible = true;

            /* 托盘菜单栏 */
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            ToolStripItem item_exit = new ToolStripMenuItem("退出(Exit)");/* 添加用于退出的菜单物品 */
            item_exit.Click += (sender, e) => /* 当点击了改菜单物品时 */
            {
                Environment.Exit(0);
            };
            notifyIcon.ContextMenuStrip.Items.Add(item_exit);
        }

        /* 当鼠标点击托盘图标时 */
        private void NotifyIcon_MouseClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)/* 左键点击 */
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
        }



        #endregion


    }
}
