using System.Xml.Serialization;
using System.Web.Services;
using System.ComponentModel;
using System.Web.Services.Protocols;
using System;
using System.Diagnostics;

namespace Rld.Acs.WpfApplication.Service.Security
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SecurityServiceSoap", Namespace="http://tempuri.org/")]
    public partial class SecurityService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback AuthenticateOperationCompleted;
        
        private System.Threading.SendOrPostCallback ChangePassowrdOperationCompleted;
        
        /// <remarks/>
        public SecurityService() {
            this.Url = "http://localhost:7362/SecurityService/SecurityService.asmx";
        }
        
        /// <remarks/>
        public event AuthenticateCompletedEventHandler AuthenticateCompleted;
        
        /// <remarks/>
        public event ChangePassowrdCompletedEventHandler ChangePassowrdCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Authenticate", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AuthenticateResult Authenticate(string username, string password) {
            object[] results = this.Invoke("Authenticate", new object[] {
                        username,
                        password});
            return ((AuthenticateResult)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginAuthenticate(string username, string password, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Authenticate", new object[] {
                        username,
                        password}, callback, asyncState);
        }
        
        /// <remarks/>
        public AuthenticateResult EndAuthenticate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((AuthenticateResult)(results[0]));
        }
        
        /// <remarks/>
        public void AuthenticateAsync(string username, string password) {
            this.AuthenticateAsync(username, password, null);
        }
        
        /// <remarks/>
        public void AuthenticateAsync(string username, string password, object userState) {
            if ((this.AuthenticateOperationCompleted == null)) {
                this.AuthenticateOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAuthenticateOperationCompleted);
            }
            this.InvokeAsync("Authenticate", new object[] {
                        username,
                        password}, this.AuthenticateOperationCompleted, userState);
        }
        
        private void OnAuthenticateOperationCompleted(object arg) {
            if ((this.AuthenticateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AuthenticateCompleted(this, new AuthenticateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ChangePassowrd", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public GenericResult ChangePassowrd(string username, string oldPassword, string newPassword) {
            object[] results = this.Invoke("ChangePassowrd", new object[] {
                        username,
                        oldPassword,
                        newPassword});
            return ((GenericResult)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginChangePassowrd(string username, string oldPassword, string newPassword, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ChangePassowrd", new object[] {
                        username,
                        oldPassword,
                        newPassword}, callback, asyncState);
        }
        
        /// <remarks/>
        public GenericResult EndChangePassowrd(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((GenericResult)(results[0]));
        }
        
        /// <remarks/>
        public void ChangePassowrdAsync(string username, string oldPassword, string newPassword) {
            this.ChangePassowrdAsync(username, oldPassword, newPassword, null);
        }
        
        /// <remarks/>
        public void ChangePassowrdAsync(string username, string oldPassword, string newPassword, object userState) {
            if ((this.ChangePassowrdOperationCompleted == null)) {
                this.ChangePassowrdOperationCompleted = new System.Threading.SendOrPostCallback(this.OnChangePassowrdOperationCompleted);
            }
            this.InvokeAsync("ChangePassowrd", new object[] {
                        username,
                        oldPassword,
                        newPassword}, this.ChangePassowrdOperationCompleted, userState);
        }
        
        private void OnChangePassowrdOperationCompleted(object arg) {
            if ((this.ChangePassowrdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ChangePassowrdCompleted(this, new ChangePassowrdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class AuthenticateResult : GenericResult {
        
        private SysOperator operatorInfoField;
        
        /// <remarks/>
        public SysOperator OperatorInfo {
            get {
                return this.operatorInfoField;
            }
            set {
                this.operatorInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class SysOperator {
        
        private int operatorIDField;
        
        private System.Nullable<int> userIDField;
        
        private string loginNameField;
        
        private string passwordField;
        
        private string saltField;
        
        private int languageIDField;
        
        private string photoField;
        
        private int createUserIDField;
        
        private System.DateTime createDateField;
        
        private GeneralStatus statusField;
        
        private System.Nullable<int> updateUserIDField;
        
        private System.Nullable<System.DateTime> updateDateField;
        
        /// <remarks/>
        public int OperatorID {
            get {
                return this.operatorIDField;
            }
            set {
                this.operatorIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> UserID {
            get {
                return this.userIDField;
            }
            set {
                this.userIDField = value;
            }
        }
        
        /// <remarks/>
        public string LoginName {
            get {
                return this.loginNameField;
            }
            set {
                this.loginNameField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string Salt {
            get {
                return this.saltField;
            }
            set {
                this.saltField = value;
            }
        }
        
        /// <remarks/>
        public int LanguageID {
            get {
                return this.languageIDField;
            }
            set {
                this.languageIDField = value;
            }
        }
        
        /// <remarks/>
        public string Photo {
            get {
                return this.photoField;
            }
            set {
                this.photoField = value;
            }
        }
        
        /// <remarks/>
        public int CreateUserID {
            get {
                return this.createUserIDField;
            }
            set {
                this.createUserIDField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime CreateDate {
            get {
                return this.createDateField;
            }
            set {
                this.createDateField = value;
            }
        }
        
        /// <remarks/>
        public GeneralStatus Status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> UpdateUserID {
            get {
                return this.updateUserIDField;
            }
            set {
                this.updateUserIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<System.DateTime> UpdateDate {
            get {
                return this.updateDateField;
            }
            set {
                this.updateDateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum GeneralStatus {
        
        /// <remarks/>
        Disabled,
        
        /// <remarks/>
        Enabled,
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AuthenticateResult))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class GenericResult {
        
        private ResultType resultTypeField;
        
        private string[] messagesField;
        
        /// <remarks/>
        public ResultType ResultType {
            get {
                return this.resultTypeField;
            }
            set {
                this.resultTypeField = value;
            }
        }
        
        /// <remarks/>
        public string[] Messages {
            get {
                return this.messagesField;
            }
            set {
                this.messagesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public enum ResultType {
        
        /// <remarks/>
        OK,
        
        /// <remarks/>
        UserNotFound,
        
        /// <remarks/>
        AuthenticationError,
        
        /// <remarks/>
        AuthorizationError,
        
        /// <remarks/>
        UnknownError,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void AuthenticateCompletedEventHandler(object sender, AuthenticateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AuthenticateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AuthenticateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AuthenticateResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AuthenticateResult)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void ChangePassowrdCompletedEventHandler(object sender, ChangePassowrdCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ChangePassowrdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ChangePassowrdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GenericResult Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GenericResult)(this.results[0]));
            }
        }
    }
}
