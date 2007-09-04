using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DatAdmin
{
    public class GenericCommonSource : ICommonSource
    {
        protected DbConnection m_conn;
        protected IPhysicalConnection m_pconn;
        //private IConnectionHooks m_hooks = new HooksBase();

        public GenericCommonSource(IPhysicalConnection conn)
        {
            m_pconn = conn;
            m_conn = m_pconn.SystemConnection;
        }

        /*
        public GenericCommonConnection(DbConnection conn, DbProviderFactory fact)
        {
            m_conn = conn;
            m_fact = fact;
        }
        */

        //#region IServerConnection Members

        //public void Open()
        //{
        //    m_hooks.BeforeOpen();
        //    Logging.Info("Opening connection {0}", m_conn.ConnectionString);
        //    m_conn.Open();
        //    m_hooks.AfterOpen();
        //}

        //public void Close()
        //{
        //    m_hooks.BeforeClose();
        //    m_conn.Close();
        //    m_hooks.AfterClose();
        //}

        //public ConnectionStatus State
        //{
        //    get
        //    {
        //        ConnectionState state = m_conn.State;
        //        if (state == ConnectionState.Open) return ConnectionStatus.Open;
        //        else return ConnectionStatus.Closed;
        //    }
        //}

        //public DbConnection SystemConnection
        //{
        //    get { return m_conn; }
        //}

        //public IConnectionHooks Hooks
        //{
        //    get { return m_hooks; }
        //    set
        //    {
        //        m_hooks = value;
        //        if (m_hooks == null) m_hooks = new HooksBase();
        //    }
        //}

        //public DbProviderFactory DbFactory
        //{
        //    get { return m_fact; }
        //}

        //#endregion

        #region ICommonSource Members

        public IPhysicalConnection Connection
        {
            get { return m_pconn; }
        }

        #endregion
    }

    public class GenericServerSource : GenericCommonSource, IServerSource
    {
        public GenericServerSource(IPhysicalConnection conn)
            : base(conn)
        {
        }

        #region IServerConnection Members

        public IEnumerable<string> Databases
        {
            get
            {
                DataTable dbs = m_conn.GetSchema("Databases");
                List<string> lst = new List<string>();
                foreach (DataRow row in dbs.Rows) lst.Add(row[0].ToString());
                lst.Sort();
                return lst;
            }
        }

        public IDatabaseSource GetDatabase(string name)
        {
            return new GenericDatabaseSource(m_pconn, name);
        }

        #endregion
    }

    public class GenericDatabaseSource : GenericCommonSource, IDatabaseSource
    {
        string m_dbname = null;

        public GenericDatabaseSource(IPhysicalConnection conn, string dbname)
            : base(conn)
        {
            m_dbname = dbname;
        }
        public GenericDatabaseSource(IPhysicalConnection conn)
            : base(conn)
        {
        }

        #region IDatabaseConnection Members

        public IEnumerable<string> Tables
        {
            get
            {
                if (m_dbname != null) m_conn.ChangeDatabase(m_dbname);
                string[] restr = new string[] { m_dbname };
                DataTable dbs = m_conn.GetSchema("Tables", restr);

                List<string> lst = new List<string>();
                foreach (DataRow row in dbs.Rows) lst.Add(row["TABLE_NAME"].ToString());
                lst.Sort();
                return lst;
            }
        }

        public ITableSource GetTable(string name)
        {
            return new GenericTableSource(m_pconn, m_dbname, name);
        }

        #endregion
    }

    public class GenericTableSource : GenericCommonSource, ITableSource
    {
        string m_dbname;
        string m_tblname;

        public GenericTableSource(IPhysicalConnection conn, string dbname, string tblname)
            : base(conn)
        {
            m_dbname = dbname;
            m_tblname = tblname;
        }

        public DataTable GetTabularData(TableDataProperties props)
        {
            DbDataAdapter ada = m_pconn.DbFactory.CreateDataAdapter();
            //DbCommandBuilder bld = m_fact.CreateCommandBuilder();
            DbCommand cmd = m_pconn.DbFactory.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + m_tblname;
            cmd.Connection = m_conn;
            ada.SelectCommand = cmd;
            DataTable res = new DataTable();
            ada.Fill(res);
            return res;
        }

        #region ITableConnection Members

        public string TableName
        {
            get { return m_tblname; }
        }

        #endregion
    }
}
