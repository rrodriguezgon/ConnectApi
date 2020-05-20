using Microsoft.Extensions.Options;
using Seedwork.CrossCutting.HttpClientInvoker.Configuration;
using Seedwork.CrossCutting.HttpClientInvoker.RestfulApiClient;
using Seedwork.CrossCutting.HttpClientInvoker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace OrderMailboxHub.Application.Services.Common
{
    public abstract class MgmtService<TDto, TApiConfiguration>
         : RestfulApiClientService<int, TDto, TApiConfiguration>
         , IMgmtService<TDto>
         where TDto : class, new()
         where TApiConfiguration : ApiConfiguration, new()
    {
        public MgmtService(IOptions<TApiConfiguration> apiConfiguration, IRestClientBase restClientBase, IPolicyFaultSolver policyFaultSolver)
            : base(apiConfiguration, restClientBase, policyFaultSolver)
        { }
        protected string ConcatenateParametersOld(Object parametersObj, List<string> propertiesExclude = null)
        {
            string url = string.Empty;
            foreach (PropertyInfo pInfo in parametersObj.GetType().GetProperties())
            {
                if (propertiesExclude == null || (propertiesExclude != null && !propertiesExclude.Contains(pInfo.Name)))
                {
                    var value = pInfo.GetValue(parametersObj);
                    if (value != null)
                        url += (pInfo.PropertyType == typeof(DateTime?) || pInfo.PropertyType == typeof(DateTime)) ? $"{pInfo.Name}={DateTime.Parse(value.ToString()).ToString("yyyy-MM-dd")}&"
                                                                                                                    : $"{pInfo.Name}={value.ToString()}&";
                }
            }
            return url.Length > 0 ? url.Remove(url.Length - 1) : url;
        }
        protected string ConcatenateParameters(Object parametersObj, List<string> propertiesExclude = null)
        {
            string url = string.Empty;
            foreach (PropertyInfo pInfo in parametersObj.GetType().GetProperties())
            {
                if (propertiesExclude == null || (propertiesExclude != null && !propertiesExclude.Contains(pInfo.Name)))
                {
                    var value = pInfo.GetValue(parametersObj);

                    if (value != null || (value is IEnumerable && (value as IEnumerable).Cast<object>().Any()))
                        if (value is IEnumerable && value.GetType() != typeof(string))
                        {
                            string separator = $"&{pInfo.Name}=";
                            var valueType = value.GetType();
                            var valueElemType = valueType.IsGenericType
                                                    ? valueType.GetGenericArguments()[0]
                                                    : valueType.GetElementType();
                            if (valueElemType.IsPrimitive)
                            {
                                var enumerable = value as IEnumerable;
                                url += $"{pInfo.Name}=" + string.Join(separator, enumerable.Cast<object>());
                            }
                            else if (valueElemType == typeof(DateTime))
                            {
                                var enumerable = value as IEnumerable;
                                IList<string> fechasSt = new List<string>();
                                IEnumerable<DateTime> enu = enumerable.Cast<DateTime>();
                                foreach (var date in enu)
                                {
                                    fechasSt.Add(date.ToString("s"));
                                }
                                url += $"{pInfo.Name}=" + string.Join(separator, fechasSt) + "&";

                            }
                        }
                        else
                        {
                            url += (pInfo.PropertyType == typeof(DateTime?) || pInfo.PropertyType == typeof(DateTime)) ? $"{pInfo.Name}={DateTime.Parse(value.ToString()).ToString("s")}&"
                                                                                                                    : $"{pInfo.Name}={value.ToString()}&";
                        }
                }
            }
            return url.Length > 0 ? url.Remove(url.Length - 1) : url;
        }
    }
}
