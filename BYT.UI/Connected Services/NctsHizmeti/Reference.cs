﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BYT.UI.NctsHizmeti {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws/", ConfigurationName="NctsHizmeti.WS2Service")]
    public interface WS2Service {
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getMessagesListByGuidRequest", ReplyAction="http://ws/WS2Service/getMessagesListByGuidResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.getMessagesListByGuidResponse getMessagesListByGuid(BYT.UI.NctsHizmeti.getMessagesListByGuidRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getMessagesListByGuidRequest", ReplyAction="http://ws/WS2Service/getMessagesListByGuidResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getMessagesListByGuidResponse> getMessagesListByGuidAsync(BYT.UI.NctsHizmeti.getMessagesListByGuidRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getNotReadMessagesListRequest", ReplyAction="http://ws/WS2Service/getNotReadMessagesListResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.getNotReadMessagesListResponse getNotReadMessagesList(BYT.UI.NctsHizmeti.getNotReadMessagesListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/getNotReadMessagesListRequest", ReplyAction="http://ws/WS2Service/getNotReadMessagesListResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getNotReadMessagesListResponse> getNotReadMessagesListAsync(BYT.UI.NctsHizmeti.getNotReadMessagesListRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/downloadmessagebyindexRequest", ReplyAction="http://ws/WS2Service/downloadmessagebyindexResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.downloadmessagebyindexResponse downloadmessagebyindex(BYT.UI.NctsHizmeti.downloadmessagebyindexRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/downloadmessagebyindexRequest", ReplyAction="http://ws/WS2Service/downloadmessagebyindexResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.downloadmessagebyindexResponse> downloadmessagebyindexAsync(BYT.UI.NctsHizmeti.downloadmessagebyindexRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/submitdeclarationRequest", ReplyAction="http://ws/WS2Service/submitdeclarationResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.submitdeclarationResponse submitdeclaration(BYT.UI.NctsHizmeti.submitdeclarationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/submitdeclarationRequest", ReplyAction="http://ws/WS2Service/submitdeclarationResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.submitdeclarationResponse> submitdeclarationAsync(BYT.UI.NctsHizmeti.submitdeclarationRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebylrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebylrnResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.uploadmessagebylrnResponse uploadmessagebylrn(BYT.UI.NctsHizmeti.uploadmessagebylrnRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebylrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebylrnResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebylrnResponse> uploadmessagebylrnAsync(BYT.UI.NctsHizmeti.uploadmessagebylrnRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebymrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebymrnResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        BYT.UI.NctsHizmeti.uploadmessagebymrnResponse uploadmessagebymrn(BYT.UI.NctsHizmeti.uploadmessagebymrnRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws/WS2Service/uploadmessagebymrnRequest", ReplyAction="http://ws/WS2Service/uploadmessagebymrnResponse")]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebymrnResponse> uploadmessagebymrnAsync(BYT.UI.NctsHizmeti.uploadmessagebymrnRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://ws/")]
    public partial class result : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string corrGuidField;
        
        private string errorField;
        
        private string[] listField;
        
        private string msgContentField;
        
        private string msgIdField;
        
        private string msgStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string corrGuid {
            get {
                return this.corrGuidField;
            }
            set {
                this.corrGuidField = value;
                this.RaisePropertyChanged("corrGuid");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string error {
            get {
                return this.errorField;
            }
            set {
                this.errorField = value;
                this.RaisePropertyChanged("error");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("list", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true, Order=2)]
        public string[] list {
            get {
                return this.listField;
            }
            set {
                this.listField = value;
                this.RaisePropertyChanged("list");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string msgContent {
            get {
                return this.msgContentField;
            }
            set {
                this.msgContentField = value;
                this.RaisePropertyChanged("msgContent");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string msgId {
            get {
                return this.msgIdField;
            }
            set {
                this.msgIdField = value;
                this.RaisePropertyChanged("msgId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string msgStatus {
            get {
                return this.msgStatusField;
            }
            set {
                this.msgStatusField = value;
                this.RaisePropertyChanged("msgStatus");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getMessagesListByGuid", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getMessagesListByGuidRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CORR_GUID;
        
        public getMessagesListByGuidRequest() {
        }
        
        public getMessagesListByGuidRequest(string FIRM_ID, string USER_ID, string CORR_GUID) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.CORR_GUID = CORR_GUID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getMessagesListByGuidResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getMessagesListByGuidResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public getMessagesListByGuidResponse() {
        }
        
        public getMessagesListByGuidResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNotReadMessagesList", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getNotReadMessagesListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        public getNotReadMessagesListRequest() {
        }
        
        public getNotReadMessagesListRequest(string FIRM_ID, string USER_ID) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getNotReadMessagesListResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class getNotReadMessagesListResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public getNotReadMessagesListResponse() {
        }
        
        public getNotReadMessagesListResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="downloadmessagebyindex", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class downloadmessagebyindexRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string FIRM_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USER_ID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string INDEX;
        
        public downloadmessagebyindexRequest() {
        }
        
        public downloadmessagebyindexRequest(string FIRM_ID, string USER_ID, string INDEX) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.INDEX = INDEX;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="downloadmessagebyindexResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class downloadmessagebyindexResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public downloadmessagebyindexResponse() {
        }
        
        public downloadmessagebyindexResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitdeclaration", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class submitdeclarationRequest {
        
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
        
        public submitdeclarationRequest() {
        }
        
        public submitdeclarationRequest(string FIRM_ID, string USER_ID, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="submitdeclarationResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class submitdeclarationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public submitdeclarationResponse() {
        }
        
        public submitdeclarationResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebylrn", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebylrnRequest {
        
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
        
        public uploadmessagebylrnRequest() {
        }
        
        public uploadmessagebylrnRequest(string FIRM_ID, string USER_ID, string LRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.LRN = LRN;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebylrnResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebylrnResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public uploadmessagebylrnResponse() {
        }
        
        public uploadmessagebylrnResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebymrn", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebymrnRequest {
        
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
        
        public uploadmessagebymrnRequest() {
        }
        
        public uploadmessagebymrnRequest(string FIRM_ID, string USER_ID, string MRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            this.FIRM_ID = FIRM_ID;
            this.USER_ID = USER_ID;
            this.MRN = MRN;
            this.MSG_TYPE = MSG_TYPE;
            this.SIGN_FLAG = SIGN_FLAG;
            this.MSG_CONTENT = MSG_CONTENT;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="uploadmessagebymrnResponse", WrapperNamespace="http://ws/", IsWrapped=true)]
    public partial class uploadmessagebymrnResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://ws/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BYT.UI.NctsHizmeti.result @return;
        
        public uploadmessagebymrnResponse() {
        }
        
        public uploadmessagebymrnResponse(BYT.UI.NctsHizmeti.result @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WS2ServiceChannel : BYT.UI.NctsHizmeti.WS2Service, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WS2ServiceClient : System.ServiceModel.ClientBase<BYT.UI.NctsHizmeti.WS2Service>, BYT.UI.NctsHizmeti.WS2Service {
        
        public WS2ServiceClient() {
        }
        
        public WS2ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WS2ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WS2ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WS2ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.getMessagesListByGuidResponse BYT.UI.NctsHizmeti.WS2Service.getMessagesListByGuid(BYT.UI.NctsHizmeti.getMessagesListByGuidRequest request) {
            return base.Channel.getMessagesListByGuid(request);
        }
        
        public BYT.UI.NctsHizmeti.result getMessagesListByGuid(string FIRM_ID, string USER_ID, string CORR_GUID) {
            BYT.UI.NctsHizmeti.getMessagesListByGuidRequest inValue = new BYT.UI.NctsHizmeti.getMessagesListByGuidRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.CORR_GUID = CORR_GUID;
            BYT.UI.NctsHizmeti.getMessagesListByGuidResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).getMessagesListByGuid(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getMessagesListByGuidResponse> BYT.UI.NctsHizmeti.WS2Service.getMessagesListByGuidAsync(BYT.UI.NctsHizmeti.getMessagesListByGuidRequest request) {
            return base.Channel.getMessagesListByGuidAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getMessagesListByGuidResponse> getMessagesListByGuidAsync(string FIRM_ID, string USER_ID, string CORR_GUID) {
            BYT.UI.NctsHizmeti.getMessagesListByGuidRequest inValue = new BYT.UI.NctsHizmeti.getMessagesListByGuidRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.CORR_GUID = CORR_GUID;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).getMessagesListByGuidAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.getNotReadMessagesListResponse BYT.UI.NctsHizmeti.WS2Service.getNotReadMessagesList(BYT.UI.NctsHizmeti.getNotReadMessagesListRequest request) {
            return base.Channel.getNotReadMessagesList(request);
        }
        
        public BYT.UI.NctsHizmeti.result getNotReadMessagesList(string FIRM_ID, string USER_ID) {
            BYT.UI.NctsHizmeti.getNotReadMessagesListRequest inValue = new BYT.UI.NctsHizmeti.getNotReadMessagesListRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            BYT.UI.NctsHizmeti.getNotReadMessagesListResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).getNotReadMessagesList(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getNotReadMessagesListResponse> BYT.UI.NctsHizmeti.WS2Service.getNotReadMessagesListAsync(BYT.UI.NctsHizmeti.getNotReadMessagesListRequest request) {
            return base.Channel.getNotReadMessagesListAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.getNotReadMessagesListResponse> getNotReadMessagesListAsync(string FIRM_ID, string USER_ID) {
            BYT.UI.NctsHizmeti.getNotReadMessagesListRequest inValue = new BYT.UI.NctsHizmeti.getNotReadMessagesListRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).getNotReadMessagesListAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.downloadmessagebyindexResponse BYT.UI.NctsHizmeti.WS2Service.downloadmessagebyindex(BYT.UI.NctsHizmeti.downloadmessagebyindexRequest request) {
            return base.Channel.downloadmessagebyindex(request);
        }
        
        public BYT.UI.NctsHizmeti.result downloadmessagebyindex(string FIRM_ID, string USER_ID, string INDEX) {
            BYT.UI.NctsHizmeti.downloadmessagebyindexRequest inValue = new BYT.UI.NctsHizmeti.downloadmessagebyindexRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.INDEX = INDEX;
            BYT.UI.NctsHizmeti.downloadmessagebyindexResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).downloadmessagebyindex(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.downloadmessagebyindexResponse> BYT.UI.NctsHizmeti.WS2Service.downloadmessagebyindexAsync(BYT.UI.NctsHizmeti.downloadmessagebyindexRequest request) {
            return base.Channel.downloadmessagebyindexAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.downloadmessagebyindexResponse> downloadmessagebyindexAsync(string FIRM_ID, string USER_ID, string INDEX) {
            BYT.UI.NctsHizmeti.downloadmessagebyindexRequest inValue = new BYT.UI.NctsHizmeti.downloadmessagebyindexRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.INDEX = INDEX;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).downloadmessagebyindexAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.submitdeclarationResponse BYT.UI.NctsHizmeti.WS2Service.submitdeclaration(BYT.UI.NctsHizmeti.submitdeclarationRequest request) {
            return base.Channel.submitdeclaration(request);
        }
        
        public BYT.UI.NctsHizmeti.result submitdeclaration(string FIRM_ID, string USER_ID, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.submitdeclarationRequest inValue = new BYT.UI.NctsHizmeti.submitdeclarationRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            BYT.UI.NctsHizmeti.submitdeclarationResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).submitdeclaration(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.submitdeclarationResponse> BYT.UI.NctsHizmeti.WS2Service.submitdeclarationAsync(BYT.UI.NctsHizmeti.submitdeclarationRequest request) {
            return base.Channel.submitdeclarationAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.submitdeclarationResponse> submitdeclarationAsync(string FIRM_ID, string USER_ID, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.submitdeclarationRequest inValue = new BYT.UI.NctsHizmeti.submitdeclarationRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).submitdeclarationAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.uploadmessagebylrnResponse BYT.UI.NctsHizmeti.WS2Service.uploadmessagebylrn(BYT.UI.NctsHizmeti.uploadmessagebylrnRequest request) {
            return base.Channel.uploadmessagebylrn(request);
        }
        
        public BYT.UI.NctsHizmeti.result uploadmessagebylrn(string FIRM_ID, string USER_ID, string LRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.uploadmessagebylrnRequest inValue = new BYT.UI.NctsHizmeti.uploadmessagebylrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.LRN = LRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            BYT.UI.NctsHizmeti.uploadmessagebylrnResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).uploadmessagebylrn(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebylrnResponse> BYT.UI.NctsHizmeti.WS2Service.uploadmessagebylrnAsync(BYT.UI.NctsHizmeti.uploadmessagebylrnRequest request) {
            return base.Channel.uploadmessagebylrnAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebylrnResponse> uploadmessagebylrnAsync(string FIRM_ID, string USER_ID, string LRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.uploadmessagebylrnRequest inValue = new BYT.UI.NctsHizmeti.uploadmessagebylrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.LRN = LRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).uploadmessagebylrnAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        BYT.UI.NctsHizmeti.uploadmessagebymrnResponse BYT.UI.NctsHizmeti.WS2Service.uploadmessagebymrn(BYT.UI.NctsHizmeti.uploadmessagebymrnRequest request) {
            return base.Channel.uploadmessagebymrn(request);
        }
        
        public BYT.UI.NctsHizmeti.result uploadmessagebymrn(string FIRM_ID, string USER_ID, string MRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.uploadmessagebymrnRequest inValue = new BYT.UI.NctsHizmeti.uploadmessagebymrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MRN = MRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            BYT.UI.NctsHizmeti.uploadmessagebymrnResponse retVal = ((BYT.UI.NctsHizmeti.WS2Service)(this)).uploadmessagebymrn(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebymrnResponse> BYT.UI.NctsHizmeti.WS2Service.uploadmessagebymrnAsync(BYT.UI.NctsHizmeti.uploadmessagebymrnRequest request) {
            return base.Channel.uploadmessagebymrnAsync(request);
        }
        
        public System.Threading.Tasks.Task<BYT.UI.NctsHizmeti.uploadmessagebymrnResponse> uploadmessagebymrnAsync(string FIRM_ID, string USER_ID, string MRN, string MSG_TYPE, string SIGN_FLAG, string MSG_CONTENT) {
            BYT.UI.NctsHizmeti.uploadmessagebymrnRequest inValue = new BYT.UI.NctsHizmeti.uploadmessagebymrnRequest();
            inValue.FIRM_ID = FIRM_ID;
            inValue.USER_ID = USER_ID;
            inValue.MRN = MRN;
            inValue.MSG_TYPE = MSG_TYPE;
            inValue.SIGN_FLAG = SIGN_FLAG;
            inValue.MSG_CONTENT = MSG_CONTENT;
            return ((BYT.UI.NctsHizmeti.WS2Service)(this)).uploadmessagebymrnAsync(inValue);
        }
    }
}
