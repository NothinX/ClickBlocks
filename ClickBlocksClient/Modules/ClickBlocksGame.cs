using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClickBlocksClient
{
    class ClickBlocksGame
    {
        private static readonly Style KEY_BUTTON_STYLE = App.Current.Resources.MergedDictionaries[0]["KeyButton"] as Style;
        private static readonly Color RED = Color.FromArgb(255, 232, 17, 35);
        private static readonly Color BLUE = Color.FromArgb(255, 0, 99, 177);
        private static readonly Color YELLOW = Color.FromArgb(255, 255, 140, 0);

        /// <summary>
        /// 方块类型
        /// </summary>
        private enum BlockTypes { Rigth, Error };
        /// <summary>
        /// 色块颜色类型
        /// </summary>
        private enum ColorTypes { BaseColor, ClickedColor };
        private class BlockStatus
        {
            public BlockStatus(BlockTypes type)
            {
                BlockType = type;
                IsClicked = false;
            }
            public BlockTypes BlockType { get; set; }
            public bool IsClicked { get; set; }
        }
        /// <summary>
        /// 键位设置
        /// </summary>
        IDictionary<Key, int> KeyMap = new Dictionary<Key, int>()
        {
            {Key.Z, 0 },
            {Key.X, 1 },
            {Key.C, 2 },
            {Key.V, 3 },
        };
        /// <summary>
        /// 记录色块颜色信息
        /// </summary>
        IDictionary<BlockTypes, IDictionary<ColorTypes, Color>> ColorMap = new Dictionary<BlockTypes, IDictionary<ColorTypes, Color>>()
        {
            {BlockTypes.Error, new Dictionary<ColorTypes, Color>()
            {
                {ColorTypes.BaseColor,  Colors.White},
                {ColorTypes.ClickedColor,  RED}
            }},
            {BlockTypes.Rigth, new Dictionary<ColorTypes, Color>()
            {
                {ColorTypes.BaseColor,  Colors.Black},
                {ColorTypes.ClickedColor,  BLUE}
            }}
        };
        public Grid Playground { get; set; }
        public List<string> Sheet { get; set; }
        public double Fps { get; set; }
        public double Speed { get; set; }
        private List<string> playSheet = new List<string>();
        private ConcurrentQueue<Grid> gs = new ConcurrentQueue<Grid>();
        private ConcurrentQueue<Button>[] ColumnBlocks = new ConcurrentQueue<Button>[4];
        private volatile bool IsWin = true;
        private volatile int ClickedCounter = 0;
        private CancellationTokenSource cts;
        private Window Context;

        public ClickBlocksGame(Window context)
        {
            Context = context;
            Playground = new Grid();
            Speed = 3;
            CreateSheet();
            Reset();
        }

        public ClickBlocksGame(List<string> sheet)
        {
            Playground = new Grid();
            Speed = 3;
            Sheet = sheet;
            Reset();
        }

        public ClickBlocksGame(double speed)
        {
            Playground = new Grid();
            Speed = speed;
            CreateSheet();
            Reset();
        }

        public ClickBlocksGame(List<string> sheet, double speed)
        {
            Playground = new Grid();
            Speed = speed;
            Sheet = sheet;
            Reset();
        }

        public void ReStart(bool reset = false)
        {
            if (reset) CreateSheet();
            Reset();
        }

        public void ReStart(List<string> sheet)
        {
            Sheet = sheet;
            Reset();
        }

        private void Reset()
        {
            Fps = 0;
            cts = new CancellationTokenSource();
            IsWin = true;
            playSheet.Clear();
            Sheet.ForEach(x => playSheet.Add(x));
            ClickedCounter = playSheet.Count;
            Playground.IsEnabled = false;
            gs = new ConcurrentQueue<Grid>();
            Playground.Children.Clear();
            for (int i = 0; i < 4; i++) ColumnBlocks[i] = new ConcurrentQueue<Button>();
            for (int i = 2; i >= -2; i--)
            {
                CreateBlocks(i * 115.44);
            }
        }

        public async Task<bool> Start(double speed = 3)
        {
            Reset();
            Playground.IsEnabled = true;
            Context.KeyDown += KeyListener;
            Speed = speed;
            var gameover = GameOver();
            var game = StartGame();
            var draw = Draw();
            await gameover;
            await draw;
            return await game;
        }

        private async Task<bool> StartGame()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int fps = 0;
            while (!cts.IsCancellationRequested)
            {
                gs.ToList().ForEach(x =>
                {
                    x.Margin = new Thickness(x.Margin.Left, x.Margin.Top + Speed, x.Margin.Right, x.Margin.Bottom);
                });
                fps++;
                await Task.Delay(1);
            }
            Playground.IsEnabled = false;
            Fps = fps * 1.0 / sw.ElapsedMilliseconds * 1000.0;
            return IsWin;
        }

        private async Task Draw()
        {
            //Stopwatch sw = Stopwatch.StartNew();
            //int sp = 0;
            while (!cts.IsCancellationRequested)
            {
                Grid g = null;
                if (gs.TryPeek(out g))
                {
                    if (g.Margin.Top >= 115.44 * 4)
                    {
                        gs.TryDequeue(out g);
                        Playground.Children.Remove(g);
                        foreach (var btn in g.Children)
                        {
                            var b = btn as Button;

                            if (b != null)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    Button peek;
                                    if (ColumnBlocks[i].TryPeek(out peek) && peek == btn)
                                    {
                                        ColumnBlocks[i].TryDequeue(out peek);
                                    }
                                }
                                var blockStatus = b.Tag as BlockStatus;
                                if (blockStatus.BlockType == BlockTypes.Rigth
                                    && blockStatus.IsClicked == false)
                                {
                                    IsWin = false;
                                    break;
                                }
                            }
                        }
                        if (gs.Count <= 0)
                        {
                            break;
                        }
                        else
                        {
                            CreateBlocks(gs.Last().Margin.Top - 115.44);
                            //sp++;
                        }
                    }
                }
                await Task.Delay(1);
            }
            //Fps = sp * 1.0 / sw.ElapsedMilliseconds * 1000.0;
        }

        private async Task GameOver()
        {
            while (IsWin && ClickedCounter != 0)
            {
                await Task.Delay(1);
            }
            cts.Cancel();
            Context.KeyDown -= KeyListener;
        }

        private void CreateSheet()
        {
            Sheet = new List<string>();
            Random r = new Random();
            for (int i = 0; i < 100; ++i)
            {
                int site = r.Next(4);
                string str = "";
                switch (site)
                {
                    case 0:
                        str = string.Format("{0}111", 0);
                        break;
                    case 1:
                        str = string.Format("1{0}11", 0);
                        break;
                    case 2:
                        str = string.Format("11{0}1", 0);
                        break;
                    case 3:
                        str = string.Format("11{0}1", 0);
                        break;
                    default:
                        break;
                }
                Sheet.Add(str);
            }
        }

        private Button CreateBlock(double x, BlockTypes type)
        {
            Button b = new Button()
            {
                Style = KEY_BUTTON_STYLE,
                Background = new SolidColorBrush(ColorMap[type][ColorTypes.BaseColor]),
                Tag = new BlockStatus(type),
                Margin = new Thickness(x, 0, 0, 0),
                Height = 115.44,
                Width = 82.25,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            b.Click += (s, e) => Click(s as Button, type);
            return b;
        }

        private bool CreateBlocks(double top)
        {
            if (playSheet.Count <= 0) return false;
            Grid g = new Grid()
            {
                Margin = new Thickness(0, top, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            gs.Enqueue(g);
            Playground.Children.Add(g);
            var key = playSheet.First();
            for (int i = 0; i < 4; i++)
            {
                Button btn = null;
                if (key[i] == '0')
                {
                    btn = CreateBlock(i * 82.25, BlockTypes.Rigth);
                }
                else
                {
                    btn = CreateBlock(i * 82.25, BlockTypes.Error);
                }
                g.Children.Add(btn);
                ColumnBlocks[i].Enqueue(btn);
            }
            playSheet.RemoveAt(0);
            return true;
        }
        private void KeyListener(object sender, KeyEventArgs e)
        {
            int column;
            if (!KeyMap.TryGetValue(e.Key, out column)) return;
            var cur = ColumnBlocks[column];
            Button firstButton;
            cur.TryPeek(out firstButton);
            while (cur.Count != 0)
            {
                Button curButton;
                cur.TryDequeue(out curButton);
                var blockStatus = curButton.Tag as BlockStatus;
                if (blockStatus.BlockType == BlockTypes.Rigth)
                {
                    Click(curButton, BlockTypes.Rigth);
                    return;
                }
            }
            Click(firstButton, BlockTypes.Error);
        }
        private void Click(Button btn, BlockTypes type)
        {
            btn.Background = new SolidColorBrush(ColorMap[type][ColorTypes.ClickedColor]);
            var blockStatus = btn.Tag as BlockStatus;
            blockStatus.IsClicked = true;
            if (type == BlockTypes.Error)
            {
                //btn.Tag = clickColor.ToString();
                IsWin = false;
            }
            else
            {
                //btn.Tag = clickColor.ToString();
                ClickedCounter--;
            }
        }
    }
}
