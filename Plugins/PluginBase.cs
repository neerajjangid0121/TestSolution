namespace AeroSuiteARPAIIntegrationPlugins
{
    using CCLLC.CDS.Sdk;
    using CCLLC.Core;

    public abstract class PluginBase : InstrumentedCDSPlugin
    {
        private static IIocContainer _container;

        // override to provide a static container shared by all plugins in the assembly
        public override IIocContainer Container
        {
            get
            {
                if (_container is null)
                {
                    var lockObj = new object();
                    lock (lockObj)
                    {
                        if (_container is null)
                        {
                            _container = new IocContainer();
                        }
                    }
                }

                return _container;
            }
        }

        protected struct SecurityRoles
        {
            //public static readonly Guid AgreementApprover = new Guid("{5b2b380b-629e-ed11-aacf-001dd8098656}");
        }


        protected PluginBase(string unsecureConfig, string secureConfig)
            : base(unsecureConfig, secureConfig)
        {
            //InstrumentationVariableName = "jdx_TelemetryKey";
        }
    }
}
