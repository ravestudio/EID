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

namespace EIDClient.Views.Controls
{
public sealed partial class ChartControl : UserControl
    {

        private int count = 0;
        double k = 0;
        double cutvalue = 0;

        public ChartControl()
        {
            this.InitializeComponent();
        }

        public void ShowCandles(IList<ICandle> candleList)
        {
            canvas.Children.Clear();

            count = candleList.Count;

            decimal max = Math.Max(candleList.Max(c => c.open), candleList.Max(c => c.close));
            decimal min = Math.Min(candleList.Min(c => c.open), candleList.Min(c => c.close));

            double l = (double)candleList.Max(c => Math.Abs(c.open - c.close));

            //коридор
            double y = (double)(max - min);

            k = 200 / y;

            cutvalue = (double)min;

            int i = 0;
            foreach(Candle candle in candleList)
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
                Height = (double)Math.Abs(item.open - item.close)*k,
                Fill = item.close >= item.open ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.HotPink),
                Stroke = item.close > item.open ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft(rect, i * 10);
            double y = item.close >= item.open ? (double)item.close : (double)item.open;

            Canvas.SetTop(rect, 200 - getDiff(item)*k);
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
            Windows.UI.Xaml.Shapes.Polyline poliline = new Polyline();
            poliline.Stroke = new SolidColorBrush(color);
            poliline.StrokeThickness = 2;
 
            int i = count-ma.Count;
            foreach(decimal p in ma)
            {
                i++;
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, 100-((double)p - cutvalue) * k));
            }

            Canvas.SetLeft(poliline, 0);
            Canvas.SetTop(poliline, 0);

            canvas.Children.Add(poliline);
        }

        public void ShowMACD(MACD macd, Color color)
        {
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
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, ((double)item.MACD)*k*-3));
                signal.Points.Add(new Point(i * 10, ((double)item.Signal) * k*-3));

                Line line = new Line()
                {
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Colors.Red)
                };

                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = ((double)item.Histogram) * k*(-3);

                Canvas.SetLeft(line, i * 10);
                Canvas.SetTop(line, 400);

                canvas.Children.Add(line);
            }

            Canvas.SetLeft(poliline, 0);
            Canvas.SetTop(poliline, 350);

            Canvas.SetLeft(signal, 0);
            Canvas.SetTop(signal, 350);

            //Canvas.SetLeft(histogram, 0);
            //Canvas.SetTop(histogram, 400);

            canvas.Children.Add(poliline);
            canvas.Children.Add(signal);
            //canvas.Children.Add(histogram);

        }
    }
    
