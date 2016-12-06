using Autofac;
using Dms.Service.Implements;
using Dms.Service.Interfaces;

namespace Dms.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LotteryService>().As<ILotteryService>().InstancePerRequest();
            base.Load(builder);
        }
    }
}
