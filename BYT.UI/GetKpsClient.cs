using BYT.UI.KpsWs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI
{
    public static class GetKPSClient
    {
        public static KpsWS_PortTypeClient GetKimlikBilgiServiceClient()
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Transport.Realm = "";
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            EndpointAddress address = new EndpointAddress("http://ws.gtb.gov.tr:8080/GTB_KPSV2");
            KpsWS_PortTypeClient client = new KpsWS_PortTypeClient(binding, address);

            client.ClientCredentials.UserName.UserName = "TAKPASUser";
            client.ClientCredentials.UserName.Password = "s6bSuctKzn";

            return client;
        }

        public static KisiBilgisi GetKisiBilgisi(string tckn, string ip)
        {
            UserInfo userinfo = new UserInfo()
            {
                UserName = "SirketSorgulamaUser",
                UserIP = ip,
                ApplicationID = "KATBİS"
            };

            KpsWs.KisiBilgisiSorgulaOUT kimlikOut = GetKimlikBilgiServiceClient().KisiBilgiSorgula(userinfo, tckn);
            return kimlikOut.ListeleCokluResponse.ListeleCokluResult.SorguSonucu[0];
        }

        public static CuzdanBilgisi GetNufusCuzdan(string tckn, string ip)
        {
            UserInfo userinfo = new UserInfo()
            {
                UserName = "SirketSorgulamaUser",
                UserIP = ip,
                ApplicationID = "KATBİS"
            };

            NCuzdaniSorgulaIN nCuzdan = new NCuzdaniSorgulaIN()
            {
                UserInfo = userinfo,
                Tckno = tckn
            };

            KpsWs.NCuzdaniSorgulaOUT cuzdanOut = GetKimlikBilgiServiceClient().NCuzdaniSorgula(nCuzdan);
            if (cuzdanOut.ListeleCokluResult != null)
            {
                if (cuzdanOut.ListeleCokluResult.SorguSonucu != null)
                {
                    return cuzdanOut.ListeleCokluResult.SorguSonucu.CuzdanBilgisi;
                }
            }
            return null;
        }

        public static TCKKBilgisi GetTCKKBilgisi(string tckn, string ip)
        {
            UserInfo userinfo = new UserInfo()
            {
                UserName = "SirketSorgulamaUser",
                UserIP = ip,
                ApplicationID = "KATBİS"
            };

            TCKKBilgisiIN tckkBilgi = new TCKKBilgisiIN()
            {
                UserInfo = userinfo,
                TCKimlikNo = tckn
            };
          
            KpsWs.TCKKBilgisiOUT tckkOut = GetKimlikBilgiServiceClient().TCKKBilgisi(tckkBilgi);
            return tckkOut.TCKKBilgisi;
        }
    }
}
