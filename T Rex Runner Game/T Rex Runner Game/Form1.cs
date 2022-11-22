using System;
using System.Media;
using System.Windows.Forms;

namespace T_Rex_Runner_Game
{
    public partial class Form1 : Form
    {
        bool jumping = false; // boolean to check if player is jumping or not
        int jumpSpeed; // integer to set jump speed
        int force = 12; // force of the jump in an integer
        int score = 0; // default score integer set to 0
        int obstacleSpeed = 10; // the default speed for the obstacles
        Random rand = new Random(); // create a new random class
        bool isGameOver = false; // boolean to check is game over
        int topPosition; // integer to set trex initial top position
        SoundPlayer scoreSound; // class for score sounds
        SoundPlayer gameOverSound; // class for game over sound
        public Form1()
        {
            InitializeComponent();
            topPosition = floor.Top - trex.Height; // set trex initial top position
            scoreSound = new SoundPlayer(T_Rex_Runner_Game.Properties.Resources.score);
            gameOverSound = new SoundPlayer(T_Rex_Runner_Game.Properties.Resources.game_over);

            resetGame(); // run the reset game function
        }

        private void gameEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed; // move trex upper when jumping

            scoreText.Text = "Score: " + score;

            // reset jumping
            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            // gradually decrease jump force spead
            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            // reset jump and position params
            if (trex.Top > topPosition - 1 && jumping == false)
            {
                force = 12;
                trex.Top = topPosition;
                jumpSpeed = 0;
            }


            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;

                    // set new position for disapeared obstacle
                    if (x.Left < -100)
                    {
                        x.Left = this.ClientSize.Width + rand.Next(300, 500) + (x.Width * 15);
                        score++;
                        scoreSound.Play();
                    }

                    // make game over when intersect with obstacle
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        scoreText.Text += " Press R to restart the game!";
                        isGameOver = true;
                        gameOverSound.Play();
                    }
                }
            }

            // gradually increase game difficulty with increasing score
            if (score > 5)
            {
                obstacleSpeed = 12;
            } else if (score > 10)
            {
                obstacleSpeed = 15;
            }

        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // if the R key is pressed and released then we run the reset function
            if (e.KeyCode == Keys.R && isGameOver == true)
            {
                resetGame();
            }

            //when the keys are released we check if jumping is true
            // if so we need to set it back to false so the player can jump again
            if (jumping)
            {
                jumping = false;
            }
        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //if the player pressed the space key and jumping boolean is false
            // then we set jumping to true
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        public void resetGame()
        {
            // This is the reset function
            force = 12; // set the force to 12
            jumpSpeed = 0; // set the jump speed to 0
            jumping = false; // change jumping to false
            score = 0; // set score to 0
            obstacleSpeed = 10; // set obstacle speed back to 10
            scoreText.Text = "Score: " + score; // change the score text to just show the score
            trex.Image = Properties.Resources.running; // change the t rex image to running
            isGameOver = false;
            trex.Top = topPosition;

            foreach (Control x in this.Controls)
            {
                // is X is a picture box and it has a tag of obstacle
                if (x is PictureBox && x.Tag == "obstacle")
                {
                    // generate a random number in the position integer between 600 and 1000
                    // change the obstacles left position to a random location begining of the game
                    x.Left = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);
                }
            }

            gameTimer.Start(); // start the timer
        }
    }
}
