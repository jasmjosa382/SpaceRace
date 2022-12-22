using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Space Racing Game
//December 2022
//Jasmine Josan
namespace SpaceRace
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle(130, 310, 20, 20);
        Rectangle player2 = new Rectangle(400, 310, 20, 20);
        List<Rectangle> obstacle = new List<Rectangle>();
        List<Rectangle> obstacle2 = new List<Rectangle>();
        List<int> obstacleSpeeds = new List<int>();
        List<int> obstacle2Speeds = new List<int>();


        SolidBrush goldBrush = new SolidBrush(Color.Goldenrod);
        SolidBrush whiteBrush = new SolidBrush(Color.White);


        int player1Score = 0;
        int player2Score = 0;
        int player1Speed = 4;
        int player2Speed = 4;
        int speed;

        int obstacleSize =5;
        int obstacleSpeed = 2;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        Random randGen = new Random();
        int randValue = 0;

        string gameState = "waiting";

        public Form1()
        {
            InitializeComponent();
        }

        public void GameSetup()
        {
            gameState = "running";

            titleLabel.Text = "";
            subtitleLabel.Text = "";

            gameTimer.Enabled = true;
            player1Score = 0;
            player2Score = 0;

            player1.Y = 310;
            player2.Y = 310;

            obstacle.Clear();
            obstacleSpeeds.Clear();
            obstacle2.Clear();
            obstacle2Speeds.Clear();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        SoundPlayer start = new SoundPlayer(Properties.Resources.start);
                        start.Play();

                        GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        this.Close();
                    }
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Space Race Game";
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }
            else if (gameState == "running")
            {
                winLabel.Visible = false;

                //update labels
                p1ScoreLabel.Text = $"{player1Score}";
                p2ScoreLabel.Text = $"{player2Score}";

                //Draw players
                e.Graphics.FillRectangle(goldBrush, player1);
                e.Graphics.FillRectangle(goldBrush, player2);

                //Draw obstacles
                for (int i = 0; i < obstacle.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, obstacle[i]);

                }

                for (int i = 0; i < obstacle2.Count(); i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, obstacle2[i]);

                }

            }
            else if (gameState == "over")
            {
                p1ScoreLabel.Text = "";
                p2ScoreLabel.Text = "";

                titleLabel.Text = "Game Over";
                subtitleLabel.Text = "Press Space to Start or Esc to Exit";
            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player 1
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
            }

            //move player 2
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= player2Speed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
            }

            //Player makes it to the other side
            if (player2.Y < 5)
            {
                player2Score++;
                p2ScoreLabel.Text = $"{player2Score}";

                player2.Y = 310;
                SoundPlayer point = new SoundPlayer(Properties.Resources.point);
                point.Play();
            }
            else if (player1.Y < 5)
            {
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";

                player1.Y = 310;
                SoundPlayer point = new SoundPlayer(Properties.Resources.point);
                point.Play();
            }

            //move obstacles 
            for (int i = 0; i < obstacle.Count; i++)
            {
                int x = obstacle[i].X + obstacleSpeeds[i];
                obstacle[i] = new Rectangle(x, obstacle[i].Y, obstacleSize, obstacleSize);
            }

            //move obstacles on right side
            for (int i = 0; i < obstacle2.Count; i++)
            {
                int x = obstacle2[i].X + obstacle2Speeds[i];
                obstacle2[i] = new Rectangle(x, obstacle2[i].Y, obstacleSize, obstacleSize);
            }

            //generate a random value
            randValue = randGen.Next(1, 101);

            //generate new ball if it is time
            if (randValue < 3)
            {
                obstacle.Add(new Rectangle(0, randGen.Next(0, this.Height - 80), obstacleSize, obstacleSize));
                obstacleSpeeds.Add(10);
            }
            else if (randValue < 8)
            {
                obstacle.Add(new Rectangle(0, randGen.Next(0, this.Height - 80), obstacleSize, obstacleSize));
                obstacleSpeeds.Add(14);
            }
            else if (randValue < 28)
            {
                obstacle2.Add(new Rectangle(600, randGen.Next(0, this.Height - 80), obstacleSize, obstacleSize));
                obstacle2Speeds.Add(-16);
            }
            else if (randValue < 28)
            {
                obstacle2.Add(new Rectangle(600, randGen.Next(0, this.Height - 80), obstacleSize, obstacleSize));
                obstacle2Speeds.Add(-12);
            }

            //remove obstacle if it goes off screen
            for (int i = 0; i < obstacle.Count; i++)
            {
                if (obstacle[i].Y >= this.Height)
                {
                    obstacle.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                }
            }

            for (int i = 0; i < obstacle2.Count; i++)
            {
                if (obstacle2[i].Y >= this.Height)
                {
                    obstacle2.RemoveAt(i);
                    obstacle2Speeds.RemoveAt(i);
                }
            }

            //check for collision between player and obstacles
            for (int i = 0; i < obstacle.Count; i++)
            {
                if (player1.IntersectsWith(obstacle[i]))
                {
                    SoundPlayer hit = new SoundPlayer(Properties.Resources.hit);
                    hit.Play();

                    player1.Y = 310;
                    obstacle.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                }
                else if (player2.IntersectsWith(obstacle[i]))
                {
                    SoundPlayer hit = new SoundPlayer(Properties.Resources.hit);
                    hit.Play();

                    player2.Y = 310;
                    obstacle.RemoveAt(i);
                    obstacleSpeeds.RemoveAt(i);
                }
            }

            for (int i = 0; i < obstacle2.Count; i++)
            {
                if (player1.IntersectsWith(obstacle2[i]))
                {
                    SoundPlayer hit = new SoundPlayer(Properties.Resources.hit);
                    hit.Play();

                    player1.Y = 310;
                    obstacle2.RemoveAt(i);
                    obstacle2Speeds.RemoveAt(i);
                }
                else if (player2.IntersectsWith(obstacle2[i]))
                {
                    SoundPlayer hit = new SoundPlayer(Properties.Resources.hit);
                    hit.Play();

                    player2.Y = 310;
                    obstacle2.RemoveAt(i);
                    obstacle2Speeds.RemoveAt(i);
                }
            }

            //end game if either players scores equal 3
            if (player1Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 1 Wins!!";
                gameState = "over";
                SoundPlayer winner = new SoundPlayer(Properties.Resources.winner);
                winner.Play();
            }
            else if (player2Score == 3)
            {
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player 2 Wins!!";
                gameState = "over";
                SoundPlayer winner = new SoundPlayer(Properties.Resources.winner);
                winner.Play();
            }

            Refresh();
        }
    }
}
