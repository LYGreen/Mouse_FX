using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mouse_FX
{
    /// <summary>
    /// ParticlesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParticlesWindow : Window
    {
        ParticleSpawner particleSpawner;
        bool isPlaying = false;
        public ParticlesWindow()
        {
            InitializeComponent();
            ShowInTaskbar = false;/* 在任务栏中不显示该窗口 */

            /* 设置粒子窗口置顶+鼠标穿透 */
            this.Show();/* 显示了窗口之后才能设置窗口穿透 */
            this.Topmost = true;/* 总是显示在最前面 */
            IntPtr handle = new WindowInteropHelper(this).Handle;/* 当前窗口句柄 */
            Winapi.SetWindowLongW(handle, Winapi.GWL_EXSTYLE, Winapi.WS_EX_TRANSPARENT);/* 设置窗口穿透 */

            /* 粒子窗口位置&大小 */
            Left = 0;
            Top = 0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            /* 用于创建粒子发射器 */
            Winapi.POINT p = new Winapi.POINT();
            Winapi.GetCursorPos(ref p);
            DpiScale ds = VisualTreeHelper.GetDpi(this);
            /***** 构造函数中的参数为默认值 *****/
            particleSpawner = new ParticleSpawner(canvas1, Color.FromRgb(
                0,192,0), 320, p.x, p.y, -8, 8, -8, 8, 0, 1, 0.5, 1, 3, 3
                );

            /* 新建线程，用于粒子的移动 */
            isPlaying = true;
            Thread t = new Thread(new ThreadStart(ThreadLoop));
            t.Start();

        }
        /// <summary>
        /// 线程函数，用于播放动画
        /// </summary>
        void ThreadLoop()
        {
            double lastTick = Environment.TickCount / 1000.0;
            Winapi.POINT p = new Winapi.POINT();
            Winapi.GetCursorPos(ref p);
            DpiScale ds = VisualTreeHelper.GetDpi(this);
            while (isPlaying)
            {
                Thread.Sleep(1);
                Dispatcher.Invoke(new Action(() =>
                {

                   Winapi.POINT p = new Winapi.POINT();
                   Winapi.GetCursorPos(ref p);
                   DpiScale ds = VisualTreeHelper.GetDpi(this);

                   particleSpawner.x = p.x / ds.DpiScaleX;
                   particleSpawner.y = p.y / ds.DpiScaleY;

                   particleSpawner.PlayParticles(Environment.TickCount / 1000.0 - lastTick);
                
                   lastTick = Environment.TickCount / 1000.0;
                }));
            }
        }

        /* 设置粒子发射器属性函数 */
        public void SetParticleSpawnValues(/* 设置粒子发射器属性 */
            Color color, 
            int maxCount, 
            double mindx, 
            double maxdx,
            double mindy,
            double maxdy,
            double minLife,
            double maxLife,
            double minAlpha,
            double maxAlpha,
            double minSize,
            double maxSize
            )
        {
            particleSpawner.particleColor = color;
            particleSpawner.MaxCount = maxCount;
            particleSpawner.Mindx = mindx;
            particleSpawner.Maxdx = maxdx;
            particleSpawner.Mindy = mindy;
            particleSpawner.Maxdy = maxdy;
            particleSpawner.MinLife = minLife;
            particleSpawner.MaxLife = maxLife;
            particleSpawner.MinAlpha = minAlpha;
            particleSpawner.MaxAlpha = maxAlpha;
            particleSpawner.MinSize = minSize;
            particleSpawner.MaxSize = maxSize;
        }
        public void SetParticleSpawnRandomColor(bool randomColor)
        {
            particleSpawner.RandomColor = randomColor;
        }
    }
}
