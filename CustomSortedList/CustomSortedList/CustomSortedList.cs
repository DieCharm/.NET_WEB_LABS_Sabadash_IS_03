using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomSortedList
{
    public class CustomSortedList<T> : 
        IList<T>, ICloneable 
        where T : IComparable
    {
        private T[] Repository { get; set; }

        public delegate void CollectionChangeHandler();

        public event CollectionChangeHandler Report = 
            () => Console.WriteLine("Collection is changed. Element is not at specified position");

        public CustomSortedList() => Repository = new T[0];

        public CustomSortedList(T[] array)
        {
            Repository = array;
            Sort();
        }
        
        public object Clone() => new CustomSortedList<T>(Repository);

        private bool Sort()
        {
            List<T> temp = Repository.ToList();
            temp.Sort();
            bool flag = !temp.ToArray().SequenceEqual(Repository);
            Repository = temp.ToArray();
            return flag;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var element in Repository)
            {
                yield return element;
            }
        }

        public IEnumerable<T> Reverse
        {
            get
            {
                for (int i = Repository.Length - 1; i >= 0; i--)
                {
                    yield return Repository[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T? item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter is null");
            }

            T[] temp = new T[Repository.Length + 1];
            CopyTo(temp,0);
            temp[temp.Length - 1] = item;
            Repository = temp;
            Sort();
        }

        public void Clear() => Repository = new T[0];

        public bool Contains(T? item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter is null");
            }
            
            return Repository.Any(el => el.Equals(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex >= Repository.Length || arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index is outside the array");
            }

            if (array.Length < Repository.Length + arrayIndex)
            {
                throw new ArgumentException("Array size is not enough");
            }
            
            for (int i = 0; i < Repository.Length; i++)
            {
                array[i + arrayIndex] = Repository[i];
            }
        }

        public bool Remove(T? item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter is null");
            }

            if (!this.Contains(item))
            {
                return false;
            }
            
            RemoveAt(IndexOf(item));
            return true;
        }

        public int Count
        {
            get => Repository.Length;
        }
        public bool IsReadOnly
        {
            get => false;
        }
        
        public int IndexOf(T item)
        {
            if (!Contains(item))
            {
                throw new ArgumentException("There are no elements in list equal to value");
            }
            
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter is null");
            }

            for (int i = 0; i < Repository.Length; i++)
            {
                if (Repository[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is outside the array");
            }
            
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Parameter is null");
            }

            T[] temp = new T[Repository.Length + 1];
            temp[index] = item;
            for (int i = 0; i < index; i++)
            {
                temp[i] = Repository[i];
            }

            for (int i = index; i < Repository.Length; i++)
            {
                temp[i + 1] = Repository[i];
            }
            
            Repository = temp;
            if (Sort())
            {
                Report.Invoke();
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Repository.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is outside the array");
            }
            
            T[] temp = new T[Repository.Length - 1];
            for (int i = 0; i < index; i++)
            {
                temp[i] = Repository[i];
            }

            for (int i = index; i < Repository.Length - 1; i++)
            {
                temp[i] = Repository[i + 1];
            }

            Repository = temp;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Repository.Length)
                {
                    throw new ArgumentException("Index is outside the array");
                }

                return Repository.First(el => IndexOf(el) == index);
            }

            set
            {
                if (index < 0 || index >= Repository.Length)
                {
                    throw new ArgumentException("Index is outside the array");
                }
                
                RemoveAt(index);
                Insert(index,value);
            }
        }
    }
}

