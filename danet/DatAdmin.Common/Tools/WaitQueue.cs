using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DatAdmin
{
    public class WaitQueue<T>
    {
        Queue<T> m_queue = new Queue<T>();
        AutoResetEvent m_event = new AutoResetEvent(false);

        public void Put(T element)
        {
            lock (m_queue)
            {
                m_queue.Enqueue(element);
                m_event.Set();
            }
        }

        public T Get()
        {
            for (; ; )
            {
                lock (m_queue)
                {
                    if (m_queue.Count > 0) return m_queue.Dequeue();
                }
                m_event.WaitOne();
            }
        }
    }
}
