﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OzetBeyanHizmeti
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://Gumruk.BizTalk.Integration", ConfigurationName="OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap")]
    public interface Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://Gumruk.BizTalk.Integration/Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOz" +
            "etBeyanTalep/OzetBeyan", ReplyAction="http://Gumruk.BizTalk.Integration/Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOz" +
            "etBeyanTalep/OzetBeyan")]
        System.Threading.Tasks.Task<OzetBeyanHizmeti.OzetBeyanSoapOut> OzetBeyanAsync(OzetBeyanHizmeti.OzetBeyanSoapIn request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class OzetBeyanSoapIn
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="OzetBeyan", Namespace="http://Gumruk.BizTalk.Integration", Order=0)]
        public OzetBeyanHizmeti.OzetBeyanSoapInBody Body;
        
        public OzetBeyanSoapIn()
        {
        }
        
        public OzetBeyanSoapIn(OzetBeyanHizmeti.OzetBeyanSoapInBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/BizTalk/2003/Any")]
    public partial class OzetBeyanSoapInBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement Root;
        
        public OzetBeyanSoapInBody()
        {
        }
        
        public OzetBeyanSoapInBody(System.Xml.XmlElement Root)
        {
            this.Root = Root;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class OzetBeyanSoapOut
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="OzetBeyanResponse", Namespace="http://Gumruk.BizTalk.Integration", Order=0)]
        public OzetBeyanHizmeti.OzetBeyanSoapOutBody Body;
        
        public OzetBeyanSoapOut()
        {
        }
        
        public OzetBeyanSoapOut(OzetBeyanHizmeti.OzetBeyanSoapOutBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/BizTalk/2003/Any")]
    public partial class OzetBeyanSoapOutBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.XmlElement Root;
        
        public OzetBeyanSoapOutBody()
        {
        }
        
        public OzetBeyanSoapOutBody(System.Xml.XmlElement Root)
        {
            this.Root = Root;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapChannel : OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient : System.ServiceModel.ClientBase<OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap>, OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient() : 
                base(Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetDefaultBinding(), Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetBindingForEndpoint(endpointConfiguration), Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<OzetBeyanHizmeti.OzetBeyanSoapOut> OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap.OzetBeyanAsync(OzetBeyanHizmeti.OzetBeyanSoapIn request)
        {
            return base.Channel.OzetBeyanAsync(request);
        }
        
        public System.Threading.Tasks.Task<OzetBeyanHizmeti.OzetBeyanSoapOut> OzetBeyanAsync(System.Xml.XmlElement Root)
        {
            OzetBeyanHizmeti.OzetBeyanSoapIn inValue = new OzetBeyanHizmeti.OzetBeyanSoapIn();
            inValue.Body = new OzetBeyanHizmeti.OzetBeyanSoapInBody();
            inValue.Body.Root = Root;
            return ((OzetBeyanHizmeti.Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap)(this)).OzetBeyanAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://ws.gtb.gov.tr:8080/EXT/Gumruk/EGE/Provider/OzetBeyanWS");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoapClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_Gumruk_Biztalk_EImzaTescil_YeniOzetBeyan_YeniOzetBeyanTalepSoap,
        }
    }
}