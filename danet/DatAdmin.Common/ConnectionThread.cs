using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;

namespace DAIntf
{
    //public delegate TConnection ConnectDelegate<TConnection>();

    public delegate void ConnectionDelegate<TConnection>(TConnection connection);

    public class ConnectionThread<TConnection> where TConnection : ICommonConnection
    {
        internal static object ENDMARK = new object();
        Thread m_thread;
        //ConnectDelegate m_connect;
        TConnection m_conn;
        Queue m_queue = Queue.Synchronized(new Queue());

        public ConnectionThread(TConnection conn)
        {
            m_thread = new Thread(Run);
            m_conn = conn;
        }
        public void Start()
        {
            m_thread.Start();
        }
        private void Run()
        {
            m_conn.Open();
            for (; ; )
            {
                object obj = m_queue.Dequeue();
                if (obj == ENDMARK) return;
            }
            m_conn.Close();
        }
        //public void Async(ConnectionDelegate<TConnection> callback, I
    }
}
