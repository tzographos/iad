using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetection.Core.Helpers
{
    public class RollingList<T> : IEnumerable<T>
    {
        //private variables
        //
        private readonly LinkedList<T> _list = new LinkedList<T>();


        //indexers
        //
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                    throw new ArgumentOutOfRangeException();

                return _list.Skip(index).First();
            }
        }

        //properties
        //
        public LinkedListNode<T> FirstNode => _list.First;

        public LinkedListNode<T> LastNode => _list.Last;
        
        #region Property Count
        public int Count => _list.Count;
        #endregion

        #region Property MaximumCount
        protected int m_MaximumCount;
        public int MaximumCount
        {
            get => m_MaximumCount;
            set
            {
                m_MaximumCount = value;
                this.CheckLinkedListSize();
            }
        }
        #endregion
        
        
        /// <summary>
        /// Adds a new value into the end of the list. If MaximumCount has been reached, removes the first element.
        /// </summary>
        /// <param name="value"></param>
        public virtual void Add(T value)
        {
            if (_list.Count == this.MaximumCount)
            {
                _list.RemoveFirst();
            }
            _list.AddLast(value);
        }

        /// <summary>
        /// Removes first occurance of value.
        /// </summary>
        /// <param name="value"></param>
        public virtual void Remove(T value)
        {
            LinkedListNode<T> node = _list.First;
            while (node != null)
            {
                if (node.Value.Equals(value))
                {
                    _list.Remove(node);
                    break;
                }
                node = node.Next;
            }
        }

        public virtual void RemoveAll(Predicate<T> match)
        {
            LinkedListNode<T> node = _list.First;
            while (node != null)
            {
                if (match(node.Value))
                { 
                    _list.Remove(node);
                }
                node = node.Next;
            }
        }


        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();



        //constructors
        //
        public RollingList(int maximumCount)
        {
            if (maximumCount <= 0)
                throw new ArgumentException(null, nameof(maximumCount));

            this.MaximumCount = maximumCount;
        }


        private void CheckLinkedListSize()
        {
            while (_list.Count > this.MaximumCount)
            {
                _list.RemoveFirst();
            }
        }


    }
}
