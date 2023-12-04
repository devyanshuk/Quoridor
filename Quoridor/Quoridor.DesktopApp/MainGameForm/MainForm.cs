using System;
using System.Linq;
using System.Drawing;
using Castle.Windsor;
using System.Windows.Forms;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using Quoridor.DesktopApp.MainGameForm;
using Quoridor.DesktopApp.Configuration;

namespace Quoridor.DesktopApp
{
    public partial class MainForm : Form
    {
        public EventHandler OnMainMenuPressed;


        private readonly IConfigProvider _configProvider;
        private readonly IGameFactory _gameFactory;
        private readonly IBoard _board;
        private readonly ColorSettings _colorSettings;
        private readonly FormSettings _formSettings;
        private readonly GameSettings _gameSettings;
        private readonly FontSettings _fontSettings;

        private System.Timers.Timer _timer;
        private IGameEnvironment _game;
        private bool PlayerSelected;
        private Wall SelectedWall;

        private WindowType _windowType = WindowType.MainMenu;

        public MainForm(IWindsorContainer container)
        {
            _configProvider = container.Resolve<IConfigProvider>();

            _gameFactory = container.Resolve<IGameFactory>();
            _colorSettings = _configProvider.AppSettings.ColorSettings;
            _formSettings = _configProvider.AppSettings.FormSettings;
            _gameSettings = _configProvider.AppSettings.GameSettings;
            _fontSettings = _configProvider.AppSettings.FontSettings;
            _board = container.Resolve<IBoard>();
            _timer = new() { Interval = 60 };
            _timer.Elapsed += OnTimerTick;

            InitializeComponent();
            BackColor = _colorSettings.BackgroundColor;
            ClientSize = new(_formSettings.ScreenWidth, _formSettings.ScreenHeight);

            SetStyle(flag: ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, value: true);
        }

        public void Initialize()
        {
            _board.SetDimension(_gameSettings.Dimension);
            if (_game != null) _gameFactory.Release(_game);
            _game = _gameFactory.CreateGameEnvironment(_gameSettings.Players, _gameSettings.Walls);
            PerformSavedMoves();
        }

        private void PerformSavedMoves()
        {
            foreach (var move in _gameSettings.Moves)
            {
                _game.Move(move.GetMovement());
            }
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            DrawBackground(args.Graphics);
            DrawPlayers(args.Graphics);
            DrawWalls(args.Graphics);
            DrawCurrentPlayerStats();
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
            foreach (var wall in _game.Walls)
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

        private void DrawCurrentPlayerStats()
        {
            lbInfoPlayer.Text = $"Player {_game.CurrentPlayer}'s turn. {_game.CurrentPlayer.NumWalls} walls left.";
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

            foreach (var player in _game.Players)
            {
                var x = GetAdjustedPos(player.CurrX) + adjustedMidCellPos - fontSize / 2;
                var y = GetAdjustedPos(player.CurrY, _formSettings.OffsetY) + adjustedMidCellPos - fontSize / 2;
                graphics.DrawString(player.ToString(), font, brush, new PointF(x, y));
            }
        }

        private int GetAdjustedPos(int pos, int offset = 0)
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
                if (_game.CurrentPlayer.CurrentPos.Equals(cellPos))
                {
                    PlayerSelected = true;
                }
                else if (!SelectedWall?.From.Equals(cellPos) ?? true)
                {
                    SelectedWall = new(Direction.North, new(cellPos.X, cellPos.Y));
                    StartUpdatingOpacity();
                }
                else
                {
                    SetNextNonIntersectingWall();
                }
            }
            else
            {
                SelectedWall = null;
                StopUpdatingOpacity();
            }
        }

        private void StopUpdatingOpacity()
        {
            _timer.Enabled = false;
        }

        private void StartUpdatingOpacity()
        {
            _colorSettings.OpacityUpdateDirection = Direction.North;
            _colorSettings.AnimatableOpacity = 0;
            _timer.Enabled = true;
        }

        private void SetNextNonIntersectingWall()
        {
            var currentDir = (int)SelectedWall.Placement;
            var nextDir = (currentDir + 1) % 4;

            while (nextDir != currentDir)
            {
                SelectedWall.Placement = (Direction)nextDir;
                if (_game.Walls.All(w => !w.Intersects(SelectedWall)))
                    return;
                nextDir = (nextDir + 1) % 4;
            }
            SelectedWall = null;
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            OnMainMenuPressed?.Invoke(this, null);
        }
    }
}
