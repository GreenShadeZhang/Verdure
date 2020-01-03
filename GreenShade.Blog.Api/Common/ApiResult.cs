using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GreenShade.Blog.Api.Common
{
    public class ApiResult<T>
    {
        public ApiResult()
        {

        }
        public string Msg { get; set; }
        public HttpStatusCode Code { get; set; }
        public T Result { get; set; }

        public static ApiResult<T> Ok(T result = default(T))
        {
            return new ApiResult<T>() { Code = HttpStatusCode.OK, Msg = "", Result = result };
        }
        public static ApiResult<T> Ok(string msg = "")
        {
            return new ApiResult<T>() { Code = HttpStatusCode.OK, Msg = msg };
        }

        public static ApiResult<T> Fail(string msg = "")
        {
            return new ApiResult<T>() { Code = HttpStatusCode.OK, Msg = msg };
        }
        public static ApiResult<T> Fail(HttpStatusCode code, string msg = "")
        {
            return new ApiResult<T>() { Code = code, Msg =  msg };
        }
    }

    public class ApiResult : ApiResult<string>
    {
        private string _result;
        public ApiResult()
        {
        }
        public static ApiResult Ok(string result)
        {
            return new ApiResult() { Code = HttpStatusCode.OK, Result = result };
        }
        public static ApiResult Fail(string msg = "")
        {
            return new ApiResult() { Code = HttpStatusCode.NoContent, Msg = msg };
        }
        public string Result
        {
            get
            {
                return _result ?? (_result = string.Empty);
            }
            set
            {
                _result = value;
            }
        }
    }
}
