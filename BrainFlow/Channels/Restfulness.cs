using System;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    namespace Restfulness
    {
        public class PluginHandler : Interface.LucidPluginBase
        {
            public override string Name { get { return "Restfulness"; } }

            public override bool Initialize()
            {
                try
                {
                    return Device.Initialize();
                }
                catch (Exception ex)
                {
                    throw (new Exception("The '" + Name + "' plugin failed to initialize: " + ex.Message));
                }
            }

            public override double Value
            {
                get
                {
                    return Device.GetMetric(brainflow.BrainFlowMetrics.RESTFULNESS);
                }
            }

            public override void Dispose()
            {
                Device.Dispose();
            }
        }
    }
}
