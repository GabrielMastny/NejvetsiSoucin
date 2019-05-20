using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;

namespace NasobeniEdhouse.Models.EdhouseTask
{
    public class Product
    {
        private int order;
        private List<int> neighbouringNumbers;
        private long productOfNeighbours =1;

        public Product(int order, Queue<int> neighbouringNumbers)
        {
            this.order = order;
            this.neighbouringNumbers = new List<int>(neighbouringNumbers);

            foreach (int iterator in neighbouringNumbers)
            {
                if (iterator == 0)
                {
                    productOfNeighbours = 0;
                    break;
                }
                else productOfNeighbours *= iterator;
            }


        }

        public int Order
        {
            get { return order; }
        }

        public List<int> NeighbouringNumbers
        {
            get { return new List<int>(neighbouringNumbers);}
        }

        public long ProductOfNeighbours
        {
            get { return productOfNeighbours; }
        }
    }
}
