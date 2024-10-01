using System;

namespace lucidcode.LucidScribe.Plugin.BrainFlow
{
    namespace Mindfulness
    {
        public class PluginHandler : Interface.LucidPluginBase
        {
            public override string Name { get { return "Mindfulness"; } }

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
                    return Device.GetMetric(brainflow.BrainFlowMetrics.MINDFULNESS);
                }
            }

            public override void Dispose()
            {
                Device.Dispose();
            }
        }
    }
}
