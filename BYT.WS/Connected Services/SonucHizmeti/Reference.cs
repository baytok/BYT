﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SonucHizmeti
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SonucHizmeti.GumrukWSSoap")]
    public interface GumrukWSSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string IslemTipi, string IslemSonucu, string BasIslemGunu);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSorgula1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula1Async(string KullaniciAdi, string KullaniciSifre, string IslemTipi, string BasIslemGunu);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSorgula2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula2Async(string KullaniciAdi, string KullaniciSifre, string BasIslemGunu);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSorgula3", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula3Async(string KullaniciAdi, string KullaniciSifre, string RefID, string BasIslemGunu);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSonucGetir", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetirAsync(string GUIDof, string KullaniciAdi, string KullaniciSifre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSonucGetir1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir1Async(string RefID, string KullaniciAdi, string KullaniciSifre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSonucGetir2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir2Async(string GUIDof, string KullaniciAdi, string KullaniciSifre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSonucGetir3", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir3Async(string RefID, string KullaniciAdi, string KullaniciSifre);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemSonucGetir4", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir4Async(string GUIDof);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SonuclanmamisIslemdekiMesajiIptalet", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> SonuclanmamisIslemdekiMesajiIptaletAsync(string KullaniciAdi, string KullaniciSifre, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IslemdeKalmisGeciciNumaraSil", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> IslemdeKalmisGeciciNumaraSilAsync(string KullaniciAdi, string KullaniciSifre, string Beyanname_No, string GUID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TescilSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> TescilSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string GUID, string Gumruk, string Müsavir_referansi);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ozetBeyanTescilSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanTescilSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string tarih);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ozetBeyanTescilSorgulaEmsg", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanTescilSorgulaEmsgAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string tarih);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ozetBeyanDurumSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanDurumSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ozbyMuayeneMemuruAdiSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozbyMuayeneMemuruAdiSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/tasinanOzbyBilgiSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> tasinanOzbyBilgiSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string VergiNo, string TasitNo, string ReferansNo, string BasTarihi, string BitTarihi);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/tasinanOzbyBilgiSorgulaTsNoyaGore", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> tasinanOzbyBilgiSorgulaTsNoyaGoreAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TasimaSenediNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/mesajSorgula", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> mesajSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface GumrukWSSoapChannel : SonucHizmeti.GumrukWSSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class GumrukWSSoapClient : System.ServiceModel.ClientBase<SonucHizmeti.GumrukWSSoap>, SonucHizmeti.GumrukWSSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public GumrukWSSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(GumrukWSSoapClient.GetBindingForEndpoint(endpointConfiguration), GumrukWSSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GumrukWSSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(GumrukWSSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GumrukWSSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(GumrukWSSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public GumrukWSSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string IslemTipi, string IslemSonucu, string BasIslemGunu)
        {
            return base.Channel.IslemSorgulaAsync(KullaniciAdi, KullaniciSifre, IslemTipi, IslemSonucu, BasIslemGunu);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula1Async(string KullaniciAdi, string KullaniciSifre, string IslemTipi, string BasIslemGunu)
        {
            return base.Channel.IslemSorgula1Async(KullaniciAdi, KullaniciSifre, IslemTipi, BasIslemGunu);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula2Async(string KullaniciAdi, string KullaniciSifre, string BasIslemGunu)
        {
            return base.Channel.IslemSorgula2Async(KullaniciAdi, KullaniciSifre, BasIslemGunu);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSorgula3Async(string KullaniciAdi, string KullaniciSifre, string RefID, string BasIslemGunu)
        {
            return base.Channel.IslemSorgula3Async(KullaniciAdi, KullaniciSifre, RefID, BasIslemGunu);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetirAsync(string GUIDof, string KullaniciAdi, string KullaniciSifre)
        {
            return base.Channel.IslemSonucGetirAsync(GUIDof, KullaniciAdi, KullaniciSifre);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir1Async(string RefID, string KullaniciAdi, string KullaniciSifre)
        {
            return base.Channel.IslemSonucGetir1Async(RefID, KullaniciAdi, KullaniciSifre);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir2Async(string GUIDof, string KullaniciAdi, string KullaniciSifre)
        {
            return base.Channel.IslemSonucGetir2Async(GUIDof, KullaniciAdi, KullaniciSifre);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir3Async(string RefID, string KullaniciAdi, string KullaniciSifre)
        {
            return base.Channel.IslemSonucGetir3Async(RefID, KullaniciAdi, KullaniciSifre);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> IslemSonucGetir4Async(string GUIDof)
        {
            return base.Channel.IslemSonucGetir4Async(GUIDof);
        }
        
        public System.Threading.Tasks.Task<string> SonuclanmamisIslemdekiMesajiIptaletAsync(string KullaniciAdi, string KullaniciSifre, string GUID)
        {
            return base.Channel.SonuclanmamisIslemdekiMesajiIptaletAsync(KullaniciAdi, KullaniciSifre, GUID);
        }
        
        public System.Threading.Tasks.Task<string> IslemdeKalmisGeciciNumaraSilAsync(string KullaniciAdi, string KullaniciSifre, string Beyanname_No, string GUID)
        {
            return base.Channel.IslemdeKalmisGeciciNumaraSilAsync(KullaniciAdi, KullaniciSifre, Beyanname_No, GUID);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> TescilSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string GUID, string Gumruk, string Müsavir_referansi)
        {
            return base.Channel.TescilSorgulaAsync(KullaniciAdi, KullaniciSifre, GUID, Gumruk, Müsavir_referansi);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanTescilSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string tarih)
        {
            return base.Channel.ozetBeyanTescilSorgulaAsync(KullaniciAdi, KullaniciSifre, Gumruk, tarih);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanTescilSorgulaEmsgAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string tarih)
        {
            return base.Channel.ozetBeyanTescilSorgulaEmsgAsync(KullaniciAdi, KullaniciSifre, Gumruk, tarih);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozetBeyanDurumSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo)
        {
            return base.Channel.ozetBeyanDurumSorgulaAsync(KullaniciAdi, KullaniciSifre, Gumruk, TescilNo);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> ozbyMuayeneMemuruAdiSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo)
        {
            return base.Channel.ozbyMuayeneMemuruAdiSorgulaAsync(KullaniciAdi, KullaniciSifre, Gumruk, TescilNo);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> tasinanOzbyBilgiSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string VergiNo, string TasitNo, string ReferansNo, string BasTarihi, string BitTarihi)
        {
            return base.Channel.tasinanOzbyBilgiSorgulaAsync(KullaniciAdi, KullaniciSifre, Gumruk, VergiNo, TasitNo, ReferansNo, BasTarihi, BitTarihi);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> tasinanOzbyBilgiSorgulaTsNoyaGoreAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TasimaSenediNo)
        {
            return base.Channel.tasinanOzbyBilgiSorgulaTsNoyaGoreAsync(KullaniciAdi, KullaniciSifre, Gumruk, TasimaSenediNo);
        }
        
        public System.Threading.Tasks.Task<SonucHizmeti.ArrayOfXElement> mesajSorgulaAsync(string KullaniciAdi, string KullaniciSifre, string Gumruk, string TescilNo)
        {
            return base.Channel.mesajSorgulaAsync(KullaniciAdi, KullaniciSifre, Gumruk, TescilNo);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.GumrukWSSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.GumrukWSSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.GumrukWSSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/GumrukWS");
            }
            if ((endpointConfiguration == EndpointConfiguration.GumrukWSSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/GumrukWS");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            GumrukWSSoap,
            
            GumrukWSSoap12,
        }
    }
    
    [System.Xml.Serialization.XmlSchemaProviderAttribute(null, IsAny=true)]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil-lib", "2.0.0.2")]
    public partial class ArrayOfXElement : object, System.Xml.Serialization.IXmlSerializable
    {
        
        private System.Collections.Generic.List<System.Xml.Linq.XElement> nodesList = new System.Collections.Generic.List<System.Xml.Linq.XElement>();
        
        public ArrayOfXElement()
        {
        }
        
        public virtual System.Collections.Generic.List<System.Xml.Linq.XElement> Nodes
        {
            get
            {
                return this.nodesList;
            }
        }
        
        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            System.Collections.Generic.IEnumerator<System.Xml.Linq.XElement> e = nodesList.GetEnumerator();
            for (
            ; e.MoveNext(); 
            )
            {
                ((System.Xml.Serialization.IXmlSerializable)(e.Current)).WriteXml(writer);
            }
        }
        
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            for (
            ; (reader.NodeType != System.Xml.XmlNodeType.EndElement); 
            )
            {
                if ((reader.NodeType == System.Xml.XmlNodeType.Element))
                {
                    System.Xml.Linq.XElement elem = new System.Xml.Linq.XElement("default");
                    ((System.Xml.Serialization.IXmlSerializable)(elem)).ReadXml(reader);
                    Nodes.Add(elem);
                }
                else
                {
                    reader.Skip();
                }
            }
        }
    }
}
