using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using NasobeniEdhouse.Models.EdhouseTask;
using NasobeniEdhouse.ViewModels;

namespace NasobeniEdhouse.Models
{
    public class ProcessedValuesHolder
    {
        private List<int> validatedInput;
        private List<Product> biggestNumber;
        private List<Product> processedProducts;
        private CancellationTokenSource cancelRequested;
        private SeriesCollection seriesCollection;
        private NasobeniViewModel.OrderBy selectedOrderBy;

        public ProcessedValuesHolder()
        {
            seriesCollection = new SeriesCollection();
            seriesCollection.Add(new ColumnSeries());
            seriesCollection.First().Values = new ChartValues<long>();
        }

        public void ChangeSeriesCollectionOrder(NasobeniViewModel.OrderBy newOrder)
        {
            selectedOrderBy = newOrder;
            processedProducts = ReOrderBySelectedMethod();
             seriesCollection[0].Values = new ChartValues<long>(from s in processedProducts select s.ProductOfNeighbours);
             SeriesChangedEvent(seriesCollection);
        }

        public delegate void SeriesChanged(SeriesCollection seriesCollection);
        public event SeriesChanged SeriesChangedEvent;

        public delegate void BiggestNumberListChanged(List<Product> biggestNumberList);

        public event BiggestNumberListChanged BiggestNumberEvent;

        public delegate void PercentageChanged(int percentage);

        public event PercentageChanged PercentageEvent;
        
        public async Task StartProcessing(string inputValues, int neighbourNumberRange, NasobeniViewModel.OrderBy selectedOrderByMethod)
        {
            selectedOrderBy = selectedOrderByMethod;
            biggestNumber = new List<Product>();
            cancelRequested = new CancellationTokenSource();
            processedProducts = new List<Product>();
            seriesCollection.First().Values = new ChartValues<long>();
            SeriesChangedEvent(seriesCollection);

            try
            {
                validatedInput = await InputProcessor.ValidateInput(inputValues, cancelRequested.Token);

            }
            catch (OperationCanceledException e)
            {
                throw new OperationCanceledException("",e);
            }
            catch (FormatException e)
            {
                throw new FormatException("",e);
            }

            Progress<ProcessingReport> Report = new Progress<ProcessingReport>();
            Report.ProgressChanged += ReportProgress;
            await InputProcessor.ProcessValidatedInput(Report, validatedInput, neighbourNumberRange, cancelRequested.Token);
            cancelRequested.Cancel();
            

            
        }

        private List<Product> ReOrderBySelectedMethod()
        {
            if (processedProducts != null)
            {
                switch (selectedOrderBy)
                {
                    case NasobeniViewModel.OrderBy.Vzestupně: return new List<Product>(from s in processedProducts orderby s.ProductOfNeighbours select s);
                    case NasobeniViewModel.OrderBy.Sestupně: return new List<Product>(from s in processedProducts orderby s.ProductOfNeighbours descending select s);
                    case NasobeniViewModel.OrderBy.podlePořadí: return new List<Product>(from s in processedProducts orderby s.Order select s);
                }
            }
            
            return new List<Product>();
        }

        private void UpdateSeriesCollection(Product product)
        {
            processedProducts.Add(product);
            processedProducts = ReOrderBySelectedMethod();
            seriesCollection[0].Values = new ChartValues<long>(from s in processedProducts select s.ProductOfNeighbours);
            SeriesChangedEvent(seriesCollection);
        }

        public async Task<bool> StopProcessing()
        {
            if (cancelRequested.Token.CanBeCanceled)
            {
                cancelRequested.Cancel();
                return true;
            }
            else return false;
        }

        private void ReportProgress(object sender, ProcessingReport e)
        {
            if (biggestNumber.Count > 0 )
            {
                if (biggestNumber.First().ProductOfNeighbours == e.ProcessedProduct.ProductOfNeighbours)
                {
                    biggestNumber.Add(e.ProcessedProduct);
                    BiggestNumberEvent(biggestNumber);
                }

                if (biggestNumber.First().ProductOfNeighbours < e.ProcessedProduct.ProductOfNeighbours)
                {
                    biggestNumber = new List<Product>() { e.ProcessedProduct };
                    BiggestNumberEvent(biggestNumber);
                }
            }
            else
            {
                if (e.ProcessedProduct.ProductOfNeighbours > 0)
                {
                    biggestNumber.Add(e.ProcessedProduct);
                    BiggestNumberEvent(biggestNumber);
                }
            }

            

            

            if (!(e.ProcessedProduct.ProductOfNeighbours == 0))
            {
                UpdateSeriesCollection(e.ProcessedProduct);
                SeriesChangedEvent(seriesCollection);
            }
            

            PercentageEvent(e.Percentage);

        }
    }
}
