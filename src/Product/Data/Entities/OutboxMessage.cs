using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ProductApi.Data.Entities
{
    public class OutboxMessage
    {
        public OutboxMessage()
        {

        }

        public OutboxMessage(string data, string url, HttpMethod method)
        {
            this.OccurredOn = DateTime.Now;
            this.HttpMethod = method;
            this.Data = data;
            this.Url = url;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Data { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string Url { get; set; }
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Error { get; set; }
    }
}