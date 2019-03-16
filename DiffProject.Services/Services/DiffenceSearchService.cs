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
                if (!IsInDiff && fromFirst != fromSecond)
                {
                    IsInDiff = true;
                    sizeCount++;
                    difPosition = i;
                }
                else if (IsInDiff && fromFirst == fromSecond)
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
                else if (IsInDiff && fromFirst != fromSecond)
                {
                    sizeCount++;
                }
            }
            if (IsInDiff)
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
