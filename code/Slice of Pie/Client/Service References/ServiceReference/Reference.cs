﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceUser", Namespace="http://schemas.datacontract.org/2004/07/WcfService")]
    [System.SerializableAttribute()]
    public partial class ServiceUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string emailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string passwordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int rootFolderIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string email {
            get {
                return this.emailField;
            }
            set {
                if ((object.ReferenceEquals(this.emailField, value) != true)) {
                    this.emailField = value;
                    this.RaisePropertyChanged("email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string password {
            get {
                return this.passwordField;
            }
            set {
                if ((object.ReferenceEquals(this.passwordField, value) != true)) {
                    this.passwordField = value;
                    this.RaisePropertyChanged("password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int rootFolderId {
            get {
                return this.rootFolderIdField;
            }
            set {
                if ((this.rootFolderIdField.Equals(value) != true)) {
                    this.rootFolderIdField = value;
                    this.RaisePropertyChanged("rootFolderId");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceFolder", Namespace="http://schemas.datacontract.org/2004/07/WcfService")]
    [System.SerializableAttribute()]
    public partial class ServiceFolder : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> parentFolderIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                if ((object.ReferenceEquals(this.nameField, value) != true)) {
                    this.nameField = value;
                    this.RaisePropertyChanged("name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> parentFolderId {
            get {
                return this.parentFolderIdField;
            }
            set {
                if ((this.parentFolderIdField.Equals(value) != true)) {
                    this.parentFolderIdField = value;
                    this.RaisePropertyChanged("parentFolderId");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceDocument", Namespace="http://schemas.datacontract.org/2004/07/WcfService")]
    [System.SerializableAttribute()]
    public partial class ServiceDocument : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime creationTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int creatorIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string nameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string pathField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime creationTime {
            get {
                return this.creationTimeField;
            }
            set {
                if ((this.creationTimeField.Equals(value) != true)) {
                    this.creationTimeField = value;
                    this.RaisePropertyChanged("creationTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int creatorId {
            get {
                return this.creatorIdField;
            }
            set {
                if ((this.creatorIdField.Equals(value) != true)) {
                    this.creatorIdField = value;
                    this.RaisePropertyChanged("creatorId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                if ((object.ReferenceEquals(this.nameField, value) != true)) {
                    this.nameField = value;
                    this.RaisePropertyChanged("name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string path {
            get {
                return this.pathField;
            }
            set {
                if ((object.ReferenceEquals(this.pathField, value) != true)) {
                    this.pathField = value;
                    this.RaisePropertyChanged("path");
                }
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ServiceUserdocument", Namespace="http://schemas.datacontract.org/2004/07/WcfService")]
    [System.SerializableAttribute()]
    public partial class ServiceUserdocument : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int documentIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int folderIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int userIdField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int documentId {
            get {
                return this.documentIdField;
            }
            set {
                if ((this.documentIdField.Equals(value) != true)) {
                    this.documentIdField = value;
                    this.RaisePropertyChanged("documentId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int folderId {
            get {
                return this.folderIdField;
            }
            set {
                if ((this.folderIdField.Equals(value) != true)) {
                    this.folderIdField = value;
                    this.RaisePropertyChanged("folderId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int userId {
            get {
                return this.userIdField;
            }
            set {
                if ((this.userIdField.Equals(value) != true)) {
                    this.userIdField = value;
                    this.RaisePropertyChanged("userId");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddUser", ReplyAction="http://tempuri.org/IService1/AddUserResponse")]
        int AddUser(string email, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddFolder", ReplyAction="http://tempuri.org/IService1/AddFolderResponse")]
        int AddFolder(string name, int parentFolderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddDocumentWithUserDocument", ReplyAction="http://tempuri.org/IService1/AddDocumentWithUserDocumentResponse")]
        int AddDocumentWithUserDocument(string name, int userId, string filepath, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddDocumentRevision", ReplyAction="http://tempuri.org/IService1/AddDocumentRevisionResponse")]
        void AddDocumentRevision(int editorId, int documentId, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SaveMergedDocument", ReplyAction="http://tempuri.org/IService1/SaveMergedDocumentResponse")]
        void SaveMergedDocument(int editorId, int documentId, string content);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetUserByEmailAndPass", ReplyAction="http://tempuri.org/IService1/GetUserByEmailAndPassResponse")]
        int GetUserByEmailAndPass(string email, string pass);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetUserById", ReplyAction="http://tempuri.org/IService1/GetUserByIdResponse")]
        Client.ServiceReference.ServiceUser GetUserById(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetUserByEmail", ReplyAction="http://tempuri.org/IService1/GetUserByEmailResponse")]
        Client.ServiceReference.ServiceUser GetUserByEmail(string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetFolder", ReplyAction="http://tempuri.org/IService1/GetFolderResponse")]
        Client.ServiceReference.ServiceFolder GetFolder(int folderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetRootFolderId", ReplyAction="http://tempuri.org/IService1/GetRootFolderIdResponse")]
        int GetRootFolderId(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDocumentById", ReplyAction="http://tempuri.org/IService1/GetDocumentByIdResponse")]
        Client.ServiceReference.ServiceDocument GetDocumentById(int documentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DeleteFolder", ReplyAction="http://tempuri.org/IService1/DeleteFolderResponse")]
        void DeleteFolder(int folderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DeleteDocumentReference", ReplyAction="http://tempuri.org/IService1/DeleteDocumentReferenceResponse")]
        void DeleteDocumentReference(int userId, int documentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DeleteDocument", ReplyAction="http://tempuri.org/IService1/DeleteDocumentResponse")]
        void DeleteDocument(int documentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetAllUserDocumentsByUserId", ReplyAction="http://tempuri.org/IService1/GetAllUserDocumentsByUserIdResponse")]
        Client.ServiceReference.ServiceUserdocument[] GetAllUserDocumentsByUserId(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDocumentContent", ReplyAction="http://tempuri.org/IService1/GetDocumentContentResponse")]
        string GetDocumentContent(string directoryPath, string filename);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetLatestDocumentContent", ReplyAction="http://tempuri.org/IService1/GetLatestDocumentContentResponse")]
        string GetLatestDocumentContent(int documentId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/SyncDocument", ReplyAction="http://tempuri.org/IService1/SyncDocumentResponse")]
        string[][] SyncDocument(int editorId, int documentId, string filepath, string content, string title, string[] original);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetDocumentId", ReplyAction="http://tempuri.org/IService1/GetDocumentIdResponse")]
        int GetDocumentId(int userId, string title);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddUserDocument", ReplyAction="http://tempuri.org/IService1/AddUserDocumentResponse")]
        void AddUserDocument(int userId, int documentId, string filepath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/FolderExists", ReplyAction="http://tempuri.org/IService1/FolderExistsResponse")]
        int FolderExists(int parentFolderId, string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddUserDocumentInRoot", ReplyAction="http://tempuri.org/IService1/AddUserDocumentInRootResponse")]
        void AddUserDocumentInRoot(int userId, int documentId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : Client.ServiceReference.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<Client.ServiceReference.IService1>, Client.ServiceReference.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int AddUser(string email, string password) {
            return base.Channel.AddUser(email, password);
        }
        
        public int AddFolder(string name, int parentFolderId) {
            return base.Channel.AddFolder(name, parentFolderId);
        }
        
        public int AddDocumentWithUserDocument(string name, int userId, string filepath, string content) {
            return base.Channel.AddDocumentWithUserDocument(name, userId, filepath, content);
        }
        
        public void AddDocumentRevision(int editorId, int documentId, string content) {
            base.Channel.AddDocumentRevision(editorId, documentId, content);
        }
        
        public void SaveMergedDocument(int editorId, int documentId, string content) {
            base.Channel.SaveMergedDocument(editorId, documentId, content);
        }
        
        public int GetUserByEmailAndPass(string email, string pass) {
            return base.Channel.GetUserByEmailAndPass(email, pass);
        }
        
        public Client.ServiceReference.ServiceUser GetUserById(int userId) {
            return base.Channel.GetUserById(userId);
        }
        
        public Client.ServiceReference.ServiceUser GetUserByEmail(string email) {
            return base.Channel.GetUserByEmail(email);
        }
        
        public Client.ServiceReference.ServiceFolder GetFolder(int folderId) {
            return base.Channel.GetFolder(folderId);
        }
        
        public int GetRootFolderId(int userId) {
            return base.Channel.GetRootFolderId(userId);
        }
        
        public Client.ServiceReference.ServiceDocument GetDocumentById(int documentId) {
            return base.Channel.GetDocumentById(documentId);
        }
        
        public void DeleteFolder(int folderId) {
            base.Channel.DeleteFolder(folderId);
        }
        
        public void DeleteDocumentReference(int userId, int documentId) {
            base.Channel.DeleteDocumentReference(userId, documentId);
        }
        
        public void DeleteDocument(int documentId) {
            base.Channel.DeleteDocument(documentId);
        }
        
        public Client.ServiceReference.ServiceUserdocument[] GetAllUserDocumentsByUserId(int userId) {
            return base.Channel.GetAllUserDocumentsByUserId(userId);
        }
        
        public string GetDocumentContent(string directoryPath, string filename) {
            return base.Channel.GetDocumentContent(directoryPath, filename);
        }
        
        public string GetLatestDocumentContent(int documentId) {
            return base.Channel.GetLatestDocumentContent(documentId);
        }
        
        public string[][] SyncDocument(int editorId, int documentId, string filepath, string content, string title, string[] original) {
            return base.Channel.SyncDocument(editorId, documentId, filepath, content, title, original);
        }
        
        public int GetDocumentId(int userId, string title) {
            return base.Channel.GetDocumentId(userId, title);
        }
        
        public void AddUserDocument(int userId, int documentId, string filepath) {
            base.Channel.AddUserDocument(userId, documentId, filepath);
        }
        
        public int FolderExists(int parentFolderId, string name) {
            return base.Channel.FolderExists(parentFolderId, name);
        }
        
        public void AddUserDocumentInRoot(int userId, int documentId) {
            base.Channel.AddUserDocumentInRoot(userId, documentId);
        }
    }
}
