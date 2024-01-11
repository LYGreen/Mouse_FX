using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Mouse_FX
{
    /// <summary>
    /// 粒子发射器
    /// </summary>
    partial class ParticleSpawner
    {
        public double x;/* 粒子发射器的位置 */
        public double y;/* 粒子发射器的位置 */
        public double Maxdx;/* x 方向的最大速度 */
        public double Mindx;/* x 方向的最小速度 */
        public double Maxdy;/* y 方向的最大速度 */
        public double Mindy;/* y 方向的最小速度 */
        public double MaxLife;/* 粒子最大持续时间 */
        public double MinLife;/* 粒子最小持续时间 */
        public double MinAlpha;/* 最小透明度 */
        public double MaxAlpha;/* 最大透明度 */
        public double MinSize;
        public double MaxSize;
        public int MaxCount;/* 该粒子发射器的最大粒子数 */
        public Color particleColor;/* 粒子的颜色 */
        public bool RandomColor = true;/* 为 true 时，粒子的颜色变成随机颜色 */
        private List<Particle> particles = new List<Particle>();/* 该粒子发射器的粒子 */
        private Canvas canvas;/* 该粒子发射器所在的画布 */
        public List<Particle> GetParticles()
        {
            return particles;
        }
        public ParticleSpawner(/* 粒子发射器构造函数 */
            Canvas can, 
            Color color,
            int maxCount,
            double posx,
            double posy,
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
            canvas = can;
            particleColor = color;
            x = posx;
            y = posy;
            Mindx = mindx;
            Maxdx = maxdx;
            Mindy = mindy;
            Maxdy = maxdy;
            MinLife = minLife;
            MaxLife = maxLife;
            MinAlpha = minAlpha;
            MaxAlpha = maxAlpha;
            MaxCount = maxCount;
            MinSize = minSize;
            MaxSize = maxSize;


            /* 可删除 */
            //for(int i = 0;i<maxCount;i++)
            //{
            //    Particle p = new Particle(particleColor, x, y, Mindx, Maxdx, Mindy, Maxdy, MinLife, MaxLife);
            //    canvas.Children.Add(p.ellipse);
            //    particles.Add(p);
            //}

        }
        /// <summary>
        /// 播放粒子动画
        /// </summary>
        /// <param name="delta">当前帧与上一帧的时间变化量 </param>
        public void PlayParticles(double delta)
        {
            /* 删除life <= 0 的粒子 */
            for(int i = 0;i<particles.Count;i++)
            {
                if (particles[i].life <= 0)
                {
                    canvas.Children.Remove(particles[i].ellipse);
                    particles.RemoveAt(i);
                    i--;
                }
            }
            /* 补充粒子 */
            while(particles.Count < MaxCount)
            {
                if (RandomColor)/* RandomColor 启用，则无视 particleColor 变量 */
                {
                    Particle p = new Particle(
                        x, y, Mindx, Maxdx, Mindy, Maxdy, MinLife, MaxLife, MinAlpha, MaxAlpha, MinSize, MaxSize
                        );
                    canvas.Children.Add(p.ellipse);
                    particles.Add(p);
                }
                else /* RandomColor 禁用 */
                {
                    Particle p = new Particle(
                        particleColor, x, y, Mindx, Maxdx, Mindy, Maxdy, MinLife, MaxLife, MinAlpha, MaxAlpha, MinSize, MaxSize
                        );
                    canvas.Children.Add(p.ellipse);
                    particles.Add(p);
                }
            }

            foreach(var p in particles)
            {
                p.x += p.dx * delta;
                p.y += p.dy * delta;
                p.life -= delta;

                p.ellipse.Margin = new Thickness(p.x, p.y, 0, 0);

            }
        }
    }
    /// <summary>
    /// 粒子
    /// </summary>
    partial class Particle
    {
        public double x;/* x 坐标 */
        public double y;/* y 坐标 */
        public double dx;/* x 轴上的速度 */
        public double dy;/* y 轴上的速度 */
        public double life;/* 粒子持续时间 */
        public Ellipse ellipse;/* 该粒子对应的图形(椭圆) */
        /// <summary>
        /// 粒子的构造函数
        /// </summary>
        /// <param name="posx">粒子的 x 坐标</param>
        /// <param name="posy">粒子的 y 坐标</param>
        public Particle(/* 粒子的构造函数 */
            Color color,
            double posx,
            double posy,
            double mindx,
            double maxdx,
            double mindy,
            double maxdy,
            double minlife,
            double maxlife,
            double minAlpha,
            double maxAlpha,
            double minSize,
            double maxSize
            )
        {
            Random r = new Random();/* r.NextDouble() 的值为 0~1 */
            x = posx;
            y = posy;
            dx = r.NextDouble() * (maxdx - mindx) + mindx;/* mindx ~ maxdx */
            dy = r.NextDouble() * (maxdy - mindy) + mindy;/* mindy ~ maxdy */
            ellipse = new Ellipse()
            {
                Fill = new SolidColorBrush(color),
                Width = r.NextDouble() * (maxSize - minSize) + minSize,
                Height = r.NextDouble() * (maxSize - minSize) + minSize,
                Margin = new Thickness(x, y, 0, 0),
                Opacity = r.NextDouble() * (maxAlpha - minAlpha) + minAlpha,/* minAlpha ~ maxAlpha */
            };
            life = r.NextDouble() * (maxlife - minlife) + minlife;/* minlife ~ maxlife */
        }

        /// <summary>
        /// 粒子的构造函数(随机颜色)
        /// </summary>
        public Particle(/* 粒子的构造函数(随机颜色) */
            double posx,
            double posy,
            double mindx,
            double maxdx,
            double mindy,
            double maxdy,
            double minlife,
            double maxlife,
            double minAlpha,
            double maxAlpha,
            double minSize,
            double maxSize
            )
        {
            Random r = new Random();/* r.NextDouble() 的值为 0~1 */
            x = posx;
            y = posy;
            dx = r.NextDouble() * (maxdx - mindx) + mindx;/* mindx ~ maxdx */
            dy = r.NextDouble() * (maxdy - mindy) + mindy;/* mindy ~ maxdy */
            byte red = Convert.ToByte(r.Next(0, 255));
            byte green = Convert.ToByte(r.Next(0, 255));
            byte blue = Convert.ToByte(r.Next(0, 255));
            ellipse = new Ellipse()
            {

                Fill = new SolidColorBrush(Color.FromRgb(red, green, blue)),
                Width = r.NextDouble() * (maxSize - minSize) + minSize,
                Height = r.NextDouble() * (maxSize - minSize) + minSize,
                Margin = new Thickness(x, y, 0, 0),
                Opacity = r.NextDouble() * (maxAlpha - minAlpha) + minAlpha,/* minAlpha ~ maxAlpha */
            };
            life = r.NextDouble() * (maxlife - minlife) + minlife;/* minlife ~ maxlife */
        }
    }
}
