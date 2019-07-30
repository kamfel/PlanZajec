using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace PlanZajec.Core
{
    /// <summary>
    /// An IoC container for providing services
    /// </summary>
    public static class IoC
    {
        public static ICache<int, ScheduleItem> Cache => kernel.Get<ICache<int, ScheduleItem>>();

        public static IDataProvider DataProvider => kernel.Get<IDataProvider>();

        public static IEventPropagator<EventPropagatorArgs> EventPropagator => kernel.Get<IEventPropagator<EventPropagatorArgs>>();

        public static ApplicationViewModel ViewModelApplication => kernel.Get<ApplicationViewModel>();

        public static IKernel kernel { get; private set; } = new StandardKernel();

        public static void SetUp()
        {
            kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }
    }
}
