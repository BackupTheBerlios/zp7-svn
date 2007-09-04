using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;
using System.Data.Common;
using System.Data;

namespace DatAdmin
{
    //public delegate TConnection ConnectDelegate<TConnection>();

    //public delegate void ConnectionDelegate<TConnection>(TConnection connection);

    public class GenericDbConnection : IPhysicalConnection, IInvoker
    {
        internal static object ENDMARK = new object();
        Thread m_thread;
        //ConnectDelegate m_connect;
        DbConnection m_conn = null;
        DbProviderFactory m_factory;
        //IInvoker m_invoker;
        WaitQueue<object> m_queue = null;

        public GenericDbConnection(DbConnection conn, DbProviderFactory factory)
        {
            m_conn = conn;
            m_factory = factory;
        }

        private void Run()
        {
            try
            {
                ThreadRegister.RegisterThread(m_thread);
                for (; ; )
                {
                    object obj = m_queue.Get();
                    if (obj == ENDMARK) break;
                    ((SimpleCallback)obj)();
                }
            }
            finally
            {
                ThreadRegister.UnregisterThread(m_thread);
                m_thread = null;
            }

            //for (; ; )
            //{
            //    object obj = m_queue.Dequeue();
            //    if (obj == ENDMARK) break;
            //    WantOpen();
            //    ((SimpleCallback)obj)();
            //}
            //if (m_conn.State != ConnectionState.Closed)
            //{
            //    DoClose();
            //}
        }

        //public void Async(ConnectionDelegate<TConnection> callback, I

        #region IPhysicalConnection Members

        //public ConnectionStatus State
        //{
        //    get
        //    {
        //        ConnectionState state = m_conn.State;
        //        if (state == ConnectionState.Open) return ConnectionStatus.Open;
        //        else return ConnectionStatus.Closed;
        //    }
        //}

        //public IInvoker EventsInvoker
        //{
        //    get { return m_invoker; }
        //    set { m_invoker = value; }
        //}

        public event PhysicalConnectionDelegate BeforeOpen;

        public event PhysicalConnectionDelegate AfterOpen;

        public event PhysicalConnectionDelegate BeforeClose;

        public event PhysicalConnectionDelegate AfterClose;

        public IAsyncVoid InvokeVoid(SimpleCallback func)
        {
            AsyncAction async = new AsyncAction(func);
            m_queue.Put((SimpleCallback)async.DoRun);
            return async.Async;
        }

        public IAsyncValue<T> InvokeValue<T>(ReturnValueCallback<T> func)
        {
            AsyncResultAction<T> async = new AsyncResultAction<T>(func);
            m_queue.Put((SimpleCallback)async.DoRun);
            return async.Async;
        }

        public DbConnection SystemConnection
        {
            get { return m_conn; }
        }

        public DbProviderFactory DbFactory
        {
            get { return m_factory; }
        }

        private void DoOpen()
        {
            if (BeforeOpen != null) BeforeOpen(this);
            m_conn.Open();
            if (AfterOpen != null) AfterOpen(this);
        }

        private void DoClose()
        {
            if (BeforeClose != null) BeforeClose(this);
            m_conn.Close();
            if (AfterClose != null) AfterClose(this);

            m_conn = null;
            m_queue = null;
        }

        public IAsyncVoid Open()
        {
            if (m_thread != null) throw new ConnectionException("Opening allready opened connection ");
            m_thread = new Thread(Run);
            m_queue = new WaitQueue<object>();
            m_thread.Start();
            return InvokeVoid(DoOpen);
        }

        public IAsyncVoid Close()
        {
            if (m_thread == null) throw new ConnectionException("Closing closed connection");
            return InvokeVoid(DoClose);
        }

        public bool IsOpened
        {
            get { return m_thread != null; }
        }

        public IInvoker Invoker
        {
            get { return this; }
        }

        #endregion
    }
}
