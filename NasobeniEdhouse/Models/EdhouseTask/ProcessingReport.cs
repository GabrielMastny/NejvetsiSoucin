using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasobeniEdhouse.Models.EdhouseTask
{
    class ProcessingReport
    {
        private int percentageOfTask;
        private Product processedProduct;

        public ProcessingReport(int percentageOfTask, Product processedProduct)
        {
            this.percentageOfTask = percentageOfTask;
            this.processedProduct = processedProduct;
        }

        public int Percentage
        {
            get { return percentageOfTask; }
        }

        public Product ProcessedProduct
        {
            get { return processedProduct; }
        }
    }
}
