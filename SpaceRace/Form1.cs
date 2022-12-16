using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        Rectangle player1 = new Rectangle(130, 280, 20, 20);
        Rectangle player2 = new Rectangle(400, 280, 20, 20);
        List<Rectangle> obstacle = new List<Rectangle>();
        List<int> obstacleSpeeds = new List<int>();


        SolidBrush goldBrush = new SolidBrush(Color.Goldenrod);
        SolidBrush whiteBrush = new SolidBrush(Color.White);


        int player1Score = 0;
        int player2Score = 0;
        int player1Speed = 4;
        int player2Speed = 4;
        int speed;

        int obstacleSize = 10;
        int obstacleSpeed = 7;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;



        public Form1()
        {
            InitializeComponent();
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
            }
            }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Draw players
            e.Graphics.FillRectangle(goldBrush, player1);
            e.Graphics.FillRectangle(goldBrush, player2);

            //Draw obstacles
            for (int i = 0; i < obstacle.Count(); i++)
            {
                e.Graphics.FillEllipse(whiteBrush, obstacle[i]);

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

                player2.Y = 280;
            }
            else if (player1.Y < 5)
            {
                player1Score++;
                p1ScoreLabel.Text = $"{player1Score}";

                player1.Y = 280;
            }

            //move obstacles 
            for (int i = 0; i < obstacle.Count; i++)
            {
                int y = obstacle[i].Y + obstacleSpeeds[i];
                obstacle[i] = new Rectangle(obstacle[i].X, y, obstacleSize, obstacleSize);
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

            //check for collision between players and obstacles
            for (int i = 0; i < obstacle.Count; i++)
            {
                if (player1.IntersectsWith(obstacle[i]))
                {
                    player1.Y = 280;
                }
                else if (player2.IntersectsWith(obstacle[i]))
                {
                    player2.Y = 280;
                }

                obstacle.RemoveAt(i);
                obstacleSpeeds.RemoveAt(i);

                
            }

            Refresh();
        }
    }
}
