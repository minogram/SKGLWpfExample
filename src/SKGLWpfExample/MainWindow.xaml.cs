using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using SkiaSharp;

namespace SKGLWpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch _sw = new Stopwatch();

        List<SKPoint> _points = new List<SKPoint>();

        public MainWindow()
        {
            InitializeComponent();

            var r = new Random();

            for (int i = 0; i < 2000; i++)
            {
                _points.Add(new SKPoint((float)(r.NextDouble() * 2.0 - 1.0),
                                        (float)(r.NextDouble() * 2.0 - 1.0)));
            }
        }

        private void SKElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var width = e.Info.Width;
            var height = e.Info.Height;

            var ske = sender as SKElement;

            if (ske.Visibility == Visibility.Visible)
                Render(canvas, width, height, nameof(SKElement));
        }

        private void sk_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var width = e.BackendRenderTarget.Width;
            var height = e.BackendRenderTarget.Height;

            var skglwpf = sender as SKGLWpfControl.SKGLWpfControl;

            if (skglwpf.Visibility == Visibility.Visible)
                Render(canvas, width, height, nameof(SKGLWpfControl.SKGLWpfControl));
        }

        private void Render(SKCanvas canvas, int width, int height, string label)
        {
            var deltaT = _sw.ElapsedMilliseconds;

            _sw.Restart();

            var scale = (float)PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice.M11;
            var size = new SKSizeI((int)(width / scale), (int)(height / scale));

            canvas.Scale(scale);

            canvas.Clear(SKColors.White);

            float ox = size.Width / 2f;
            float oy = size.Height / 2f;

            if (_points.Count > 1)
            {
                using (SKPaint skPaint = new SKPaint())
                {
                    skPaint.Style = SKPaintStyle.Stroke;
                    skPaint.IsAntialias = true;
                    skPaint.Color = SKColors.DarkKhaki;
                    skPaint.StrokeWidth = 2f;
                    skPaint.StrokeCap = SKStrokeCap.Round;

                    SKPoint pmsg = _points[0];

                    float mpx = -pmsg.X;
                    float mpy = -pmsg.Y;

                    for (int i = 1; i < _points.Count; i++)
                    {
                        SKPoint msg = _points[i];

                        mpx = -pmsg.X;
                        mpy = -pmsg.Y;

                        float mx = -msg.X;
                        float my = -msg.Y;

                        float px = ox + mpx * ox;
                        float py = oy + mpy * oy;

                        float x = ox + mx * ox;
                        float y = oy + my * oy;

                        canvas.DrawLine(px, py, x, y, skPaint);

                        pmsg = msg;
                    }
                }
            }

            using (var skPaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Fill,
            })
            {
                canvas.DrawRect(0, size.Height - 20, size.Width, size.Height, skPaint);
            }

            using (var skPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal),
                TextAlign = SKTextAlign.Left,
                TextSize = 14
            })
            {
                var statsCoords = new SKPoint(0, size.Height - 5);

                canvas.DrawText($"Lines =  {_points.Count - 1}, DrawTime = {_sw.ElapsedMilliseconds} ms., RenderDeltaT = {deltaT} ms., FPS = {1000.0 / deltaT: 0.000}", statsCoords, skPaint);
            }

            using (var skPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal),
                TextAlign = SKTextAlign.Center,
                TextSize = 28
            })
            {
                canvas.DrawText(label, new SKPoint(ox, oy), skPaint);
            }

            _sw.Restart();
        }
    }
}
