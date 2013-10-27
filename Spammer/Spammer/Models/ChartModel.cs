using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Spammer
{
    public class ChartModel : ObservableCollection<KeyValuePair<string, int>>
    {
        public void Init(int daysCount)
        {
            this.Clear();

            var results = DataPersister.GetChartHistory(daysCount);
            var resultsAsString = new Dictionary<string, int>();
            foreach (var result in results)
            {
                string key = result.Key.ToString("d/MMM");
                resultsAsString[key] = result.Value;
            }

            var date = DateTime.Now.AddDays(-daysCount);
            for (int i = 0; i < daysCount; i++)
            {
                date = date.AddDays(1);
                string dateAsString = date.ToString("d/MMM");
                int count = 0;

                var dateExists = resultsAsString.ContainsKey(dateAsString);
                if (dateExists == true)
                {
                    count = resultsAsString[dateAsString];
                }

                this.Add(new KeyValuePair<string, int>(dateAsString, count));
            }
        }
    }
}
