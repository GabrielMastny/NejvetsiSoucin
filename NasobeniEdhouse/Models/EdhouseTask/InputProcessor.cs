using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NasobeniEdhouse.Models.EdhouseTask
{
    static class InputProcessor
    {

        public static async Task<List<int>> IsInputValid(string input, CancellationToken cancelRequest)
        {
            List<int> convertedValuesList = new List<int>();
            foreach (char iterator in input)
            {
                if (!cancelRequest.IsCancellationRequested)
                {
                    int convertedValue;
                    if (int.TryParse(iterator.ToString(), out convertedValue))
                    {
                        convertedValuesList.Add(convertedValue);
                    }
                    else throw new FormatException();
                }
                else cancelRequest.ThrowIfCancellationRequested();
            }

            //await Task.Delay(3000);
            return convertedValuesList;
        }

        public static async Task ProcessValidatedInput(IProgress<ProcessingReport> progressRepport, List<int> validatedInput,int numberRange, CancellationToken cancelRequested)
        {
            

            Queue<int> neighbouringNumberQueue = new Queue<int>();
            int order = 0;
            int validatedInputItemNumber = 0;
            foreach (int number in validatedInput)
            {
                if (!cancelRequested.IsCancellationRequested)
                {
                    if (neighbouringNumberQueue.Count == (numberRange - 1))
                    {

                        neighbouringNumberQueue.Enqueue(number);
                        validatedInputItemNumber++;

                        if (!neighbouringNumberQueue.Contains(0))
                        {
                            order++;


                            await Task.Delay(10);
                        }

                        progressRepport.Report(new ProcessingReport(getPercentage(),
                            new Product(order, neighbouringNumberQueue)));


                        neighbouringNumberQueue.Dequeue();

                    }
                    else
                    {
                        neighbouringNumberQueue.Enqueue(number);
                    }
                }
                else cancelRequested.ThrowIfCancellationRequested();

                int getPercentage()
                {
                    return (int)((float)validatedInputItemNumber / (float)(validatedInput.Count - numberRange) * 100);
                }

            }


        }
    }
}
