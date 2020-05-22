﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NctsHizmeti
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws/", ConfigurationName="NctsHizmeti.WS2Service")]
    public interface WS2Service
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getMessagesListByGuidRequest", ReplyAction="http://ws/WS2Service/getMessagesListByGuidResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.getMessagesListByGuidResponse> getMessagesListByGuidAsync(NctsHizmeti.getMessagesListByGuidRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getNotReadMessagesListRequest", ReplyAction="http://ws/WS2Service/getNotReadMessagesListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.getNotReadMessagesListResponse> getNotReadMessagesListAsync(NctsHizmeti.getNotReadMessagesListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/downloadmessagebyindexRequest", ReplyAction="http://ws/WS2Service/downloadmessagebyindexResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.downloadmessagebyindexResponse> downloadmessagebyindexAsync(NctsHizmeti.downloadmessagebyindexRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/submitdeclarationRequest", ReplyAction="http://ws/WS2Service/submitdeclarationResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.submitdeclarationResponse> submitdeclarationAsync(NctsHizmeti.submitdeclarationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebylrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebylrnResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebylrnResponse> uploadmessagebylrnAsync(NctsHizmeti.uploadmessagebylrnRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebymrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebymrnResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebymrnResponse> uploadmessagebymrnAsync(NctsHizmeti.uploadmessagebymrnRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws/")]
    public partial class result
    {
        
        private string corrGuidField;
        
        private string errorField;
        
        private string[] listField;
        
        private string msgContentField;
        
        private string msgIdField;
        
        private string msgStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string corrGuid
        {
            get
            {
                return this.corrGuidField;
            }
            set
            {
                this.corrGuidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("list", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=2)]
        public string[] list
        {
            get
            {
                return this.listField;
            }
            set
            {
                this.listField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string msgContent
        {
            get
            {
                return this.msgContentField;
            }
            set
            {
                this.msgContentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string msgId
        {
            get
            {
                return this.msgIdField;
            }
            set
            {
                this.msgIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string msgStatus
        {
            get
            {
                return this.msgStatusField;
            }
            set
            {
                this.msgStatusField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getMessagesListByGuid", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getMessagesListByGuidRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CORR_GUID;
        
        public getMessagesListByGuidRequest()
        {
        }
        
        public getMessagesListByGuidRequest(string FIRM_ID, string USER_ID, string CORR_GUID)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.CORR_GUID = CORR_GUID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getMessagesListByGuidResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getMessagesListByGuidResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public getMessagesListByGuidResponse()
        {
        }
        
        public getMessagesListByGuidResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNotReadMessagesList", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getNotReadMessagesListRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        public getNotReadMessagesListRequest()
        {
        }
        
        public getNotReadMessagesListRequest(string FIRM_ID, string USER_ID)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNotReadMessagesListResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getNotReadMessagesListResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public getNotReadMessagesListResponse()
        {
        }
        
        public getNotReadMessagesListResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="downloadmessagebyindex", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class downloadmessagebyindexRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string INDEX;
        
        public downloadmessagebyindexRequest()
        {
        }
        
        public downloadmessagebyindexRequest(string FIRM_ID, string USER_ID, string INDEX)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.INDEX = INDEX;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="downloadmessagebyindexResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class downloadmessagebyindexResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public downloadmessagebyindexResponse()
        {
        }
        
        public downloadmessagebyindexResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitdeclaration", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class submitdeclarationRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_TYPE;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SIGN_FLAG;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_CONTENT;
        
        public submitdeclarationRequest()
        {
        }
        
        public submitdeclarationRequest(string FIRM_ID, string USER_ID, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitdeclarationResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class submitdeclarationResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public submitdeclarationResponse()
        {
        }
        
        public submitdeclarationResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebylrn", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebylrnRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LRN;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_TYPE;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SIGN_FLAG;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_CONTENT;
        
        public uploadmessagebylrnRequest()
        {
        }
        
        public uploadmessagebylrnRequest(string FIRM_ID, string USER_ID, string LRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.LRN = LRN;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebylrnResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebylrnResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public uploadmessagebylrnResponse()
        {
        }
        
        public uploadmessagebylrnResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebymrn", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebymrnRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MRN;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_TYPE;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=4)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SIGN_FLAG;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=5)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSG_CONTENT;
        
        public uploadmessagebymrnRequest()
        {
        }
        
        public uploadmessagebymrnRequest(string FIRM_ID, string USER_ID, string MRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.MRN = MRN;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebymrnResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebymrnResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NctsHizmeti.result @return;
        
        public uploadmessagebymrnResponse()
        {
        }
        
        public uploadmessagebymrnResponse(NctsHizmeti.result @return)
        {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface WS2ServiceChannel : NctsHizmeti.WS2Service, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class WS2ServiceClient : System.ServiceModel.ClientBase<NctsHizmeti.WS2Service>, NctsHizmeti.WS2Service
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WS2ServiceClient() : 
                base(WS2ServiceClient.GetDefaultBinding(), WS2ServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WS2ServicePort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WS2ServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(WS2ServiceClient.GetBindingForEndpoint(endpointConfiguration), WS2ServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WS2ServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WS2ServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WS2ServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WS2ServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WS2ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.getMessagesListByGuidResponse> NctsHizmeti.WS2Service.getMessagesListByGuidAsync(NctsHizmeti.getMessagesListByGuidRequest request)
        {
            return base.Channel.getMessagesListByGuidAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.getMessagesListByGuidResponse> getMessagesListByGuidAsync(string FIRM_ID, string USER_ID, string CORR_GUID)
        {
            NctsHizmeti.getMessagesListByGuidRequest inValue = new NctsHizmeti.getMessagesListByGuidRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.CORR_GUID = CORR_GUID;
            return ((NctsHizmeti.WS2Service)(this)).getMessagesListByGuidAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.getNotReadMessagesListResponse> NctsHizmeti.WS2Service.getNotReadMessagesListAsync(NctsHizmeti.getNotReadMessagesListRequest request)
        {
            return base.Channel.getNotReadMessagesListAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.getNotReadMessagesListResponse> getNotReadMessagesListAsync(string FIRM_ID, string USER_ID)
        {
            NctsHizmeti.getNotReadMessagesListRequest inValue = new NctsHizmeti.getNotReadMessagesListRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            return ((NctsHizmeti.WS2Service)(this)).getNotReadMessagesListAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.downloadmessagebyindexResponse> NctsHizmeti.WS2Service.downloadmessagebyindexAsync(NctsHizmeti.downloadmessagebyindexRequest request)
        {
            return base.Channel.downloadmessagebyindexAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.downloadmessagebyindexResponse> downloadmessagebyindexAsync(string FIRM_ID, string USER_ID, string INDEX)
        {
            NctsHizmeti.downloadmessagebyindexRequest inValue = new NctsHizmeti.downloadmessagebyindexRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.INDEX = INDEX;
            return ((NctsHizmeti.WS2Service)(this)).downloadmessagebyindexAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.submitdeclarationResponse> NctsHizmeti.WS2Service.submitdeclarationAsync(NctsHizmeti.submitdeclarationRequest request)
        {
            return base.Channel.submitdeclarationAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.submitdeclarationResponse> submitdeclarationAsync(string FIRM_ID, string USER_ID, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            NctsHizmeti.submitdeclarationRequest inValue = new NctsHizmeti.submitdeclarationRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((NctsHizmeti.WS2Service)(this)).submitdeclarationAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebylrnResponse> NctsHizmeti.WS2Service.uploadmessagebylrnAsync(NctsHizmeti.uploadmessagebylrnRequest request)
        {
            return base.Channel.uploadmessagebylrnAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebylrnResponse> uploadmessagebylrnAsync(string FIRM_ID, string USER_ID, string LRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            NctsHizmeti.uploadmessagebylrnRequest inValue = new NctsHizmeti.uploadmessagebylrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.LRN = LRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((NctsHizmeti.WS2Service)(this)).uploadmessagebylrnAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebymrnResponse> NctsHizmeti.WS2Service.uploadmessagebymrnAsync(NctsHizmeti.uploadmessagebymrnRequest request)
        {
            return base.Channel.uploadmessagebymrnAsync(request);
        }
        
        public System.Threading.Tasks.Task<NctsHizmeti.uploadmessagebymrnResponse> uploadmessagebymrnAsync(string FIRM_ID, string USER_ID, string MRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT)
        {
            NctsHizmeti.uploadmessagebymrnRequest inValue = new NctsHizmeti.uploadmessagebymrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MRN = MRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((NctsHizmeti.WS2Service)(this)).uploadmessagebymrnAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.WS2ServicePort))
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
            if ((endpointConfiguration == EndpointConfiguration.WS2ServicePort))
            {
                return new System.ServiceModel.EndpointAddress("http://wstest.gtb.gov.tr:8080/EXT/Gumruk/NCTS/Provider/NCTS2SWS");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WS2ServiceClient.GetBindingForEndpoint(EndpointConfiguration.WS2ServicePort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WS2ServiceClient.GetEndpointAddress(EndpointConfiguration.WS2ServicePort);
        }
        
        public enum EndpointConfiguration
        {
            
            WS2ServicePort,
        }
    }
}
