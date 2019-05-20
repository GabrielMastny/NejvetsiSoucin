﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NasobeniEdhouse.ViewModels
{
    public class ActiveInputViewModel : BaseViewModel
    {
        private string inputValues;
        public ActiveInputViewModel(NasobeniViewModel parrentViewModel)
        {
            inputValues = "1398024787037910369017901433749013239719493017201986983520312774506326239578318016984801869478851843758615607891129494954595017379583319528532088055110254069874715852386305071569329096329522744304355786696648950445244523161731856403098711121722383113522298934233803081353362766142828064444866452387492035890729629049156044077239071381051585930796086680172427121883998797908792274921901699720888093776757273330010533678812202354218097512545405947522436258490771167055601360483958644670632441572215539743697817977846174064955149290862569321978468622482339722413756570560574902614079729686524145351004744216637048440319989000889524345065854122758866688126427171479924442928230863465674813919123162824586078664583591245665294765456828489128831426076900421421902267105562632111110937054421750694165896040817198403850962455444362981230987879927244284909188745801561660979191338754992005240636899125607176063588611646710940507754100225698315520005593572972521636269561882670428252483600823257530420752963450";

        }

        public string InputValues
        {
            get { return inputValues; }
            set
            {
                inputValues = value;
                NotifyOfPropertyChange(() => InputValues);
            }
        }
    }
}
