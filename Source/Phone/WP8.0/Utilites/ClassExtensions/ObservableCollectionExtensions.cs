using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;

namespace SOS.Phone
{
    public static partial class Extensions
    {
        public static void Sort<T, TKey>(this ObservableCollection<T> Items, Func<T, TKey> keySelector, System.ComponentModel.ListSortDirection direction) //where T : IComparable
        {
            List<T> sortedItemsList = direction == ListSortDirection.Ascending ? Items.OrderBy(keySelector).ToList() : Items.OrderByDescending(keySelector).ToList();

            Items.Clear();
            foreach (var item in sortedItemsList)
            {
                //Items.Move(Items.IndexOf(item), sortedItemsList.IndexOf(item));
                Items.Add(item);
            }
        }
    }
}
