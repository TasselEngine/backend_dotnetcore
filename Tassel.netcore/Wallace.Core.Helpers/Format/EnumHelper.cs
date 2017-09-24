using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Wallace.Core.Helpers.Format {
    public static class EnumHelper {

        public static T Parse<T>(string content) where T : struct
            => (T)Enum.Parse(typeof(T), content);

        public static T TryParse<T>(string content)where T: struct {
            var ok = Enum.TryParse(content, out T result);
            return ok ? result : default(T);
        }

        public static T TryParseHash<T>(int hashCode) where T : struct {
            var values = TryGetValues<T>();
            foreach(var item in values) {
                if (item.GetHashCode() == hashCode)
                    return item;
            }
            return default(T);
        }

        public static T[] TryGetValues<T>() where T : struct
            => Enum.GetValues(typeof(T)) as T[];

        public static string[] TryGetNames<T>() where T : struct
            => Enum.GetNames(typeof(T));

        public static IEnumerable<dynamic> TryGetNamesLocal<T,C>(IStringLocalizer<C> localize) where T : struct
            => new EnumCollection<T>(TryGetValues<T>().Select(i => new EnumEntry<T>(i)).ToList()).Localization(localize);

    }

    [DataContract]
    class EnumEntry<T> {

        public EnumEntry(T entry) => _entry = entry;

        readonly T _entry;

        [DataMember(Name ="key")]
        public int Key { get => _entry.GetHashCode(); }

        [DataMember(Name = "value")]
        public string Value { get => _entry.ToString(); }
    }

    class EnumCollection<T> : IList<EnumEntry<T>> {

        IList<EnumEntry<T>> list;

        public EnumCollection() => list = new List<EnumEntry<T>>();

        public EnumCollection(IList<EnumEntry<T>> list) => this.list = list;

        public EnumEntry<T> this[int index] {
            get => list[index];
            set => list[index] = value;
        }

        public int Count => list.Count;

        public bool IsReadOnly => list.IsReadOnly;

        public void Add(EnumEntry<T> item) => list.Add(item);

        public void Clear() => list.Clear();

        public bool Contains(EnumEntry<T> item) => list.Contains(item);

        public void CopyTo(EnumEntry<T>[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public IEnumerator<EnumEntry<T>> GetEnumerator() => list.GetEnumerator();

        public int IndexOf(EnumEntry<T> item) => list.IndexOf(item);

        public void Insert(int index, EnumEntry<T> item) => list.Insert(index, item);

        public bool Remove(EnumEntry<T> item) => list.Remove(item);

        public void RemoveAt(int index) => list.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        public IEnumerable<dynamic> Localization<C>(IStringLocalizer<C> localizer) {
            var newList = new List<dynamic>();
            foreach(var item in list) {
                newList.Add(new {
                    Key = item.Key,
                    Value = localizer[item.Value].Value
                });
            }
            return newList;
        }
    }

}
