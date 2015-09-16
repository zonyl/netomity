using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Web
{
    public class RestHost : NetomityObject
    {
        public const string BASE_URL = "http://localhost:8081/Netomity/API/v1";
        ServiceHost _host = null;

        public RestHost()
        {
            Open();
        }

        public void Open()
        {
            //http://msdn.microsoft.com/en-us/library/ms733768.aspx
            //netsh http add urlacl url=http://+:8081/Netomity/API/v1 user=DOMAIN\user
            //netsh http add urlacl url=http://+:8081/Netomity/API/v1 user=jason
            _host = new ServiceHost(typeof(RestAPI),
                new Uri[] { });

            WebHttpBinding binding = new WebHttpBinding(WebHttpSecurityMode.None);

            ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(RestAPI)),
                binding, new EndpointAddress(BASE_URL));
            WebHttpBehavior webBehavior = new WebHttpBehavior();
            endPoint.Behaviors.Add(webBehavior);
            _host.AddServiceEndpoint(endPoint);

            _host.Open();
            Log("Netomity RestAPI Service Started");
        }

        public void Close()
        {
            if (_host == null)
                _host.Close();

        }
    }
}
