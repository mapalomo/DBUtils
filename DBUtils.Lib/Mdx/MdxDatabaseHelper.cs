namespace DBUtils.Lib.Mdx
{

    using Microsoft.AnalysisServices.AdomdClient;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class MdxDatabaseHelper
    {
        private static string mdxConnectionString = ResourceMdx.CONNECTION_MDX_STRING;

        private static DataTable ExecuteMdx(string query, List<AdomdParameter> parameters = null)
        {
            DataTable dataTable = new DataTable();

            using (AdomdConnection sqlCon = new AdomdConnection(mdxConnectionString))
            {
                sqlCon.Open();

                string processedQuery = MdxQueryFormatter.GetQueryWithParameters(parameters, query);

                AdomdCommand mdxCmd = new AdomdCommand(processedQuery, sqlCon);
                using (AdomdDataAdapter dataAdapter = new AdomdDataAdapter(mdxCmd))
                {
                    dataAdapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static IEnumerable<T> ExecuteMdx<T>(string query, List<AdomdParameter> listParamMDX, Func<DataRow, T> mapAction)
        {
            DataTable dt = ExecuteMdx(query, listParamMDX);

            var predictions = dt.AsEnumerable().Select(mapAction);

            return predictions;
        }
    }
}
