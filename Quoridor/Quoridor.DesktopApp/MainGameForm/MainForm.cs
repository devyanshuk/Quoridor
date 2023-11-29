using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.DesktopApp.Configuration;
using Quoridor.DesktopApp.StartupInfrastructure;

namespace Quoridor.DesktopApp
{
    public partial class MainForm : Form
    {
        private readonly IWindsorContainer _container;
        private readonly ILocalSettings _localSettings;
        private readonly IConfigProvider _configProvider;

        public MainForm(IWindsorContainer container)
        {
            _container = container;
            _localSettings = _container.Resolve<ILocalSettings>();
            _configProvider = _container.Resolve<IConfigProvider>();

            InitializeComponent();
        }
    }
}
