using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TLS_AD_tool
{
    class netcheck
    {
        public static string PingResult(string ip)
        {
            Ping p = new Ping();
            int timeout = 1000;
            PingReply reply;
            try
            {
                reply = p.Send(ip, timeout);
            }
            catch (Exception)
            {

                return "AD connect failed plz try again later";
            }
            
            return reply.Status.ToString();
        }
    }
}
