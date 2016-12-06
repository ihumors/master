using Autofac;
using Dms.Repository.Implements;
using Dms.Repository.Interfaces;

namespace Dms.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LotteryRepository>().As<ILotteryRepository>().InstancePerRequest();
            builder.RegisterType<PagingRepository>().As<IPagingRepository>().InstancePerRequest();

            base.Load(builder);
        }
    }
}
