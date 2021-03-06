﻿using System;
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
        /// <summary>
        /// Validates string input, input has to contain only numerical characters, otherwise is thrown exception
        /// </summary>
        /// <param name="input">expected only numerical characters</param>
        /// <param name="cancelRequest">token for cancelation of method</param>
        /// <returns></returns>
        public static async Task<List<int>> ValidateInput(string input, CancellationToken cancelRequest)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressRepport">delegate for reporting of progress of method</param>
        /// <param name="validatedInput"> List of validated values</param>
        /// <param name="numberRange"> range of neighbouring numbers</param>
        /// <param name="cancelRequested">token for cancelation of method</param>
        /// <returns></returns>
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


                            //await Task.Delay(500);
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
