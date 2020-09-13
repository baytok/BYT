using BYT.UI.KpsWs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;


namespace BYT.UI
{
    public class ServiceHelper
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

                UI.KpsWs.KisiBilgisiSorgulaOUT kimlikOut = GetKimlikBilgiServiceClient().KisiBilgiSorgula(userinfo, tckn);
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

                UI.KpsWs.NCuzdaniSorgulaOUT cuzdanOut = GetKimlikBilgiServiceClient().NCuzdaniSorgula(nCuzdan);
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

                UI.KpsWs.TCKKBilgisiOUT tckkOut = GetKimlikBilgiServiceClient().TCKKBilgisi(tckkBilgi);
                return tckkOut.TCKKBilgisi;
            }
        }

        //Service reference
        //public static ReferansVerileri.GumrukReferansVerileriClient GetReferansVeriWS(string UserName, string Password)
        //{
        //    BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
        //    binding.SendTimeout = TimeSpan.FromSeconds(125);
        //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        //    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
        //    //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
        //    //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        //    binding.MaxBufferSize = 6553600;
        //    binding.MaxReceivedMessageSize = 6553600;
        //    EndpointAddress address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/GumrukReferansVeri/Provider/GumrukReferansVerileriWS");
        //    ReferansVerileri.GumrukReferansVerileriClient client = new ReferansVerileri.GumrukReferansVerileriClient(binding, address);

        //    //client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["user"];
        //    //client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["password

        //    client.ClientCredentials.UserName.UserName = UserName;
        //    client.ClientCredentials.UserName.Password = Password;

        //    return client;
        //}

        //        public static UI.ETicaretEGE_Test.GumrukETApp_ETOrch_ReceiveInpPortClient GetEticaretBeyanWSClient(string UserName, string Password)
        //        {
        //            EndpointAddress address;
        //            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
        //            binding.SendTimeout = TimeSpan.FromSeconds(125);
        //            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        //            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
        //            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
        //            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
        //            binding.MaxReceivedMessageSize = 2147483647;

        //#if (DEBUG)
        //            address = new EndpointAddress("https://wstest.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/ETicaretWS");

        //#else
        //             address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/ETicaretWS");

        //#endif
        //            UI.ETicaretEGE_Test.GumrukETApp_ETOrch_ReceiveInpPortClient client = new UI.ETicaretEGE_Test.GumrukETApp_ETOrch_ReceiveInpPortClient(binding, address);
        //            client.ClientCredentials.UserName.UserName = UserName;
        //            client.ClientCredentials.UserName.Password = Password;

        //            return client;
        //        }
        public static TescilHizmeti.Gumruk_Biztalk_EImzaTescil_Tescil_PortTescilSoapClient GetTescilWSClient(string UserName, string Password)
        {
            EndpointAddress address;
            //  BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly); 8080 için
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;

#if (DEBUG)
            //  address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/TescilWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/TescilWS");
#else
             address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/TescilWS");
          
#endif
            TescilHizmeti.Gumruk_Biztalk_EImzaTescil_Tescil_PortTescilSoapClient client = new TescilHizmeti.Gumruk_Biztalk_EImzaTescil_Tescil_PortTescilSoapClient(binding, address);
            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            return client;
        }
        public static KontrolHizmeti.Gumruk_Biztalk_EImzaTescil_Kontrol_KontrolTalepPortSoapClient GetKontrolWsClient(string UserName, string Password)
        {
            EndpointAddress address;

            //  BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly); 8080 için
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;
#if (DEBUG)
            // address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/KontrolHizmetiWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/KontrolHizmetiWS");

#else
             address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/KontrolHizmetiWS");
          
#endif


            KontrolHizmeti.Gumruk_Biztalk_EImzaTescil_Kontrol_KontrolTalepPortSoapClient client = new KontrolHizmeti.Gumruk_Biztalk_EImzaTescil_Kontrol_KontrolTalepPortSoapClient(binding, address);




            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            return client;

        }

        public static SonucHizmeti.GumrukWSSoapClient GetSonucWSClient(string UserName, string Password)
        {

            EndpointAddress address;
            //  BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly); 8080 için
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;
#if (DEBUG)
            //  address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/GumrukWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/GumrukWS");

#else
             address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/GumrukWS");
            
#endif

            SonucHizmeti.GumrukWSSoapClient client = new SonucHizmeti.GumrukWSSoapClient(binding, address);


            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;
            return client;
        }

        public static NctsHizmeti.WS2ServiceClient GetNctsWSClient(string UserName, string Password)
        {
            EndpointAddress address;
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;

#if (DEBUG)
            address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/NCTS/Provider/NCTS2SWS?wsdl");

#else
             address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/NCTS/Provider/NCTS2SWS?wsdl");
          
#endif
            NctsHizmeti.WS2ServiceClient client = new NctsHizmeti.WS2ServiceClient(binding, address);
            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            return client;
        }
        public static OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient GetOzetBeyanWSClient(string UserName, string Password)
        {
            EndpointAddress address;
            //   BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly); 8080 hhtp gönderileceği zaman
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;

#if (DEBUG)
            //  address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/OzetBeyanWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/OzetBeyanWS");
            //   address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/OzetBeyanWS");

#else
             address = new EndpointAddress("https://wstest.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/OzetBeyanWS");
          
#endif
            OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient client = new OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient(binding, address);
            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            return client;
        }

        public static MesaiHizmeti.MesaiHizmetiClient GetMesaiBildirWsClient(string UserName, string Password)
        {
            EndpointAddress address;
            //  BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly); 8080 için
            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;
#if (DEBUG)
            //  address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/FazlaMesaiWS");
            // address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/FazlaMesaiWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/FazlaMesaiWS");

#else
             address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/FazlaMesaiWS");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/FazlaMesaiWS");

 
#endif

            MesaiHizmeti.MesaiHizmetiClient client = new MesaiHizmeti.MesaiHizmetiClient(binding, address);
            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            //client.ClientCredentials.ClientCertificate.SetCertificate(
            //   StoreLocation.CurrentUser,
            //   StoreName.CertificateAuthority,
            //   X509FindType.FindBySubjectName,
            //    "gtb.gov.tr");

            return client;
        }

        public static IGHBHizmeti.Gumruk_Biztalk_IGHBClient GetIGHBWSClient(string UserName, string Password)
        {


            EndpointAddress address;

            BasicHttpsBinding binding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            //   binding.Security.Mode= SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.TripleDes;
            binding.MaxReceivedMessageSize = 2147483647;
#if (DEBUG)
            //  address = new EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/IGHB");
            address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/IGHB");

#else
             address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/IGHB");
          
#endif
            IGHBHizmeti.Gumruk_Biztalk_IGHBClient client = new IGHBHizmeti.Gumruk_Biztalk_IGHBClient(binding, address);
            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;
            //client.ClientCredentials.ClientCertificate.SetCertificate(
            // StoreLocation.CurrentUser,
            // StoreName.My,
            // X509FindType.FindBySubjectName,
            //  "gtb.gov.tr");

            return client;
        }
    }
}
