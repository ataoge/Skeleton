using System;
using System.Collections.Generic;

namespace Ataoge.GisCore.Geometry
{
    public class ObservableCollection<T> : ICollection<T>
    {
        private List<T> pts = new List<T>();

        public void Add(T item)
        {
            pts.Add(item);
        }

        public T this[int index]
        {
            get
            {
                if (pts.Count < index + 1 || index < 0)
                    throw new Exception("index不合法");
                else
                    return pts[index];
            }
            set
            {
                if (pts.Count < index + 1 || index < 0)
                    throw new Exception("index不合法");
                else
                    pts[index] = value;
            }
        }

        public void Clear()
        {
            pts.Clear();
        }

        public bool Contains(T item)
        {
            return pts.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return pts.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(T item)
        {
            return pts.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return pts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return pts.GetEnumerator();
        }

        public void Reverse()
        {
            int num = this.Count;
            if (num > 1)
            {
                int mid = num / 2;
                for (int i = 0; i < mid; i++)
                {
                    T mp1 = this[i];
                    this[i] = this[num - 1 - i];
                    this[num - 1 - i] = mp1;
                }
            }
        }

    }
}