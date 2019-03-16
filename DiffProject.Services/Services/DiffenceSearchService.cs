using DiffProject.Services.Interfaces;
using DiffProject.Services.Models;
using System;
using System.Collections.Generic;


namespace DiffProject.Services.Services
{
    public class DiffenceSearchService : IDiffenceSearchService
    {
        public IList<Difference> GetDifferences(string first, string second, int size)
        {
            var diffs = new List<Difference>();
            var IsInDiff = false;
            int sizeCount = 0;
            int difPosition = 0;
            char fromFirst;
            char fromSecond;
            int i = 0;
            for (i = 0; i < size; i++)
            {
                fromFirst = first[i];
                fromSecond = second[i];
                if (!IsInDiff && fromFirst != fromSecond) //new in difference
                {
                    IsInDiff = true;
                    sizeCount++;
                    difPosition = i;
                }
                else if (IsInDiff && fromFirst == fromSecond) // end of Difference
                {
                    Difference diff = new Difference
                    {
                        DifferenceSize = sizeCount,
                        DifferenceStartPosition = difPosition
                    };
                    diffs.Add(diff);
                    IsInDiff = false;
                    sizeCount = 0;
                    difPosition = 0;
                }
                else if (IsInDiff && fromFirst != fromSecond) //Still in difference
                {
                    sizeCount++;
                }
            }
            if (IsInDiff) //Process the last char if it was still a difference save to array
            {
                Difference diff = new Difference
                {
                    DifferenceSize = sizeCount,
                    DifferenceStartPosition = difPosition
                };
                diffs.Add(diff);
            }
            return diffs;
        }
    }
}
