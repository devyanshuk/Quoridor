using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.Core.Game;
using Quoridor.DesktopApp.Configuration;
using Quoridor.Core.Environment;
using Quoridor.DesktopApp.MainGameForm;
using System.Drawing;
using Quoridor.Core.Utils;

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
        private readonly GameSettings _gameSettings;
        private readonly FontSettings _fontSettings;
        private IGameEnvironment _game;

        private int _cellSize;

        private WindowType _windowType = WindowType.MainMenu;

        public MainForm(IWindsorContainer container)
        {
            _container = container;
            _localSettings = _container.Resolve<ILocalSettings>();
            _configProvider = _container.Resolve<IConfigProvider>();
            _gameFactory = _container.Resolve<IGameFactory>();
            _colorSettings = _configProvider.AppSettings.ColorSettings;
            _formSettings = _configProvider.AppSettings.FormSettings;
            _gameSettings = _configProvider.AppSettings.GameSettings;
            _fontSettings = _configProvider.AppSettings.FontSettings;
            _board = _container.Resolve<IBoard>();

            BackColor = _colorSettings.BackgroundColor;
            Initialize();
            InitializeComponent();
            SetStyle(flag: ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, value: true);
        }

        public void Initialize()
        {
            _board.SetDimension(9);
            _game = _gameFactory.CreateGameEnvironment(2, 8);
            _cellSize = GetCellSize();
            _game.AddWall(_game.CurrentPlayer, new Vector2(5, 5), Direction.North);
            _game.AddWall(_game.CurrentPlayer, new Vector2(5, 5), Direction.South);
            _game.AddWall(_game.CurrentPlayer, new Vector2(5, 5), Direction.West);
            _game.AddWall(_game.CurrentPlayer, new Vector2(6, 5), Direction.East);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            DrawBackground(args.Graphics);
            DrawPlayers(args.Graphics);
            DrawWalls(args.Graphics);
        }

        private void DrawBackground(Graphics graphics)
        {
            for (int i = 0; i < _board.Dimension; i++)
            {
                for (int j = 0; j < _board.Dimension; j++)
                {
                    var color = (j % 2 == i % 2) ? _colorSettings.EvenTileColor : _colorSettings.OddTileColor;
                    var start_i = GetAdjustedPos(i, _formSettings.OffsetX);
                    var start_j = GetAdjustedPos(j, _formSettings.OffsetY);
                    var rectangle = new Rectangle(start_i, start_j, _cellSize, _cellSize);
                    DrawFilledSquare(graphics, color, rectangle);
                }
            }
        }

        private void DrawWalls(Graphics graphics)
        {

            foreach(var wall in _game.Walls)
            {
                var rectangle = CreateWallRectangle(wall);
                DrawFilledSquare(graphics, _colorSettings.WallColor, rectangle);
            }
        }

        private Rectangle CreateWallRectangle(IWall wall)
        {
            var width = _gameSettings.WallWidth;
            var height = _cellSize * 2 + _gameSettings.WallWidth;

            var x = GetAdjustedPos(wall.From.X, _formSettings.OffsetX) - width;
            var y = GetAdjustedPos(wall.From.Y, _formSettings.OffsetY) - width;

            return wall.Placement switch
            {
                Direction.North => new Rectangle(x + width, y, height, width),
                Direction.South => new Rectangle(x + width, y + _cellSize + width, height, width),
                Direction.East => new Rectangle(x + _cellSize + width, y + width, width, height),
                _ => new Rectangle(x, y + width, width, height)

            };
        }

        private void DrawPlayers(Graphics graphics)
        {
            var fontSize = _cellSize / 5;
            var adjustedMidCellPos = (float)_cellSize / 2;

            using var font = new Font(_fontSettings.PlayerFont, fontSize);
            using var brush = new SolidBrush(_colorSettings.PlayerColor);

            foreach(var player in _game.Players)
            {
                var x = GetAdjustedPos(player.CurrX, _formSettings.OffsetX) + adjustedMidCellPos - _gameSettings.WallWidth;
                var y = GetAdjustedPos(player.CurrY, _formSettings.OffsetY) + adjustedMidCellPos - _gameSettings.WallWidth;
                graphics.DrawString(player.ToString(), font, brush, new PointF(x, y));
            }
        }

        private int GetAdjustedPos(int pos, int offset)
        {
            return offset + pos * (_cellSize + _gameSettings.WallWidth);
        }

        private void DrawFilledSquare(Graphics graphics, Color color, Rectangle rect)
        {
            using var brush = new SolidBrush(color);
            graphics.FillRectangle(brush, rect);
        }

        private int GetCellSize()
        {
            var totalWallWidth = _gameSettings.WallWidth * (_board.Dimension - 2);
            var cellSize = (_formSettings.ScreenWidth - 2 * _formSettings.OffsetX - totalWallWidth) / _board.Dimension;
            return cellSize;
        }
    }
}
