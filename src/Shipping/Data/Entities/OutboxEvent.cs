using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingApi.Data.Entities
{
    public class OutboxEvent
    {
        public OutboxEvent()
        {
        }

        public OutboxEvent(string data, string routingKey)
        {
            this.OccurredOn = DateTime.Now;
            this.Data = data;
            this.RoutingKey = routingKey;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Data { get; set; }
        public string Type { get; set; }
        public string RoutingKey { get; set; }
        public DateTime OccurredOn { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public string Error { get; set; }
    }
}