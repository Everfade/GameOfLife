using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game gof;
        Label[,] labels = new Label[20, 20];
        //Grid grid = new Grid();
        bool gameStarted = false;
        public MainWindow()
        {
            InitializeComponent();
            //Dynamicaly create 400 lables in a square

         /*   for (int i = 0; i < 20; i++)
            {

                ColumnDefinition cd = new ColumnDefinition()
                {
                    Width = new GridLength(31, GridUnitType.Star)
                };
                grid.ColumnDefinitions.Add(cd);
                RowDefinition rd = new RowDefinition()
                {
                    Height = new GridLength(31, GridUnitType.Star)
                };
              grid.RowDefinitions.Add(rd);
                
            }
            */
                    for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    labels[i, j] = new Label()
                    {
                        Background = Brushes.LightGray,
                        Width = 30,
                        Height = 30,
                    };
                    labels[i, j].AddHandler(MouseLeftButtonDownEvent, new RoutedEventHandler(Label_MouseLeftButtonDown));
                    Grid.SetRow(labels[i, j], i);
                    Grid.SetColumn(labels[i, j], j);
                    mainGrid.Children.Add(labels[i, j]);
                }
            }
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (loopCB.IsChecked.Value)
            {
                lock (labels)
                {
                    if (!gameStarted)
                    {
                        bool[,] grid = new bool[20, 20];
                        for (int i = 0; i < 20; i++)
                        {
                            for (int j = 0; j < 20; j++)
                            {
                                if (labels[i, j].Background == Brushes.LightGray)
                                {
                                    grid[i, j] = false;
                                }
                                else
                                {
                                    grid[i, j] = true;
                                }
                            }
                        }
                        gof = new Game(grid, 20, 20);
                        StartButton.Content = "Next";
                        gof.StartRound();
                        ConvertArray2Cells();
                        gameStarted = true;
                    }
                    if (gameStarted)
                    {                 
                        //Check if no copy has been found if so then cancel 
                        while (!gof.Check4Copy())
                        {
                            gof.StartRound();
                            ConvertArray2Cells();
                            Thread.Sleep(200);
                            if (gof == null)
                            {
                                return;
                            }
                        }
                        MessageLable.Content = "Infinite loop detected, simulation has been stopped";
                        ResetGame();
                    }
                }
            }
            // No Loop
            else
            {
                if (gameStarted)
                {
                    gof.StartRound();
                    ConvertArray2Cells();
                }
                else
                {
                    bool[,] grid = new bool[20, 20];
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            if (labels[i, j].Background == Brushes.LightGray)
                            {
                                grid[i, j] = false;
                            }
                            else
                            {
                                grid[i, j] = true;
                            }
                        }
                    }
                    gof = new Game(grid, 20, 20);

                    StartButton.Content = "Next";
                    gof.StartRound();
                    ConvertArray2Cells();
                    gameStarted = true;
                }
            }
        } 
        private void Label_MouseLeftButtonDown(object sender, RoutedEventArgs e)
            {
            MessageLable.Content = "";
            if (gameStarted)
                    return;
                Label sl = e.Source as Label;
                if (sl.Background == Brushes.Green)
                {
                    sl.Background = Brushes.LightGray;
                }
                else
                    sl.Background = Brushes.Green;
            }
        private void ResetGame()
            { 
            gameStarted = false;
                gof = null;
                GC.Collect();
                StartButton.Content = "Start";
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        labels[i, j].Background = Brushes.LightGray;
                    }
                }
            }
        private void ConvertArray2Cells()
            {
                lock (labels)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            if (!gof.GetCurrentState()[i, j])
                            {
                                labels[i, j].Background = Brushes.LightGray;
                            }
                            else
                            {
                                labels[i, j].Background = Brushes.Green;
                            }
                        }
                    }
                }
                ProcessUITasks();
            }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
            {
                ResetGame();
            }
        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
            {
                System.Diagnostics.Process.Start("https://de.wikipedia.org/wiki/Conways_Spiel_des_Lebens");
            }
        public static void ProcessUITasks()
            {
                DispatcherFrame frame = new DispatcherFrame();
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                    new DispatcherOperationCallback(delegate (object parameter)
                {
                    frame.Continue = false;
                    return null;
                }), null);
                Dispatcher.PushFrame(frame);
            }     
    }
}
