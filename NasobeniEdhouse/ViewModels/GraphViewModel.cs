using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using LiveCharts;
using LiveCharts.Wpf;
using NasobeniEdhouse.Models;

namespace NasobeniEdhouse.ViewModels
{
    class GraphViewModel : BaseViewModel
    {
        private SeriesCollection seriesCollection;
        private ProcessedValuesHolder processedValuesHolder;
        public GraphViewModel(ProcessedValuesHolder processedValuesHolder)
        {
            this.processedValuesHolder = processedValuesHolder;
            this.processedValuesHolder.SeriesChangedEvent += UpdateSeries;
            
            SeriesCollection = new SeriesCollection{new ColumnSeries(){Values = new ChartValues<long>()}};


            ZoomingMode = ZoomingOptions.X;

            XFormatter = val => ((long)val).ToString();
            YFormatter = val => val.ToString();

        }

        private void UpdateSeries(SeriesCollection seriesCollection)
        {
            SeriesCollection = seriesCollection;
        }

        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection;}
            set
            {
                
                seriesCollection = value;
                    XMaxValue = seriesCollection[0].Values.Count;
                NotifyOfPropertyChange(() => SeriesCollection);
            }
        }
        public Func<double, string> Formatter { get; set; }




        private ZoomingOptions _zoomingMode;
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                NotifyOfPropertyChange(() => ZoomingMode);
            }
        }

        private void ToogleZoomingMode(object sender, RoutedEventArgs e)
        {
            switch (ZoomingMode)
            {
                case ZoomingOptions.None:
                    ZoomingMode = ZoomingOptions.X;
                    break;
                case ZoomingOptions.X:
                    ZoomingMode = ZoomingOptions.Y;
                    break;
                case ZoomingOptions.Y:
                    ZoomingMode = ZoomingOptions.Xy;
                    break;
                case ZoomingOptions.Xy:
                    ZoomingMode = ZoomingOptions.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


        }

        private double xMinValue = 0;

        public double XMinValue
        {
            get { return xMinValue; }
            set
            {
                Console.WriteLine($"xMin {value}");
                if (value < 0)
                {
                    xMinValue = 0;
                }
                else xMinValue = value;
                
                NotifyOfPropertyChange(() => XMinValue);
            }
        }

        private double yMinValue = 0;

        public double YMinValue
        {
            get { return yMinValue; }
            set
            {
                Console.WriteLine($"yMin {value}");
                if (value < 0)
                {
                    yMinValue = 0;
                }
                else yMinValue = value;

                NotifyOfPropertyChange(() => YMinValue);
            }
        }

        private double xMaxValue = 1;

        public double XMaxValue
        {
            get { return xMaxValue; }
            set
            {
                double toBeChangedTo = value;
                Console.WriteLine($"xMax {value}");
                if (toBeChangedTo > seriesCollection[0].Values.Count)
                {
                    toBeChangedTo = seriesCollection[0].Values.Count;
                    
                }else if (toBeChangedTo > (xMinValue + 50))
                {
                    XMinValue = toBeChangedTo - 50;
                }
                else if (toBeChangedTo > xMinValue)
                {
                    
                    
                        xMaxValue = toBeChangedTo;
                        NotifyOfPropertyChange(() => XMaxValue);
                   
                }

                
            }
        }

        private double yMaxRange = 100;

        public double YMaxRange
        {
            get { return yMaxRange; }
            set
            {
                Console.WriteLine($"yRange {value}");
                yMaxRange = 2;
                NotifyOfPropertyChange(()=> YMaxRange);
            }
        }



    }
}
