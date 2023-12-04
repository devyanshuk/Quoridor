using System;
using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.DesktopApp.StartupInfrastructure;
using Quoridor.DesktopApp.Forms.WelcomeWindowForm;

namespace Quoridor.DesktopApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var container = new WindsorContainer().Install(new QuoridorDesktopAppDependencyInstaller());
            Application.Run(new WelcomeForm(container));
        }
    }
}