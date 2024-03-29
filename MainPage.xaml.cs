﻿using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lab6Starter;
/**
 * 
 * Name: Maximilian Patterson and Jonathan Renier-Wigg
 * Date: 11/1/2022
 * Description: Working with github and implementing ResetGame()
 * Bugs: N/A
 * Reflection: We enjoyed this lab. I think the Git related labs are good because it's something everyone needs to know.
 * 
 */



/// <summary>
/// The MainPage, this is a 1-screen app
/// </summary>
public partial class MainPage : ContentPage
{
    TicTacToeGame ticTacToe; // model class
    Button[,] grid;          // stores the buttons
    Stopwatch stopwatch; // Stopwatch for game timer
    ObservableCollection<GameResult> GameList = new ObservableCollection<GameResult>();

    /// <summary>
    /// initializes the component
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
        ticTacToe = new TicTacToeGame();
        grid = new Button[TicTacToeGame.GRID_SIZE, TicTacToeGame.GRID_SIZE] { { Tile00, Tile01, Tile02 }, { Tile10, Tile11, Tile12 }, { Tile20, Tile21, Tile22 } };
        GameListLV.ItemsSource = GameList;
        StartStopwatch();
    }

    private void StartStopwatch()
    {
        // Start new Stopwatch bound to StopwatchLBL
        stopwatch = new Stopwatch();
        stopwatch.Start();
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            StopwatchLBL.Text = stopwatch.Elapsed.ToString(@"mm\:ss");
            return true;
        });
    }

    /// <summary>
    /// Handles button clicks - changes the button to an X or O (depending on whose turn it is)
    /// Checks to see if there is a victory - if so, invoke 
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleButtonClick(object sender, EventArgs e)
    {
        Player victor;
        Player currentPlayer = ticTacToe.CurrentPlayer;

        Button button = (Button)sender;
        int row;
        int col;

        FindTappedButtonRowCol(button, out row, out col);
        if (button.Text.ToString() != "")
        { // if the button has an X or O, bail
            DisplayAlert("Illegal move", "Fill this in with something more meaningful", "OK");
            return;
        }
        button.Text = currentPlayer.ToString();
        Boolean gameOver = ticTacToe.ProcessTurn(row, col, out victor);

        if (gameOver)
        {
            CelebrateVictory(victor);
            GameList.Add(new GameResult(victor, stopwatch.Elapsed));
            stopwatch.Stop();
        }
    }

    // Class that represents a game result, rendered in the list view
    private class GameResult
    {
        public String Winner { get; set; }
        public String Duration { get; set; }

        public GameResult(Player winner, TimeSpan duration)
        {
            Winner = winner.ToString();
            Duration = duration.ToString(@"mm\:ss");
        }
    }

    /// <summary>
    /// Returns the row and col of the clicked row
    /// There used to be an easier way to do this ...
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void FindTappedButtonRowCol(Button button, out int row, out int col)
    {
        row = -1;
        col = -1;

        for (int r = 0; r < TicTacToeGame.GRID_SIZE; r++)
        {
            for (int c = 0; c < TicTacToeGame.GRID_SIZE; c++)
            {
                if (button == grid[r, c])
                {
                    row = r;
                    col = c;
                    return;
                }
            }
        }

    }


    /// <summary>
    /// Celebrates victory, displaying a message box and resetting the game
    /// </summary>
    private async void CelebrateVictory(Player victor)
    {
        //MessageBox.Show(Application.Current.MainWindow, String.Format("Congratulations, {0}, you're the big winner today", victor.ToString()));
        XScoreLBL.Text = String.Format("X's Score: {0}", ticTacToe.XScore);
        OScoreLBL.Text = String.Format("O's Score: {0}", ticTacToe.OScore);

        await DisplayAlert(String.Format("Congratulations, {0}", victor.ToString()),
            String.Format("you're the big winner today\n{0}\n{1}", XScoreLBL.Text, OScoreLBL.Text), "Next Game");

        ResetGame();
    }

    /// <summary>
    /// Resets the grid buttons so their content is all ""
    /// </summary>
    private void ResetGame()
    {
        for (int row = 0; row < TicTacToeGame.GRID_SIZE; row++)
        {
            for (int col = 0; col < TicTacToeGame.GRID_SIZE; col++)
            {
                grid[row, col].Text = ""; // Reset current button text
            }
        }

        StartStopwatch();
        ticTacToe.ResetGame();
    }

}



