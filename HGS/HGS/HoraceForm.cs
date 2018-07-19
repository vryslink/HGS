using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HGS
{
    public partial class Form1 : Form
    {
        Graphics g;
        Scene s;
        Font font;
        Bitmap picture;                 //scene picture
        possibleKeys previousKey;       //for continous movement on the road
        string scoreTableText;
        bool startGame = false;
        bool menuAlreadyPainted = false;
        int cashPlus;                   //for every 1000 points + 10$
        PrivateFontCollection pfc;

        public Form1()
        {
            InitializeComponent();

            /*GRAPHICS*/
            picture = new Bitmap(ClientSize.Width, ClientSize.Height);
            pfc = new PrivateFontCollection();
            pfc.AddFontFile("prstart.ttf ");
            font = new Font(pfc.Families[0], 16, FontStyle.Bold, GraphicsUnit.Pixel);
            g = this.CreateGraphics();

            /*LABELS*/
            Controls.Add(Lscoretable);
            LpresentedByText.Font = font;
            LpressEnter.Font = font;
            Lscoretable.Font = font;

            /*MAIN CLASS*/
            s = new Scene(ClientSize.Width, ClientSize.Height);



        }

        /*LEADING TIMER*/
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            switch (s.state)
            {
                case possibleState.road:

                    /*LABELS & TIMER*/
                    scoreTableText = String.Format("{3,1}♥  CASH {0,3}$  SCORE {1,4} HI {2,4}", s.cash, s.score, s.hi, s.life);
                    Lscoretable.Text = scoreTableText;
                    Lscoretable.BackColor = Color.Transparent;
                    Lscoretable.Visible = true;
                    LpresentedByText.Visible = false;
                    LpressEnter.Visible = false;
                    if (spawningTimer.Enabled == false) spawningTimer.Enabled = true; //spawning timer management for state transfering

                    /*LOGIC*/
                    if (s.roadHoraceHit) //hit by car
                    {
                        spawningTimer.Enabled = false;
                        previousKey = possibleKeys.none; //after hit dont move
                        keyPressed = possibleKeys.none;


                        s.moveWithAmbulanceForHorace(ClientSize.Width, ClientSize.Height);
                        s.drawScreen(g, font, picture, ClientSize.Width, ClientSize.Height);
                        s.roadHorace.imagePath = "roadHoraceD1.png"; //starting image is without skis

                    }
                    else //normal
                    {  
                        if (s.cash < 0) //out of cash
                        {
                            scoreTableText = String.Format("{3,1}♥  CASH {0,3}$  SCORE {1,4} HI {2,4}\n\nGAME OVER            ", s.cash, s.score, s.hi, s.life);
                            Lscoretable.Text = scoreTableText;

                            s.state = possibleState.gameOver;
                        }
                        else //normal
                        {
                            /*GAME CONTROL*/
                            s.moveRoadHorace(keyPressed, ClientSize.Width, ClientSize.Height);
                            s.moveWithCars(ClientSize.Width, ClientSize.Height);
                            s.detectCarCollisions();
                            s.detectRoadHoraceCollisions(ClientSize.Width, ClientSize.Height);
                            s.drawScreen(g, font, picture, ClientSize.Width, ClientSize.Height);
                        }
                    }

                    if (s.roadHorace.moveToSlope) //success, move from the road to the slope
                    {
                        previousKey = possibleKeys.none; //after hit dont move
                        keyPressed = possibleKeys.none;

                        s.roadHorace.moveToSlope = false;
                        s.state = possibleState.skiing;
                        finishTimer.Enabled = true;
                    }
                    break;
                case possibleState.skiing:

                    /*LABELS & TIMER*/
                    if (spawningTimer.Enabled == false) spawningTimer.Enabled = true; //spawning timer management for state transfering
                    LpresentedByText.Visible = false;
                    LpressEnter.Visible = false;

                    /*LOGIC*/
                    if (s.life <= 0) //out of life
                    {
                        scoreTableText = String.Format("{3,1}♥  CASH {0,3}$  SCORE {1,4} HI {2,4}\n\nGAME OVER            ", s.cash, s.score, s.hi, s.life);
                        Lscoretable.Text = scoreTableText;

                        s.state = possibleState.gameOver;
                    }
                    else //normal
                    {
                        if (s.score % cashPlus == 0) //check for the n*1000th point
                        {
                            s.cash += 10;
                            cashPlus += 1000;
                        }

                        if (s.finishFlag) //finish flag was created
                        {
                            if (s.finish.y < 30) //finish flag is already under the score table - do not move
                            {
                                System.Threading.Thread.Sleep(3000); //wait for a second and then continue
                                if (s.cash > 0)                      //safe check, not really necesarry
                                {
                                    s.roadHorace.x = (ClientSize.Width / 2) - 80;
                                    s.roadHorace.rectangle.X = (ClientSize.Width / 2) - 80;
                                    s.roadHorace.y = ClientSize.Height - 40;
                                    s.roadHorace.rectangle.Y = ClientSize.Height - 40;

                                    previousKey = possibleKeys.none; //dont move on the road in the beginning
                                    keyPressed = possibleKeys.none;

                                    s.state = possibleState.road;
                                    s.finishFlag = false;           //start a new slope  
                                    spawningTimer.Enabled = true;   //create moving objects again
                                }
                            }
                        }

                        /*GAME CONTROL*/
                        s.moveHorace(keyPressed);
                        s.moveWithObjects();
                        s.detectHoraceCollisions();
                        s.drawScreen(g, font, picture, ClientSize.Width, ClientSize.Height);
                        s.checkMissedFlags();

                        /*LABELS*/
                        scoreTableText = String.Format("{3,1}♥  CASH {0,3}$  SCORE {1,4} HI {2,4}", s.cash, s.score, s.hi, s.life);
                        Lscoretable.Text = scoreTableText;
                        Lscoretable.Font = font;
                        Lscoretable.BackColor = s.color;
                    }
                    break;
                case possibleState.gameOver:

                    if (s.score > s.hi) System.IO.File.WriteAllText("high_score.txt", s.score.ToString()); //high score write
                    System.Threading.Thread.Sleep(2000);

                    s.state = possibleState.notStarted;
                    break;
                case possibleState.notStarted:
                    
                    spawningTimer.Enabled = false;
                    cashPlus = 1000;
                    
                    string text = System.IO.File.ReadAllText("high_score.txt"); //load high-score
                    s.hi = Int32.Parse(text);

                    if (!menuAlreadyPainted) //just draw the menu once
                    {
                        menuAlreadyPainted = true;
                        g.Clear(Color.FromArgb(196, 198, 196));
                        g.DrawImage(Bitmap.FromFile("theme.png"), 0, 40); //theme picture
                    }

                    /*LABELS & TIMER*/
                    LpresentedByText.Visible = true;
                    LpressEnter.Visible = true;
                    Lscoretable.Visible = false;
                    finishTimer.Enabled = false; //dont count for finish flags

                    /*LOGIC*/
                    if (!startGame) //if enter was not pressed 
                    {
                        /*TEXT ANIMATION*/
                        if (LpressEnter.ForeColor == Color.Black) 
                        {
                            System.Threading.Thread.Sleep(500);
                            LpressEnter.ForeColor = Color.Gray;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(500);
                            LpressEnter.ForeColor = Color.Black;
                        }
                    }
                    else //enter was pressed
                    {
                        /*FLAGS*/
                        s.cash = 30;
                        s.score = 100;
                        s.life = 3;
                        spawningTimer.Enabled = true;
                        s.roadHorace.haveSkis = false;
                        s.state = possibleState.road; //switch to road
                        startGame = false;            //back to previous value
                        menuAlreadyPainted = false;

                        /*LABELS*/
                        LpressEnter.Visible = false; 
                        LpresentedByText.Visible = false;
                    }
                    break;
                case possibleState.finishing:
                    /*ON FINISH TIMER TICK*/
                    spawningTimer.Enabled = false;  //do not create any other moving objects
                    s.state = possibleState.skiing; //move to skiing state
                    break;
                default:
                    break;
            }
        }

        /*TIMER FOR CREATING OBJECTS*/
        private void spawningTimer_Tick(object sender, EventArgs e)
        {
            if (s.state == possibleState.skiing && !s.finishFlag) s.createObject();
            else if (s.state == possibleState.road) s.createCars(ClientSize.Width, ClientSize.Height);
            
        }

        /*TIMER FOR CREATING FINISH LINE*/
        private void finishTimer_Tick(object sender, EventArgs e)
        {
            s.state = possibleState.finishing; //time for finish flag
            s.finishFlag = true; //signal for skiing state
            s.createFinish(); 
            finishTimer.Enabled = false; //do not tick
        }

        /*LOGIC FOR KEYBOARD*/
        possibleKeys keyPressed = possibleKeys.none;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                keyPressed = possibleKeys.up;
                previousKey = possibleKeys.up;
                return true;
            }
            else if (keyData == Keys.Down)
            {
                keyPressed = possibleKeys.down;
                previousKey = possibleKeys.down;
                return true;
            }
            else if (keyData == Keys.Right)
            {
                keyPressed = possibleKeys.right;
                previousKey = possibleKeys.right;
                return true;
            }
            else if (keyData == Keys.Left)
            {
                keyPressed = possibleKeys.left;
                previousKey = possibleKeys.left;
                return true;
            }
            else if ((keyData == Keys.Enter) && (s.state == possibleState.notStarted))
            {
                startGame = true;
                return true;
            }
            else if (s.state == possibleState.road)
            {
                keyPressed = previousKey;
                return true;
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (s.state != possibleState.road) keyPressed = possibleKeys.none; //on road continous movement
        }


    }
    }

 