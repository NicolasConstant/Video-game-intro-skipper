using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Prism.Unity;
using VGIS.Domain.Settings;

namespace VGIS.GUI
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            // Register global settings
            var globalSettings = new GlobalSettings()
            {
                GamesSettingsFolder = $@"{Directory.GetCurrentDirectory()}\GameSettings\",
                DefaultInstallFolderConfigFile = $@"{Directory.GetCurrentDirectory()}\DefaultInstallFolders.json",
                CustomInstallFolderConfigFile = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\VGIS\CustomInstallFolders.json"
            };
            Container.RegisterInstance(globalSettings, new ContainerControlledLifetimeManager());

            // Register repositories
            Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Name.EndsWith("Repository")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);

            // Register services
            Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Name.EndsWith("Service")),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.ContainerControlled);

            // Register all other types
            Container.RegisterTypes(
                AllClasses.FromLoadedAssemblies(),
                WithMappings.FromMatchingInterface,
                WithName.Default);

            base.ConfigureContainer();
        }
    }
}
