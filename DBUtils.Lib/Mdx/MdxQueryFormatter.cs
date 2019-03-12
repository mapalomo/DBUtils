namespace DBUtils.Lib.Mdx
{
    using Microsoft.AnalysisServices.AdomdClient;
    using System.Collections.Generic;
    using System.Security;
    using System.Text.RegularExpressions;

    public static class MdxQueryFormatter
    {
        private static string pattern = ResourceMdx.REGEX_AVOID_SQL_INJECTION;

        public static string GetQueryWithParameters(List<AdomdParameter> parameters, string query)
        {
            var processedQuery = query;
            parameters.ForEach(parameter =>
            {
                GuardParam(parameter.Value);
                processedQuery = processedQuery.Replace($"@{parameter.ParameterName}", parameter.Value.ToString());
            });

            return processedQuery;
        }

        private static void GuardParam(object rawValue)
        {
            var value = rawValue.ToString();

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(value);

            if (matches.Count > 0)
                throw new SecurityException(ResourceMdx.SECURITY_SQL_INJECTION_ERROR);
        }
    }
}