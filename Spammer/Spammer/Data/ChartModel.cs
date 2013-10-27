using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spammer
{
    public class ChartModel : ObservableCollection<KeyValuePair<string, int>>
    {
        public ChartModel()
        {
            Init();
        }

        public void Init()
        {
            int daysCount = 15;
            Random rnd = new Random();
            var date = DateTime.Now.AddDays(-daysCount);

            for (int i = 0; i < daysCount; i++)
            {
                date = date.AddDays(1);
                var dateAsString = date.ToString("d/MMM");
                var value = rnd.Next(1, 1000);
                Add(new KeyValuePair<string, int>(dateAsString, value));
            }
        }

        public ObservableCollection<KeyValuePair<string, int>> GetData()
        {
            return this;
        }
    }

}
