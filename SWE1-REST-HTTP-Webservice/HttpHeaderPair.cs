using System;

namespace SWE1_REST_HTTP_Webservice
{
    /*
        HttpHeaderPair - A single header line in HTTP requests and responses.
     */
    public class HttpHeaderPair
    {
        public String HeaderKey { get; set; }
        public String HeaderValue { get; set; }


        public HttpHeaderPair(String headerKey,String headerValue)
        {
            HeaderKey = headerKey;
            HeaderValue = headerValue;
        }
        
        public override string ToString()
        {
            return String.Format("{0}: {1}", HeaderKey, HeaderValue);
        }
    }
}