using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading.Tasks;


namespace BYT.WS.AltYapi
{
    public class ServiceHelper
    {
     
      
        //Service reference
        public static ReferansVerileri.GumrukReferansVerileriClient GetReferansVeriWS(string UserName, string Password)
        {
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
          
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
            binding.MaxBufferSize = 6553600;
            binding.MaxReceivedMessageSize = 6553600;
            EndpointAddress address = new EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/GumrukReferansVeri/Provider/GumrukReferansVerileriWS");
            ReferansVerileri.GumrukReferansVerileriClient client = new ReferansVerileri.GumrukReferansVerileriClient(binding, address);

            //client.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["user"];
            //client.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["password

            client.ClientCredentials.UserName.UserName = UserName;
            client.ClientCredentials.UserName.Password = Password;

            return client;
        }

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
      
        //public static HatBildirHizmeti GetHatBildirWsClient()
        //{
        //    BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
        //    binding.SendTimeout = TimeSpan.FromSeconds(125);
        //    binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        //    binding.Security.Transport.Realm = "";
        //    binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
        //    binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
        //    binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

        //    EndpointAddress address = new EndpointAddress("https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/HatBildirWS");
        //    HatBildirHizmeti client = new HatBildirHizmeti();
        //    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["egeuser"], ConfigurationManager.AppSettings["egepassword"]);
        //    return client;
        //}

//        public static BasicHttpsBinding_Gumruk_Biztalk_IGHB GetIGHBWSClient()
//        {
//            BasicHttpsBinding_Gumruk_Biztalk_IGHB client = new BasicHttpsBinding_Gumruk_Biztalk_IGHB();

//#if (DEBUG)
//            client.Url = "https://wstest.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/IGHB";
//#else
//            client.Url = "https://ws.gtb.gov.tr:8443/EXT/Gumruk/EGE/Provider/IGHB";
//#endif

//            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["egeuser"], ConfigurationManager.AppSettings["egepassword"]);
//            return client;
//        }

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
           
            SonucHizmeti.GumrukWSSoapClient client = new SonucHizmeti.GumrukWSSoapClient(binding,address);


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

        public static DolasimBelgeHizmeti.TobbMedosPortTypeClient GetDolasimBelgeWSClient(string UserName, string Password)
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
            //  address = new EndpointAddress("https://medos.tobb.org.tr/services/service20200427.php");
            address = new EndpointAddress("https://medos.tobb.org.tr/services/service20200427.php");

#else
             address = new EndpointAddress("https://medos.tobb.org.tr/services/service20200427.php");
          
#endif
            DolasimBelgeHizmeti.TobbMedosPortTypeClient client = new DolasimBelgeHizmeti.TobbMedosPortTypeClient(binding, address);
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
