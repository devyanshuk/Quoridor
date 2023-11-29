using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.Core.Game;
using Quoridor.DesktopApp.Configuration;
using Quoridor.Core.Environment;
using Quoridor.DesktopApp.MainGameForm;
using System.Drawing;

namespace Quoridor.DesktopApp
{
    public partial class MainForm : Form
    {
        private readonly IWindsorContainer _container;
        private readonly ILocalSettings _localSettings;
        private readonly IConfigProvider _configProvider;
        private readonly IGameFactory _gameFactory;
        private readonly IBoard _board;
        private readonly ColorSettings _colorSettings;
        private readonly FormSettings _formSettings;
        private IGameEnvironment _game;

        private int _cellSize;
        private readonly int _wallWidth;

        private WindowType _windowType = WindowType.MainMenu;

        public MainForm(IWindsorContainer container)
        {
            _container = container;
            _localSettings = _container.Resolve<ILocalSettings>();
            _configProvider = _container.Resolve<IConfigProvider>();
            _gameFactory = _container.Resolve<IGameFactory>();
            _colorSettings = _configProvider.AppSettings.ColorSettings;
            _formSettings = _configProvider.AppSettings.FormSettings;
            _board = _container.Resolve<IBoard>();

            Initialize();
            InitializeComponent();
            SetStyle(flag: ControlStyles.AllPaintingInWmPaint
                            | ControlStyles.UserPaint
                            | ControlStyles.DoubleBuffer,
                     value: true);

        }

        public void Initialize()
        {
            _board.SetDimension(9);
            _game = _gameFactory.CreateGameEnvironment(2, 8);
            _cellSize = GetCellSize();

        }

        protected override void OnPaint(PaintEventArgs args)
        {
            DrawBackground(args.Graphics);
        }

        private void DrawBackground(Graphics graphics)
        {
            for (int i = 0; i < _board.Dimension; i++)
            {
                for (int j = 0; j < _board.Dimension; j++)
                {
                    var color = (j % 2 == i % 2) ? _colorSettings.EvenTileColor : _colorSettings.OddTileColor;
                    var start_i = i * _cellSize + _formSettings.OffsetX;
                    var start_j = j * _cellSize + _formSettings.OffsetY;
                    var rectangle = new Rectangle(start_i, start_j, _cellSize, _cellSize);
                    DrawFilledSquare(graphics, color, rectangle);
                }
            }
        }

        private void DrawFilledSquare(Graphics graphics, Color color, Rectangle rect)
        {
            using (var brush = new SolidBrush(color))
            {
                graphics.FillRectangle(brush, rect);
            }
        }

        private int GetCellSize()
        {
            var cellSize = (_formSettings.ScreenWidth - 2 * _formSettings.OffsetX) / _board.Dimension;
            return cellSize;
        }
    }
}
