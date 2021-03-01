using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCommon
{
    public class MSSQL_helper
    {
        public MSSQL_helper(String connectionString)
        {
            _conn = connectionString;
        }

        private string _conn = null;
        public DataTable[] RunQuery(string storeProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection myConnection = new SqlConnection(_conn))
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(storeProcedure, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                if (parameters != null && parameters.Length > 0)
                {
                    myCommand.Parameters.AddRange(parameters.ToArray());
                }

                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                List<DataTable> tables = new List<DataTable>();

                do
                {
                    DataTable table = new DataTable();
                    for (int i = 0; myReader.FieldCount > i; i++)
                    {
                        string colunmname = myReader.GetName(i);
                        if (table.Columns.Contains(colunmname))
                        {
                            int j = 2;
                            while (table.Columns.Contains(colunmname + "_" + j)) { j++; }
                            colunmname = colunmname + "_" + j;
                        }
                        table.Columns.Add(colunmname, myReader.GetFieldType(i));
                    }

                    while (myReader.Read())
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; myReader.FieldCount > i; i++)
                        {
                            row[i] = myReader[i];
                        }
                        table.Rows.Add(row);
                    }
                    tables.Add(table);

                } while (myReader.NextResult());

                return tables.ToArray();
            }
        }

        public int RunNonQuery(string storeProcedure, params SqlParameter[] parameters)
        {
            using (SqlConnection myConnection = new SqlConnection(_conn))
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(storeProcedure, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    myCommand.Parameters.AddRange(parameters.ToArray());
                }

                int c = myCommand.ExecuteNonQuery();
                return c;
            }
        }

        public DataTable[] RunQueryStatement(string statement, params SqlParameter[] parameters)
        {
            using (SqlConnection myConnection = new SqlConnection(_conn))
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(statement, myConnection);

                //foreach (SqlParameter p in parameters)
                //{
                //    myCommand.Parameters.AddWithValue(p.ParameterName, p.Value);
                //}

                if (parameters != null && parameters.Length > 0)
                {
                    myCommand.Parameters.AddRange(parameters.ToArray());
                }

                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();

                List<DataTable> tables = new List<DataTable>();

                do
                {
                    DataTable table = new DataTable();
                    //for (int i = 0; myReader.FieldCount > i; i++)
                    //{
                    //    table.Columns.Add(myReader.GetName(i), myReader.GetFieldType(i));
                    //}
                    for (int i = 0; myReader.FieldCount > i; i++)
                    {
                        string colunmname = myReader.GetName(i);
                        if (table.Columns.Contains(colunmname))
                        {
                            int j = 2;
                            while (table.Columns.Contains(colunmname + "_" + j)) { j++; }
                            colunmname = colunmname + "_" + j;
                        }
                        table.Columns.Add(colunmname, myReader.GetFieldType(i));
                    }

                    while (myReader.Read())
                    {
                        DataRow row = table.NewRow();
                        foreach (DataColumn colunm in table.Columns)
                        {
                            row[colunm.ColumnName] = myReader[colunm.ColumnName];
                        }
                        table.Rows.Add(row);
                    }
                    tables.Add(table);
                } while (myReader.NextResult());

                return tables.ToArray();
            }
        }
    }
}

