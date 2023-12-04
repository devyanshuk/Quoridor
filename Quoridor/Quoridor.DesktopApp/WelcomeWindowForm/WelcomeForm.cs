using System;
using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.DesktopApp.Configuration;
using Quoridor.DesktopApp.StrategySetupWindowForm;

namespace Quoridor.DesktopApp.WelcomeWindowForm
{
    public partial class WelcomeForm : Form
    {
        private readonly ColorSettings _colorSettings;
        private readonly FormSettings _formSettings;
        private readonly GameSettings _gameSettings;

        private StrategySetupForm _strategySetupForm;
        private MainForm _mainForm;

        private bool _btnOneClickedLast;

        public WelcomeForm(IWindsorContainer container)
        {
            var configProvider = container.Resolve<IConfigProvider>();

            _colorSettings = configProvider.AppSettings.ColorSettings;
            _formSettings = configProvider.AppSettings.FormSettings;
            _gameSettings = configProvider.AppSettings.GameSettings;

            _strategySetupForm = new StrategySetupForm(container);
            _mainForm = new MainForm(container);

            _mainForm.OnMainMenuPressed += OnMainMenuRequested;
            _strategySetupForm.OnOkButtonPress += OnSuccessfulStrategySelection;
            InitializeComponent();
            BackColor = _colorSettings.BackgroundColor;
            ClientSize = new System.Drawing.Size(_formSettings.ScreenWidth, _formSettings.ScreenHeight);

            // setup buttons tooltip text
            toolTipButtonText.SetToolTip(btnPlayerOne, "Select Strategy One...");
            toolTipButtonText.SetToolTip(btnPlayerTwo, "Select Strategy Two...");

            Initialize();
        }

        public void Initialize()
        {

            btnPlayerOne.Text = $"{_gameSettings.SelectedStrategies[0]}\n{_gameSettings.SelectedStrategies[0].GetParams()}";
            btnPlayerTwo.Text = $"{_gameSettings.SelectedStrategies[1]}\n{_gameSettings.SelectedStrategies[1].GetParams()}";

            ToggleNumPlayerButtons(true);
            ToggleStrategySelectionButtons(false);
        }

        private void ShowGameBoard()
        {
            ToggleNumPlayerButtons(false);
            ToggleStrategySelectionButtons(false);

            _mainForm.Location = this.Location;
            _mainForm.Initialize();
            _mainForm.Show();

            this.Hide();
        }

        private void OnMainMenuRequested(object sender, EventArgs e)
        {
            this.Location = _mainForm.Location;
            _mainForm.Hide();
            Initialize();
            this.Show();
        }



        private void ToggleNumPlayerButtons(bool switchVal)
        {
            btnTwoPlayers.Visible = switchVal;
            btnThreePlayers.Visible = switchVal;
            btnFourPlayers.Visible = switchVal;
        }

        private void ToggleStrategySelectionButtons(bool switchVal)
        {
            btnPlayerOne.Visible = switchVal;
            btnPlayerTwo.Visible = switchVal;
            lbVS.Visible = switchVal;
            btnPlay.Visible = switchVal;
        }

        private void ShowStrategySelectionWindow()
        {
            ToggleNumPlayerButtons(false);
            ToggleStrategySelectionButtons(true);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            ShowGameBoard();
        }

        private void OnSuccessfulStrategySelection(object sender, Strategy strategy)
        {
            this.Location = _strategySetupForm.Location;

            this.Show();

            _strategySetupForm.Hide();

            // Update strategy info
            UpdateStrategyButtonController(strategy);
        }

        private void UpdateStrategyButtonController(Strategy strategy)
        {
            var button = _btnOneClickedLast ? btnPlayerOne : btnPlayerTwo;
            var index = _btnOneClickedLast ? 0 : 1;

            button.Text = $"{strategy}\n{strategy.GetParams()}";

            _gameSettings.SelectedStrategies[index] = strategy;
        }

        private void btnPlayerOne_Click(object sender, EventArgs e)
        {
            InitConfigureStrategyForm(_gameSettings.SelectedStrategies[0]);
            _btnOneClickedLast = true;
        }


        private void btnPlayerTwo_Click(object sender, EventArgs e)
        {
            InitConfigureStrategyForm(_gameSettings.SelectedStrategies[1]);
            _btnOneClickedLast = false;
        }

        private void InitConfigureStrategyForm(Strategy selectedStrategy)
        {
            _strategySetupForm.Location = this.Location;
            _strategySetupForm.Initialize(selectedStrategy);
            _strategySetupForm.Show();

            this.Hide();
        }

        private void btnThreePlayers_Click(object sender, EventArgs e)
        {
            _gameSettings.Players = 3;
            ShowGameBoard();
        }

        private void btnTwoPlayers_Click(object sender, EventArgs e)
        {
            _gameSettings.Players = 2;
            ShowStrategySelectionWindow();
        }

        private void btnFourPlayers_Click(object sender, EventArgs e)
        {
            _gameSettings.Players = 4;
            ShowGameBoard();
        }
    }
}
