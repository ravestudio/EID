﻿using EIDClient.Core.ISS;
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
        double div = 0;

        public ChartControl()
        {
            this.InitializeComponent();
        }

        public void ShowCandles(IList<Candle> candleList)
        {
            count = candleList.Count;

            decimal max = Math.Max(candleList.Max(c => c.open), candleList.Max(c => c.close));
            decimal min = Math.Min(candleList.Min(c => c.open), candleList.Min(c => c.close));

            double l = (double)candleList.Max(c => Math.Abs(c.open - c.close));

            //коридор
            double y = (double)(max - min);

            k = 200 / y;

            div = (double)max;

            int i = 0;
            foreach(Candle candle in candleList)
            {
                i++;
                DrawItem(candle, i, k, div);
            }
        }

        void DrawItem(Candle item, int i, double k, double div)
        {
            Rectangle rect = new Rectangle()
            {
                Width = 5,
                Height = (double)Math.Abs(item.open - item.close) * k,
                Fill = item.close >= item.open ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft(rect, i * 10);
            double y = item.close >= item.open ? (double)item.close : (double)item.open;

            Canvas.SetTop(rect, 100 - (y-div)*k);
            canvas.Children.Add(rect);
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
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, 100-((double)p - div) * k));
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

            Polyline histogram = new Polyline();
            histogram.Fill = new SolidColorBrush(Colors.Red);


            int i = count - macd.Count;
            foreach (MACDItem item in macd)
            {
                i++;
                poliline.Points.Add(new Windows.Foundation.Point(i * 10, ((double)item.MACD)*k));
                signal.Points.Add(new Point(i * 10, ((double)item.Signal) * k));
                histogram.Points.Add(new Point(i * 10, ((double)item.Histogram) * k));
            }

            Canvas.SetLeft(poliline, 0);
            Canvas.SetTop(poliline, 350);

            Canvas.SetLeft(signal, 0);
            Canvas.SetTop(signal, 350);

            Canvas.SetLeft(histogram, 0);
            Canvas.SetTop(histogram, 400);

            canvas.Children.Add(poliline);
            canvas.Children.Add(signal);
            canvas.Children.Add(histogram);

        }
    }
}
