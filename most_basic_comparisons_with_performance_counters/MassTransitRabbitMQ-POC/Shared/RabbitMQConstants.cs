using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class RabbitMqServerConstants
    {
        public const string RabbitMqUri = "rabbitmq://pvcd-api.pvops.com/";
        public const string UserName = "pvadmin";
        public const string Password = "pvadmin1$";
    }

    public static class RabbitMqQueueNames
    {
        public const string OrderPublicationQueue = "MTRbtMQ-POC.OrderPublicationQueue";
    }
}
