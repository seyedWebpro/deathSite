﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceReference1
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientPaymentRequestDataBase", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ServiceReference1.ClientSaleRequestData))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ServiceReference1.ClientSaleDiscountRequestData))]
    public partial class ClientPaymentRequestDataBase : object
    {
        
        private string LoginAccountField;
        
        private long AmountField;
        
        private long OrderIdField;
        
        private string CallBackUrlField;
        
        private string AdditionalDataField;
        
        private string OriginatorField;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string LoginAccount
        {
            get
            {
                return this.LoginAccountField;
            }
            set
            {
                this.LoginAccountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public long Amount
        {
            get
            {
                return this.AmountField;
            }
            set
            {
                this.AmountField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public long OrderId
        {
            get
            {
                return this.OrderIdField;
            }
            set
            {
                this.OrderIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string CallBackUrl
        {
            get
            {
                return this.CallBackUrlField;
            }
            set
            {
                this.CallBackUrlField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string AdditionalData
        {
            get
            {
                return this.AdditionalDataField;
            }
            set
            {
                this.AdditionalDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string Originator
        {
            get
            {
                return this.OriginatorField;
            }
            set
            {
                this.OriginatorField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientSaleRequestData", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ServiceReference1.ClientSaleDiscountRequestData))]
    public partial class ClientSaleRequestData : ServiceReference1.ClientPaymentRequestDataBase
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientSaleDiscountRequestData", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientSaleDiscountRequestData : ServiceReference1.ClientSaleRequestData
    {
        
        private ServiceReference1.Product[] DiscountProductField;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public ServiceReference1.Product[] DiscountProduct
        {
            get
            {
                return this.DiscountProductField;
            }
            set
            {
                this.DiscountProductField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Product", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class Product : object
    {
        
        private int PGroupIdField;
        
        private long AmountField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int PGroupId
        {
            get
            {
                return this.PGroupIdField;
            }
            set
            {
                this.PGroupIdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=1)]
        public long Amount
        {
            get
            {
                return this.AmountField;
            }
            set
            {
                this.AmountField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientPaymentResponseDataBase", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(ServiceReference1.ClientSaleResponseData))]
    public partial class ClientPaymentResponseDataBase : object
    {
        
        private long TokenField;
        
        private string MessageField;
        
        private short StatusField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public long Token
        {
            get
            {
                return this.TokenField;
            }
            set
            {
                this.TokenField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                this.MessageField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true, Order=2)]
        public short Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                this.StatusField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClientSaleResponseData", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class ClientSaleResponseData : ServiceReference1.ClientPaymentResponseDataBase
    {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", ConfigurationName="ServiceReference1.SaleServiceSoap")]
    public interface SaleServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentRequest", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentRequestResponse> SalePaymentRequestAsync(ServiceReference1.SalePaymentRequestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentWithId", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithIdResponse> SalePaymentWithIdAsync(ServiceReference1.SalePaymentWithIdRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/UDSalePaymentRequest", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.UDSalePaymentRequestResponse> UDSalePaymentRequestAsync(ServiceReference1.UDSalePaymentRequestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService/SalePaymentWithDiscount", ReplyAction="*")]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithDiscountResponse> SalePaymentWithDiscountAsync(ServiceReference1.SalePaymentWithDiscountRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentRequestRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentRequest", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentRequestRequestBody Body;
        
        public SalePaymentRequestRequest()
        {
        }
        
        public SalePaymentRequestRequest(ServiceReference1.SalePaymentRequestRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentRequestRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleRequestData requestData;
        
        public SalePaymentRequestRequestBody()
        {
        }
        
        public SalePaymentRequestRequestBody(ServiceReference1.ClientSaleRequestData requestData)
        {
            this.requestData = requestData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentRequestResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentRequestResponse", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentRequestResponseBody Body;
        
        public SalePaymentRequestResponse()
        {
        }
        
        public SalePaymentRequestResponse(ServiceReference1.SalePaymentRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentRequestResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleResponseData SalePaymentRequestResult;
        
        public SalePaymentRequestResponseBody()
        {
        }
        
        public SalePaymentRequestResponseBody(ServiceReference1.ClientSaleResponseData SalePaymentRequestResult)
        {
            this.SalePaymentRequestResult = SalePaymentRequestResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentWithIdRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentWithId", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentWithIdRequestBody Body;
        
        public SalePaymentWithIdRequest()
        {
        }
        
        public SalePaymentWithIdRequest(ServiceReference1.SalePaymentWithIdRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentWithIdRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleRequestData requestData;
        
        public SalePaymentWithIdRequestBody()
        {
        }
        
        public SalePaymentWithIdRequestBody(ServiceReference1.ClientSaleRequestData requestData)
        {
            this.requestData = requestData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentWithIdResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentWithIdResponse", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentWithIdResponseBody Body;
        
        public SalePaymentWithIdResponse()
        {
        }
        
        public SalePaymentWithIdResponse(ServiceReference1.SalePaymentWithIdResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentWithIdResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleResponseData SalePaymentWithIdResult;
        
        public SalePaymentWithIdResponseBody()
        {
        }
        
        public SalePaymentWithIdResponseBody(ServiceReference1.ClientSaleResponseData SalePaymentWithIdResult)
        {
            this.SalePaymentWithIdResult = SalePaymentWithIdResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UDSalePaymentRequestRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UDSalePaymentRequest", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.UDSalePaymentRequestRequestBody Body;
        
        public UDSalePaymentRequestRequest()
        {
        }
        
        public UDSalePaymentRequestRequest(ServiceReference1.UDSalePaymentRequestRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class UDSalePaymentRequestRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleRequestData requestData;
        
        public UDSalePaymentRequestRequestBody()
        {
        }
        
        public UDSalePaymentRequestRequestBody(ServiceReference1.ClientSaleRequestData requestData)
        {
            this.requestData = requestData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UDSalePaymentRequestResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="UDSalePaymentRequestResponse", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.UDSalePaymentRequestResponseBody Body;
        
        public UDSalePaymentRequestResponse()
        {
        }
        
        public UDSalePaymentRequestResponse(ServiceReference1.UDSalePaymentRequestResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class UDSalePaymentRequestResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientPaymentResponseDataBase UDSalePaymentRequestResult;
        
        public UDSalePaymentRequestResponseBody()
        {
        }
        
        public UDSalePaymentRequestResponseBody(ServiceReference1.ClientPaymentResponseDataBase UDSalePaymentRequestResult)
        {
            this.UDSalePaymentRequestResult = UDSalePaymentRequestResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentWithDiscountRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentWithDiscount", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentWithDiscountRequestBody Body;
        
        public SalePaymentWithDiscountRequest()
        {
        }
        
        public SalePaymentWithDiscountRequest(ServiceReference1.SalePaymentWithDiscountRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentWithDiscountRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleDiscountRequestData requestData;
        
        public SalePaymentWithDiscountRequestBody()
        {
        }
        
        public SalePaymentWithDiscountRequestBody(ServiceReference1.ClientSaleDiscountRequestData requestData)
        {
            this.requestData = requestData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SalePaymentWithDiscountResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SalePaymentWithDiscountResponse", Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService", Order=0)]
        public ServiceReference1.SalePaymentWithDiscountResponseBody Body;
        
        public SalePaymentWithDiscountResponse()
        {
        }
        
        public SalePaymentWithDiscountResponse(ServiceReference1.SalePaymentWithDiscountResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://pec.Shaparak.ir/NewIPGServices/Sale/SaleService")]
    public partial class SalePaymentWithDiscountResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public ServiceReference1.ClientSaleResponseData SalePaymentWithDiscountResult;
        
        public SalePaymentWithDiscountResponseBody()
        {
        }
        
        public SalePaymentWithDiscountResponseBody(ServiceReference1.ClientSaleResponseData SalePaymentWithDiscountResult)
        {
            this.SalePaymentWithDiscountResult = SalePaymentWithDiscountResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface SaleServiceSoapChannel : ServiceReference1.SaleServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class SaleServiceSoapClient : System.ServiceModel.ClientBase<ServiceReference1.SaleServiceSoap>, ServiceReference1.SaleServiceSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public SaleServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(SaleServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), SaleServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SaleServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(SaleServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SaleServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(SaleServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public SaleServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentRequestResponse> ServiceReference1.SaleServiceSoap.SalePaymentRequestAsync(ServiceReference1.SalePaymentRequestRequest request)
        {
            return base.Channel.SalePaymentRequestAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SalePaymentRequestResponse> SalePaymentRequestAsync(ServiceReference1.ClientSaleRequestData requestData)
        {
            ServiceReference1.SalePaymentRequestRequest inValue = new ServiceReference1.SalePaymentRequestRequest();
            inValue.Body = new ServiceReference1.SalePaymentRequestRequestBody();
            inValue.Body.requestData = requestData;
            return ((ServiceReference1.SaleServiceSoap)(this)).SalePaymentRequestAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithIdResponse> ServiceReference1.SaleServiceSoap.SalePaymentWithIdAsync(ServiceReference1.SalePaymentWithIdRequest request)
        {
            return base.Channel.SalePaymentWithIdAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithIdResponse> SalePaymentWithIdAsync(ServiceReference1.ClientSaleRequestData requestData)
        {
            ServiceReference1.SalePaymentWithIdRequest inValue = new ServiceReference1.SalePaymentWithIdRequest();
            inValue.Body = new ServiceReference1.SalePaymentWithIdRequestBody();
            inValue.Body.requestData = requestData;
            return ((ServiceReference1.SaleServiceSoap)(this)).SalePaymentWithIdAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.UDSalePaymentRequestResponse> ServiceReference1.SaleServiceSoap.UDSalePaymentRequestAsync(ServiceReference1.UDSalePaymentRequestRequest request)
        {
            return base.Channel.UDSalePaymentRequestAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.UDSalePaymentRequestResponse> UDSalePaymentRequestAsync(ServiceReference1.ClientSaleRequestData requestData)
        {
            ServiceReference1.UDSalePaymentRequestRequest inValue = new ServiceReference1.UDSalePaymentRequestRequest();
            inValue.Body = new ServiceReference1.UDSalePaymentRequestRequestBody();
            inValue.Body.requestData = requestData;
            return ((ServiceReference1.SaleServiceSoap)(this)).UDSalePaymentRequestAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithDiscountResponse> ServiceReference1.SaleServiceSoap.SalePaymentWithDiscountAsync(ServiceReference1.SalePaymentWithDiscountRequest request)
        {
            return base.Channel.SalePaymentWithDiscountAsync(request);
        }
        
        public System.Threading.Tasks.Task<ServiceReference1.SalePaymentWithDiscountResponse> SalePaymentWithDiscountAsync(ServiceReference1.ClientSaleDiscountRequestData requestData)
        {
            ServiceReference1.SalePaymentWithDiscountRequest inValue = new ServiceReference1.SalePaymentWithDiscountRequest();
            inValue.Body = new ServiceReference1.SalePaymentWithDiscountRequestBody();
            inValue.Body.requestData = requestData;
            return ((ServiceReference1.SaleServiceSoap)(this)).SalePaymentWithDiscountAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SaleServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.SaleServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.SaleServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("https://pec.shaparak.ir/NewIPGServices/Sale/SaleService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.SaleServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("https://pec.shaparak.ir/NewIPGServices/Sale/SaleService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            SaleServiceSoap,
            
            SaleServiceSoap12,
        }
    }
}
