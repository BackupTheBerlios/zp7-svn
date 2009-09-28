using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
namespace Generated
{
    public static class DbCreator
    {
        private static void ExecuteNonQuery(DbConnection conn, DbTransaction tran, string sql)
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.Transaction = tran;
                cmd.ExecuteNonQuery();
            }
        }
        public static string GetVersion(DbConnection conn, DbTransaction tran)
        {
            try
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tran;
                    cmd.CommandText = "select value from info where name=\'dbversion\'";
                    return cmd.ExecuteScalar().ToString();
                }
            }
            catch
            {
                return null;
            }
        }
        // update to version 2
        private static void UpdateToVersion_0(DbConnection conn, DbTransaction tran)
        {
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"song\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"title\" text NULL, \n    \"groupname\" text NULL, \n    \"author\" text NULL, \n    \"songtext\" text NULL, \n    \"lang\" text NULL, \n    \"server_id\" integer NULL, \n    \"netID\" integer NULL, \n    \"transp\" integer NULL, \n    \"searchtext\" text NULL, \n    \"published\" text NULL, \n    \"localmodified\" integer NULL, \n    \"remark\" text NULL, \n    \"link_1\" text NULL, \n    \"link_2\" text NULL\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"server\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"url\" text NULL, \n    \"servertype\" text NULL, \n    \"config\" text NULL, \n    \"isreadonly\" integer NULL\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"deletedsong\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"song_netID\" integer NULL, \n    \"server_id\" integer NULL\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"info\" ( \n    \"name\" text NOT NULL, \n    \"value\" text NULL,  \n    PRIMARY KEY (\"name\")\n)");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"info\" (\"name\", \"value\") VALUES (\'dbversion\', \'2\')");
            ExecuteNonQuery(conn, tran, "update info set value=\'2\' where name=\'dbversion\'");
        }
        // update to version 3
        private static void UpdateToVersion_1(DbConnection conn, DbTransaction tran)
        {
            ExecuteNonQuery(conn, tran, "create table tmp_song_data (songid int, textdata text, dataid int);\ninsert into tmp_song_data (songid, textdata, dataid) select ID, songtext, 1 from song;\ninsert into tmp_song_data (songid, textdata, dataid) select ID, songtext, 3 from song where link_1 is not null;\ninsert into tmp_song_data (songid, textdata, dataid) select ID, songtext, 3 from song where link_2 is not null;\n");
            ExecuteNonQuery(conn, tran, "ALTER TABLE \"song\" RENAME TO \"temp_table_4_128985927061900000\"");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"song\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"title\" text NULL, \n    \"groupname\" text NULL, \n    \"author\" text NULL, \n    \"lang\" text NULL, \n    \"server_id\" integer NULL, \n    \"netID\" integer NULL, \n    \"transp\" integer NULL, \n    \"published\" datetime NULL, \n    \"localmodified\" logical NULL, \n    \"remark\" text NULL, \n    CONSTRAINT \"FK_song_server_id\" FOREIGN KEY (\"server_id\") REFERENCES \"server\"(\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "CREATE INDEX \"IX_song_groupname_ID\" ON \"song\" (\"groupname\",\"ID\")");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"song\" (\"ID\", \"title\", \"groupname\", \"author\", \"lang\", \"server_id\", \"netID\", \"transp\", \"published\", \"localmodified\", \"remark\") select \"ID\" AS \"ID\", \"title\" AS \"title\", \"groupname\" AS \"groupname\", \"author\" AS \"author\", \"lang\" AS \"lang\", \"server_id\" AS \"server_id\", \"netID\" AS \"netID\", \"transp\" AS \"transp\", \"published\" AS \"published\", \"localmodified\" AS \"localmodified\", \"remark\" AS \"remark\" FROM \"temp_table_4_128985927061900000\"");
            ExecuteNonQuery(conn, tran, "DROP TABLE \"temp_table_4_128985927061900000\"");
            ExecuteNonQuery(conn, tran, "ALTER TABLE \"server\" RENAME TO \"temp_table_5_128985927061930000\"");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"server\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"url\" text NULL, \n    \"servertype\" text NULL, \n    \"config\" text NULL, \n    \"isreadonly\" logical NULL\n)");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"server\" (\"ID\", \"url\", \"servertype\", \"config\", \"isreadonly\") select \"ID\" AS \"ID\", \"url\" AS \"url\", \"servertype\" AS \"servertype\", \"config\" AS \"config\", \"isreadonly\" AS \"isreadonly\" FROM \"temp_table_5_128985927061930000\"");
            ExecuteNonQuery(conn, tran, "DROP TABLE \"temp_table_5_128985927061930000\"");
            ExecuteNonQuery(conn, tran, "ALTER TABLE \"deletedsong\" RENAME TO \"temp_table_6_128985927061960000\"");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"deletedsong\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"song_netID\" integer NULL, \n    \"server_id\" integer NULL, \n    CONSTRAINT \"FK_deletedsong_server_id\" FOREIGN KEY (\"server_id\") REFERENCES \"server\"(\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"deletedsong\" (\"ID\", \"song_netID\", \"server_id\") select \"ID\" AS \"ID\", \"song_netID\" AS \"song_netID\", \"server_id\" AS \"server_id\" FROM \"temp_table_6_128985927061960000\"");
            ExecuteNonQuery(conn, tran, "DROP TABLE \"temp_table_6_128985927061960000\"");
            ExecuteNonQuery(conn, tran, "UPDATE \"info\" SET \"value\"=\'3\' WHERE \"name\"=\'dbversion\'");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"songdata\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"song_id\" integer NOT NULL, \n    \"datatype_id\" integer NOT NULL, \n    \"label\" text NULL, \n    \"textdata\" text NULL, \n    CONSTRAINT \"FK_songdata_song_id\" FOREIGN KEY (\"song_id\") REFERENCES \"song\"(\"ID\"), \n    CONSTRAINT \"FK_songdata_datatype_id\" FOREIGN KEY (\"datatype_id\") REFERENCES \"datatype_list\"(\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "CREATE INDEX \"IX_songdata_song_id\" ON \"songdata\" (\"song_id\")");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"datatype_list\" ( \n    \"ID\" integer NOT NULL, \n    \"name\" text NOT NULL, \n    CONSTRAINT \"PK_datatype_list\" PRIMARY KEY (\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"datatype_list\" (\"ID\", \"name\") VALUES (\'3\', \'link\')");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"datatype_list\" (\"ID\", \"name\") VALUES (\'1\', \'songtext\')");
            ExecuteNonQuery(conn, tran, "INSERT INTO \"datatype_list\" (\"ID\", \"name\") VALUES (\'2\', \'notation\')");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"songlist\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"name\" text NOT NULL, \n    \"options\" text NULL\n)");
            ExecuteNonQuery(conn, tran, "CREATE TABLE \"songlistitem\" ( \n    \"ID\" integer primary key NOT NULL, \n    \"song_id\" integer NOT NULL, \n    \"songlist_id\" integer NOT NULL, \n    \"transp\" integer NULL, \n    \"position\" integer NULL, \n    CONSTRAINT \"FK_songlistitem_song_id\" FOREIGN KEY (\"song_id\") REFERENCES \"song\"(\"ID\"), \n    CONSTRAINT \"FK_songlistitem_songlist_id\" FOREIGN KEY (\"songlist_id\") REFERENCES \"songlist\"(\"ID\")\n)");
            ExecuteNonQuery(conn, tran, "CREATE INDEX \"IX_songlistitem_song_id\" ON \"songlistitem\" (\"song_id\")");
            ExecuteNonQuery(conn, tran, "CREATE INDEX \"IX_songlistitem_songlist_id\" ON \"songlistitem\" (\"songlist_id\")");
            ExecuteNonQuery(conn, tran, "insert into songdata (song_id, textdata, datatype_id) \nselect songid, textdata, dataid from tmp_song_data;\ndrop table tmp_song_data;\n\n");
            ExecuteNonQuery(conn, tran, "update info set value=\'3\' where name=\'dbversion\'");
        }
        public static void Run(DbConnection conn)
        {
            using (DbTransaction tran = conn.BeginTransaction())
            {
                if (GetVersion(conn, tran) == null) UpdateToVersion_0(conn, tran);
                if (GetVersion(conn, tran) == "2") UpdateToVersion_1(conn, tran);
                tran.Commit();
            }
        }
    }
}
