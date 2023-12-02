using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.Core.Game;
using Quoridor.DesktopApp.Configuration;
using Quoridor.Core.Environment;
using Quoridor.DesktopApp.MainGameForm;
using System.Drawing;
using Quoridor.Core.Utils;
using System;
using System.Linq;
using Castle.DynamicProxy.Generators;

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
        
        private Timer _timer;
        private IGameEnvironment _game;
        private bool PlayerSelected;
        private Wall SelectedWall;

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
            _timer = new(new System.ComponentModel.Container()) { Interval = 60 };

            BackColor = _colorSettings.BackgroundColor;
            Initialize();
            InitializeComponent();
            SetStyle(flag: ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, value: true);
        }

        public void Initialize()
        {
            _board.SetDimension(_gameSettings.Dimension);
            _game = _gameFactory.CreateGameEnvironment(_gameSettings.Players, _gameSettings.Walls);
            PerformSavedMoves();
        }

        private void PerformSavedMoves()
        {
            foreach(var move in _gameSettings.Moves)
            {
                _game.Move(move.GetMovement());
            }
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            DrawBackground(args.Graphics);
            DrawPlayers(args.Graphics);
            DrawWalls(args.Graphics);
            DrawCurrentPlayerStats(args.Graphics);
        }


        private void DrawBackground(Graphics graphics)
        {
            var cellSize = _configProvider.AppSettings.CellSize;

            for (int i = 0; i < _board.Dimension; i++)
            {
                for (int j = 0; j < _board.Dimension; j++)
                {
                    var color = (j % 2 == i % 2) ? _colorSettings.EvenTileColor : _colorSettings.OddTileColor;
                    var start_i = GetAdjustedPos(i);
                    var start_j = GetAdjustedPos(j, _formSettings.OffsetY);
                    var rectangle = new Rectangle(start_i, start_j, cellSize, cellSize);
                    if (_game.CurrentPlayer.CurrX == i && _game.CurrentPlayer.CurrY == j)
                    {
                        DrawFilledSquare(graphics, _colorSettings.CurrentPlayerCellColor, rectangle);
                        rectangle.Width -= 5;
                        rectangle.Height -= 5;
                    }
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
            if (SelectedWall is not null)
            {
                var rectangle = CreateWallRectangle(SelectedWall);
                DrawFilledSquare(graphics, _colorSettings.PossibleWallColor, rectangle);
            }
        }

        private void DrawCurrentPlayerStats(Graphics graphics)
        {
            var fontSize = _formSettings.OffsetY / 4;
            using var font = new Font(_fontSettings.PlayerFont, fontSize);
            using var brush = new SolidBrush(_colorSettings.PlayerStatsColor);
            var text = $"Player {_game.CurrentPlayer}'s turn. {_game.CurrentPlayer.NumWalls} walls left.";
            graphics.DrawString(text, font, brush, new PointF(5, 10));
        }

        private Rectangle CreateWallRectangle(IWall wall)
        {
            var cellSize = _configProvider.AppSettings.CellSize;
            var width = _formSettings.WallWidth;
            var height = _configProvider.AppSettings.WallHeight;

            var x = GetAdjustedPos(wall.From.X) - width;
            var y = GetAdjustedPos(wall.From.Y, _formSettings.OffsetY) - width;

            return wall.Placement switch
            {
                Direction.North => new Rectangle(x + width, y, height, width),
                Direction.South => new Rectangle(x + width, y + cellSize + width, height, width),
                Direction.East => new Rectangle(x + cellSize + width, y + width, width, height),
                _ => new Rectangle(x, y + width, width, height)

            };
        }

        private void DrawPlayers(Graphics graphics)
        {
            var fontSize = _configProvider.AppSettings.CellSize / 5;
            var adjustedMidCellPos = (float)_configProvider.AppSettings.CellSize / 2;

            using var font = new Font(_fontSettings.PlayerFont, fontSize);
            using var brush = new SolidBrush(_colorSettings.PlayerColor);

            foreach(var player in _game.Players)
            {
                var x = GetAdjustedPos(player.CurrX) + adjustedMidCellPos - fontSize / 2;
                var y = GetAdjustedPos(player.CurrY, _formSettings.OffsetY) + adjustedMidCellPos - fontSize / 2;
                graphics.DrawString(player.ToString(), font, brush, new PointF(x, y));
            }
        }

        private int GetAdjustedPos(int pos, int offset=0)
        {
            return offset + pos * (_configProvider.AppSettings.CellSize + _formSettings.WallWidth);
        }

        private void DrawFilledSquare(Graphics graphics, Color color, Rectangle rect)
        {
            using var brush = new SolidBrush(color);
            graphics.FillRectangle(brush, rect);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _colorSettings.UpdateOpacity();
            Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_game.HasFinished || _gameSettings.CurrentStrategy is not HumanStrategy)
            {
                StopUpdatingOpacity();
                return;
            }

            if (_configProvider.AppSettings.WithinCellBounds(e.Location, out Vector2 cellPos))
            {
                if (_game.CurrentPlayer.CurrX == cellPos.X && _game.CurrentPlayer.CurrY == cellPos.Y)
                {
                    PlayerSelected = true;
                }
                else if (SelectedWall is null)
                {
                    SelectedWall = new (Direction.North, new(cellPos.X, cellPos.Y));
                }
                else if (SelectedWall.From.Equals(cellPos))
                {
                    SetNextNonIntersectingWall();
                }
                StartUpdatingOpacity();
            }
            else
            {
                SelectedWall = null;
                StopUpdatingOpacity();
            }
        }

        private void StopUpdatingOpacity()
        {
            _timer.Tick -= OnTimerTick;
            Invoke((MethodInvoker)delegate {
                _timer.Stop();
            });
        }

        private void StartUpdatingOpacity()
        {
            _colorSettings.OpacityUpdateDirection = Direction.North;
            _colorSettings.AnimatableOpacity = 0;
            _timer.Tick += OnTimerTick;
            Invoke((MethodInvoker) delegate { 
                _timer.Start();
            });
        }

        private void SetNextNonIntersectingWall()
        {
            var currentDir = (int)SelectedWall.Placement;
            var nextDir = (currentDir + 1) % 4;

            while(nextDir != currentDir)
            {
                SelectedWall.Placement = (Direction)nextDir;
                if (_game.Walls.All(w => !w.Intersects(SelectedWall)))
                    return;
                nextDir = (nextDir + 1) % 4;
            }
            SelectedWall = null;
        }

    }
}
