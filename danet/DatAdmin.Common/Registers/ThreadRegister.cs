using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DatAdmin
{
    public static class ThreadRegister
    {
        class Item
        {
            internal Thread m_thread;
            internal SimpleCallback m_onquit;
            internal Item(Thread thread, SimpleCallback onquit)
            {
                m_thread = thread;
                m_onquit = onquit;
            }
        }
        static List<Item> m_items = new List<Item>();

        public static void RegisterThread(Thread thread, SimpleCallback onquit)
        {
            lock (m_items)
            {
                m_items.Add(new Item(thread, onquit));
            }
        }
        public static void RegisterThread(Thread thread)
        {
            lock (m_items)
            {
                m_items.Add(new Item(thread, null));
            }
        }
        public static void UnregisterThread(Thread thread)
        {
            lock (m_items)
            {
                Item remove = null;
                foreach (Item item in m_items)
                {
                    if (item.m_thread == thread)
                    {
                        remove = item;
                    }
                }
                m_items.Remove(remove);
            }
        }
        public static void QuitAllThreads()
        {
            Item[] items;
            lock (m_items)
            {
                items = m_items.ToArray();
            }
            foreach (Item item in items)
            {
                if (item.m_onquit != null) item.m_onquit();
                if (item.m_thread.IsAlive) item.m_thread.Abort();
            }
        }
    }

}
