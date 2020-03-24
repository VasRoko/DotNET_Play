using System;

namespace Nodes
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class LinkedListNode<T>
    {
        public T Value { get; set; }
        public LinkedListNode<T> Next { get; set; }
        
        public LinkedListNode(T value)
        {
            Value = value;
        }
    }
    public class LinkedList<T> : 
        System.Collections.Generic.ICollection<T>
    {
        public int Count { get; private set; }
        public bool IsReadOnly 
        { 
            get 
            { 
                return false; 
            } 
        }
        public LinkedListNode<T> Head { get; private set; }
        public LinkedListNode<T> Tail { get; private set; }

        #region Add
        public void AddFirst(T value)
        {
            AddFirst(new LinkedListNode<T>(value));
        }
        public void AddFirst(LinkedListNode<T> node)
        {
            LinkedListNode<T> temp = Head;
            Head = node;
            
            Count++;

            if (Count == 1)
            {
                Tail = Head;
            }
        }
        public void AddLst(T value)
        {
            AddLast(new LinkedListNode<T>(value));
        }
        public void AddLast(LinkedListNode<T> node)
        {
            if (Count == 0)
            {
                Head = node;
            }
            else
            {
                Tail.Next = node;
            }
            Tail = node;
            Count++;
        }
        #endregion

        #region Remove
        public void RemoveFirst()
        {
            if (Count != 0)
            {
                Head = Head.Next;
                Count--;

                if (Count == 0)
                {
                    Tail = null;
                };
            }
        }
        public void RemoveLast()
        {
            if (Count != 0)
            {
                if (Count == 1)
                {
                    Head = null;
                    Tail = null;
                }
                else
                {
                    LinkedListNode<T> current = Head;
                    while (current.Next != Tail)
                    {
                        current = current.Next;
                    }
                    current.Next = null;
                    Tail = current;
                }

                Count--;
            }
        }
        #endregion

        #region ICollectopn
        public void Add(T item)
        {
            AddFirst(item);
        }

        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    return true;
                }

                current = current.Next;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }
        
        public bool Remove(T item)
        {
            LinkedListNode<T> previous = null;
            LinkedListNode<T> current = Head;

            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;

                        if (current.Next == null)
                        {
                            Tail = previous;
                        }

                        Count--;
                    }
                    else
                    {
                        RemoveFirst();
                    }

                    return true;
                }

                previous = current;
                current = current.Next;
            }

            return false;
        }
       
        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.Generic.IEnumerable<T>)this).GetEnumerator();
        }
        
        #endregion
    }
}
 