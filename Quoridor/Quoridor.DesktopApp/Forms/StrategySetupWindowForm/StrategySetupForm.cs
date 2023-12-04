using System;
using System.Data;
using System.Linq;
using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.DesktopApp.Configuration;

namespace Quoridor.DesktopApp.Forms.StrategySetupWindowForm
{
    public partial class StrategySetupForm : Form
    {
        private readonly ColorSettings _colorSettings;
        private readonly FormSettings _formSettings;
        private readonly GameSettings _gameSettings;

        public EventHandler<Strategy> OnOkButtonPress;
        public StrategySetupForm(IWindsorContainer container)
        {
            var configProvider = container.Resolve<IConfigProvider>();

            _colorSettings = configProvider.AppSettings.ColorSettings;
            _formSettings = configProvider.AppSettings.FormSettings;
            _gameSettings = configProvider.AppSettings.GameSettings;

            InitializeComponent();
            BackColor = _colorSettings.BackgroundColor;
            ClientSize = new System.Drawing.Size(_formSettings.ScreenWidth, _formSettings.ScreenHeight);

            cbStrategy.Items.AddRange(_gameSettings.Strategies.ToArray());
            cbStrategy.SelectedIndex = 0;
        }

        public void Initialize(Strategy strategy)
        {
            var itemList = cbStrategy.Items.OfType<Strategy>();
            cbStrategy.SelectedIndex = cbStrategy.Items.IndexOf(itemList.Single(l => l.GetType() == strategy.GetType()));
        }

        private void ToggleMctsParams(bool switchVal)
        {
            lbC.Visible = switchVal;
            lbSimulations.Visible = switchVal;
            lbAgent.Visible = switchVal;

            cbC.Visible = switchVal;
            cbSimulations.Visible = switchVal;
            cbAgent.Visible = switchVal;
        }

        private void ToggleMinimaxParams(bool switchVal)
        {
            lbDepth.Visible = switchVal;
            cbMinimaxDepth.Visible = switchVal;

            cbMinimaxDepth.SelectedIndex = 1;
        }

        private void ToggleMCTSAgentParams(bool switchVal)
        {
            lbSeed.Visible = switchVal;
            numSeed.Visible = switchVal;
        }

        private void cbStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedStrategy = ((ComboBox)sender).Items[cbStrategy.SelectedIndex] as Strategy;

            UpdateLabels(selectedStrategy);
        }

        private void UpdateLabels(Strategy selectedStrategy)
        {
            if (selectedStrategy is NonParamStrategy)
            {
                lbParameters.Visible = false;
                ToggleMctsParams(false);
                ToggleMinimaxParams(false);
                ToggleMCTSAgentParams(false);
                return;
            }

            // change visibility of parameters
            lbParameters.Visible = true;

            if (selectedStrategy is MinimaxStrategy)
            {
                ToggleMctsParams(false);
                ToggleMCTSAgentParams(false);

                ToggleMinimaxParams(true);

            }
            else if (selectedStrategy is MctsStrategy mcts)
            {
                ToggleMinimaxParams(false);
                ToggleMCTSAgentParams(false);

                ToggleMctsParams(true);

                cbC.Text = mcts.C.ToString();
                cbSimulations.Text = mcts.Simulations.ToString();
                cbAgent.Items.AddRange(_gameSettings.Strategies.Where(x => x is MCTSAgent).ToArray());
                cbAgent.SelectedIndex = 0;
            }
            else if (selectedStrategy is MCTSAgent mctsAgent)
            {
                ToggleMinimaxParams(false);
                ToggleMctsParams(false);

                ToggleMCTSAgentParams(true);

                numSeed.Text = mctsAgent.Seed.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var selectedStategy = (Strategy)cbStrategy.Items[cbStrategy.SelectedIndex];

            if (selectedStategy is MCTSAgent agent)
            {
                agent.Seed = (int)numSeed.Value;
            }
            else if (selectedStategy is MinimaxStrategy minimaxAgent)
            {
                minimaxAgent.Depth = int.Parse(cbMinimaxDepth.Text);
            }
            else if (selectedStategy is MctsStrategy mctsStrategy)
            {
                mctsStrategy.C = double.Parse(cbC.Text);
                mctsStrategy.Simulations = int.Parse(cbSimulations.Text);
                mctsStrategy.SimulationStrategy = (MCTSAgent)cbAgent.Items[cbAgent.SelectedIndex];
            }
            OnOkButtonPress?.Invoke(this, selectedStategy);
        }
    }
}
