using PlanZajec.Core;
using PlanZajec.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PlanZajec
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Custom startup method
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Setup ioc container
            IoC.SetUp();
            IoC.kernel.Bind<ICache<int, ScheduleItem>>().ToConstant<ICache<int, ScheduleItem>>(new DiskCache<int, ScheduleItem>(CacheSettings.Default));
            IoC.kernel.Bind<IDataProvider>().ToConstant<IDataProvider>(new HtmlDataProvider());
            IoC.kernel.Bind<IEventPropagator<EventPropagatorArgs>>().ToConstant<IEventPropagator<EventPropagatorArgs>>(new EventPropagator());

            //Show main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
