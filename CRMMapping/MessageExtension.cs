using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using NServiceBus;

namespace CRMMapping.CustomExtensions
{
    public static class CrMMessageExtension
    {

        public static string GetCrmValue(this IMessage message, Entity entity, string key)
        {
            
            if (entity.Contains(key))
            {
                return entity[key].ToString();
            }
            else
            {

                return string.Empty;
            }

            
        }


    }
}
