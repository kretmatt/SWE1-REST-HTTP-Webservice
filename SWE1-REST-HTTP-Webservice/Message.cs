using System;

namespace SWE1_REST_HTTP_Webservice
{
    class Message
    {
        public int Id { get; set; }
        public String Content { get; set; }
        public DateTime SentDate { get; set; }

        public override string ToString()
        {
            return String.Format("{0}-{1}-{2}\n", Id, Content, SentDate);
        }
    }
}