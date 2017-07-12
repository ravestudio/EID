using EID.Library;
using EIDClient.Core.Entities;
using EIDClient.Core.ISS;
using EIDClient.Core.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EIDClient.Core.Controls
{
    public sealed partial class ChartControl : UserControl
    {

        private int count = 0;
        double k = 0;
        double cutvalue = 0;

        int frameCount = 0;

        public ChartControl()
        {
            this.InitializeComponent();

            this.SizeChanged += ChartControl_SizeChanged;

            this.canvas.Background = new SolidColorBrush(Color.FromArgb(100, 24, 100, 172));

            this.macd_canvas.Background = new SolidColorBrush(Colors.Gray);

        }

        private void ChartControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            frameCount = (int)e.NewSize.Width / 15;
        }

        public void ShowCandles(IList<ICandle> candleList)
        {

            IList<ICandle> list = candleList.Skip(candleList.Count - frameCount).ToList();

            canvas.Children.Clear();

            count = list.Count;

            decimal max = Math.Max(list.Max(c => c.open), list.Max(c => c.close));
            decimal min = Math.Min(list.Min(c => c.open), list.Min(c => c.close));

            double l = (double)list.Max(c => Math.Abs(c.open - c.close));

            //коридор
            double y = (double)(max - min);

            k = 200 / y;

            cutvalue = (double)min;

            int i = 0;
            foreach (Candle candle in list)
            {
                i++;
                DrawItem(candle, i, k, cutvalue);
            }
        }

        void DrawItem(Candle item, int i, double k, double cutvalue)
        {
            Rectangle rect = new Rectangle()
            {
                Width = 8,
                Height = (double)Math.Abs(item.open - item.close) * k,
                Fill = item.close >= item.open ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.HotPink),
                Stroke = item.close > item.open ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft(rect, i * 10);
            double y = item.close >= item.open ? (double)item.close : (double)item.open;

            Canvas.SetTop(rect, 200 - getDiff(item) * k);
            canvas.Children.Add(rect);
        }

        private double getDiff(Candle item)
        {

            decimal Height = Math.Abs(item.open - item.close);

            double res = item.close >= item.open ? (double)Height + (double)item.open : (double)item.open;

            res -= cutvalue;

            return res;
        }

        public void ShowMA(IList<decimal> ma, Color color)
        {
            IList<decimal> list = ma.Skip(ma.Count - frameCount).ToList();

            Windows.UI.Xaml.Shapes.Polyline poliline = new Polyline();
            poliline.Stroke = new SolidColorBrush(color);
            poliline.StrokeThickness = 2;

            int i = count - list.Count;
            foreach (decimal p in list)
            {
                i++;
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, ((double)p - cutvalue) * k*-1));
            }

            Canvas.SetLeft(poliline, 0);
            Canvas.SetTop(poliline, 200);

            canvas.Children.Add(poliline);
        }

        public void ShowMACD(MACD macdSource, Color color)
        {
            int km = -20;

            IList<MACDItem> list = macdSource.Skip(macdSource.Count - frameCount).ToList();
            MACD macd = new MACD(list);

            Windows.UI.Xaml.Shapes.Polyline poliline = new Polyline();
            poliline.Stroke = new SolidColorBrush(color);
            poliline.StrokeThickness = 2;

            Windows.UI.Xaml.Shapes.Polyline signal = new Polyline();
            signal.Stroke = new SolidColorBrush(Colors.Red);
            signal.StrokeThickness = 2;


            int i = count - macd.Count;
            foreach (MACDItem item in macd)
            {
                i++;
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, ((double)item.MACD) * km));
                signal.Points.Add(new Point(i * 10, ((double)item.Signal) * km));

                Line line = new Line()
                {
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Red)
                };

                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = ((double)item.Histogram) * km;

                Canvas.SetLeft(line, i * 10);
                Canvas.SetTop(line, 100);

                macd_canvas.Children.Add(line);
            }

            Canvas.SetLeft(poliline, 0);
            Canvas.SetTop(poliline, 100);

            Canvas.SetLeft(signal, 0);
            Canvas.SetTop(signal, 100);

            //Canvas.SetLeft(histogram, 0);
            //Canvas.SetTop(histogram, 400);

            macd_canvas.Children.Add(poliline);
            macd_canvas.Children.Add(signal);
            //canvas.Children.Add(histogram);

        }
    }
}
    
