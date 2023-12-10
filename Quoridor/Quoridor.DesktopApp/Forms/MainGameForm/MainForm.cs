using System;
using System.Linq;
using System.Drawing;
using Castle.Windsor;
using System.Windows.Forms;
using System.Collections.Generic;

using Quoridor.Core.Game;
using Quoridor.Core.Utils;
using Quoridor.Core.Environment;
using Quoridor.DesktopApp.Configuration;
using Quoridor.Core.Utils.CustomExceptions;

namespace Quoridor.DesktopApp.Forms.MainGameForm
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
        private List<Wall> SelectedWalls = new List<Wall>();
        private int SelectedWallTurn = -1;

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

            if (_game == null)
            {
                _game = _gameFactory.CreateGameEnvironment(_gameSettings.Players, _gameSettings.Walls);
                _game.OnMoveDone += OnGameMove;
            }
            else
            {
                _game.InitAndAddPlayers(_gameSettings.Players, _gameSettings.Walls);
            }
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
                    var currTile = new Vector2(i, j);

                    if (_game.CurrentPlayer.CurrentPos.Equals(currTile))
                    {
                        DrawFilledSquare(graphics, _colorSettings.CurrentPlayerCellColor, rectangle);
                        rectangle.Width -= 10;
                        rectangle.Height -= 10;
                        rectangle.X += 5;
                        rectangle.Y += 5;
                    }
                    if (_game.CurrentPlayer.IsGoalMove(currTile))
                    {
                        DrawFilledSquare(graphics, _colorSettings.GoalTileColor, rectangle);
                    }
                    else
                    {
                        DrawFilledSquare(graphics, color, rectangle);
                    }
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
            if (SelectedWalls.Count > 0)
            {
                var rectangle = CreateWallRectangle(SelectedWalls[SelectedWallTurn]);
                DrawFilledSquare(graphics, _colorSettings.PossibleWallColor, rectangle);
            }
        }

        private void DrawCurrentPlayerStats()
        {
            var strat = _gameSettings.CurrentStrategy;
            var p = _game.CurrentPlayer;
            var walls = p.NumWalls;
            if (!_game.HasFinished)
            {
                lbInfoPlayer.Text = $"Player {p}'s turn. {walls} walls left. Using {strat} strategy";
            }
            else
            {
                lbInfoPlayer.Text = $"Game Over. Player {_game.Winner} wins";
            }
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

        private void OnGameMove(object sender, EventArgs e)
        {
            _gameSettings.NextTurn();
            StopUpdatingOpacity();

            Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_game.HasFinished)
            {
                return true;
            }

            if (_gameSettings.CurrentStrategy is not HumanStrategy && keyData == Keys.M)
            {
                var strategy = _gameSettings.CurrentStrategy.GetStrategy();
                var bestMove = strategy.BestMove(_game, _game.CurrentPlayer).BestMove;
                _game.Move(bestMove);
                Invalidate();
                return true;
            }
            if (_gameSettings.CurrentStrategy is HumanStrategy)
            {
                AgentMove move = null;

                if (keyData == Keys.W || keyData == Keys.Up)
                    move = new(Direction.North);
                else if (keyData == Keys.S || keyData == Keys.Down)
                    move = new(Direction.South);
                else if (keyData == Keys.A || keyData == Keys.Left)
                    move = new(Direction.West);
                else if (keyData == Keys.D || keyData == Keys.Right)
                    move = new(Direction.East);

                if (move is not null)
                {
                    try
                    {
                        _game.Move(move);
                    }
                    catch(Exception) { }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_game.HasFinished || _gameSettings.CurrentStrategy is not HumanStrategy)
            {
                StopUpdatingOpacity();
                return;
            }

            var leftClick = e.Button == MouseButtons.Left;
            var rightClick = e.Button == MouseButtons.Right;

            if (_configProvider.AppSettings.WithinCellBounds(e.Location, out Vector2 cellPos))
            {
                if (leftClick)
                {
                    if (_game.CurrentPlayer.CurrentPos.Equals(cellPos))
                    {
                        PlayerSelected = true;
                    }
                    else if (SelectedWalls.Count == 0 || !SelectedWalls[SelectedWallTurn].From.Equals(cellPos))
                    {
                        SetAvailableWallsInOrder(cellPos);
                        if (SelectedWalls.Count >= 0) StartUpdatingOpacity();
                    }
                    else
                    {
                        SelectedWallTurn = (SelectedWallTurn + 1) % SelectedWalls.Count;
                    }
                }
                else if (rightClick)
                {
                    if (SelectedWalls.Count > 0)
                    {
                        _game.Move(SelectedWalls[SelectedWallTurn]);
                    }
                }
            }
            else
            {
                StopUpdatingOpacity();
            }
        }

        private void StopUpdatingOpacity()
        {
            _timer.Enabled = false;
            SelectedWalls.Clear();
            SelectedWallTurn = -1;
        }

        private void StartUpdatingOpacity()
        {
            _colorSettings.OpacityUpdateDirection = Direction.North;
            _colorSettings.AnimatableOpacity = 0;
            _timer.Enabled = true;
        }

        private void SetAvailableWallsInOrder(Vector2 currFrom)
        {
            SelectedWalls.Clear();
            SelectedWallTurn = -1;

            for (int i = 0; i < 4; i++)
            {
                var currDir = (Direction)i;
                try
                {
                    _game.AddWall(_game.CurrentPlayer, currFrom, currDir);
                    _game.RemoveWall(_game.CurrentPlayer, currFrom, currDir);
                    SelectedWalls.Add(new(currDir, currFrom));
                    SelectedWallTurn = 0;
                }
                catch(WallException)
                {
                }
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            OnMainMenuPressed?.Invoke(this, null);
        }
    }
}
