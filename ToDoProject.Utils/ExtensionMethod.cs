﻿using System.Collections.ObjectModel;

namespace ToDoProject.Utils
{
    public static class ExtensionMethod
    {
        public static Int32 ToInt32(this string number)
        {
            if (Int32.TryParse(number, out int result))
            {
                return result;
            }
            return -1;
        }

        public static bool IsToday(this DateTime date)
        {
            return date.Equals(DateTime.Today);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> list)
        {
            return new ObservableCollection<T>(list);
        }
    }
}