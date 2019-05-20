using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NasobeniEdhouse.Models;
using NasobeniEdhouse.Models.EdhouseTask;

namespace NasobeniEdhouse.ViewModels
{
    class InfoViewModel : BaseViewModel
    {
        private int percentage;
        private Visibility progressVisibility;
        private ProcessedValuesHolder ProcessedValuesHolder;
        public InfoViewModel(ProcessedValuesHolder processedValuesHolder)
        {
            this.ProcessedValuesHolder = processedValuesHolder;
            processedValuesHolder.PercentageEvent += PercentageUpdate;
            processedValuesHolder.BiggestNumberEvent += BiggestNumberUpdate;
            interruptionVisibility = Visibility.Collapsed;
        }

        private void BiggestNumberUpdate(List<Product> biggestNumberList)
        {
            if (biggestNumberList.Count > 1)
            {
                BiggestNumbersList.Add(biggestNumberList.Last());
            }else BiggestNumbersList = new ObservableCollection<Product>(biggestNumberList);
        }

        private void PercentageUpdate(int percentage)
        {
            Percentage = percentage;
        }

        public int Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                NotifyOfPropertyChange(() => Percentage);
                ProgressVisibility = (percentage > 99) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private ObservableCollection<Product> biggestNumbersList;

        public ObservableCollection<Product> BiggestNumbersList
        {
            get { return biggestNumbersList; }
            set
            {
                biggestNumbersList = value;
                NotifyOfPropertyChange(() => BiggestNumbersList);
            }
        }




        public Visibility ProgressVisibility
        {
            get { return progressVisibility; }
            set
            {
                progressVisibility = value;
                NotifyOfPropertyChange(()=>ProgressVisibility);
            }
        }

        private Visibility interruptionVisibility;
        public Visibility InterruptionVisibility
        {
            get { return interruptionVisibility; }
            set
            {
                interruptionVisibility = value;
                NotifyOfPropertyChange(() => InterruptionVisibility);
            }
        }
    }
}
