using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_v1._0
{
    public partial class GameGUI : Form
    {
       // int timer = 0;
        public GameGUI()
        {
           
            InitializeComponent();
        }

        public void updateField()
        {

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl.GetType().ToString() == "System.Windows.Forms.Label")
                {
                    String ctrlname = ctrl.Name;
                    //String[] seperator = new String[] { "label" };
                    // String[] labelID = ctrlname.Split(seperator,StringSplitOptions.RemoveEmptyEntries);
                    if (ctrlname.Length == 7)
                    {
                        int x = Int32.Parse("" + ctrlname[5]);
                        int y = Int32.Parse("" + ctrlname[6]);
                        Label lb = (Label)ctrl;
                        Control.CheckForIllegalCrossThreadCalls = false;
                        if (Game.gamefield[x, y] == 'S')
                        {                                               //update stones
                            lb.Text = "" + Game.gamefield[x, y];
                            lb.BackColor = System.Drawing.Color.Gray;
                        }
                        else if (Game.gamefield[x, y] == 'B')       //update bricks
                        {
                            Brick brick = (Brick)(Game.cells[x, y]);
                            lb.Text = "" + Game.gamefield[x, y] + "\n" + brick.getLife();
                            lb.BackColor = System.Drawing.Color.Brown;
                        }
                        else if (Game.gamefield[x, y] == 'W')  //update water block
                        {
                            lb.Text = "" + Game.gamefield[x, y];
                            lb.BackColor = System.Drawing.Color.Blue;
                        }
                        else if (Game.gamefield[x, y] == 'C')       //update coins
                        {
                            if (((Coin)Game.cells[x, y]).getTime() > 0)
                            {                                               //reduce the lifetime of a coin pack by 1 second
                                lb.Text = "Coins\n" + ((Coin)Game.cells[x, y]).getValue() + "\n" + ((Coin)Game.cells[x, y]).getTime();
                                lb.BackColor = System.Drawing.Color.Purple;
                                ((Coin)Game.cells[x, y]).setTime((((Coin)Game.cells[x, y]).getTime()) - 1000);
                            }
                            else if (((Coin)Game.cells[x, y]).getTime() <= 0)
                            {                                                   //if the life time of a coin pack is over, remove it
                                Game.cells[x, y] = null;
                                Game.gamefield[x, y] = '\0';
                                lb.Text = "" + x + y;
                                lb.BackColor = System.Drawing.Color.White;
                                lb.ForeColor = System.Drawing.Color.Black;
                            }
                        }
                        else if (Game.gamefield[x, y] == 'L')       //update lifepacks
                        {
                            if (((LifePack)Game.cells[x, y]).getTime() > 0)
                            {
                                lb.Text = "LifePack\n" + ((LifePack)Game.cells[x, y]).getTime();
                                lb.BackColor = System.Drawing.Color.Yellow;         //reduce lifepack life time by 1 second
                                ((LifePack)Game.cells[x, y]).setTime((((LifePack)Game.cells[x, y]).getTime()) - 1000);
                            }
                            else if (((LifePack)Game.cells[x, y]).getTime() <= 0)
                            {
                                Game.cells[x, y] = null;                //if the lifetime of life pack is over, remove it
                                Game.gamefield[x, y] = '\0';
                                lb.Text = "" + x + y;
                                lb.BackColor = System.Drawing.Color.White;
                                lb.ForeColor = System.Drawing.Color.Black;
                            }
                            
                            
                        }
                        else
                        {
                            //update normal blocks of cells
                            Game.cells[x, y] = null;
                            Game.gamefield[x, y] = '\0';
                            lb.Font = new Font(lb.Font, FontStyle.Regular);
                            lb.Text = "" + x + y;
                            lb.BackColor = System.Drawing.Color.White;
                            lb.ForeColor = System.Drawing.Color.Black;
                        }


                        if ((x == Game.CurrentxCordinate) & (y == Game.CurrentyCordinate)) //update current player co-ordinates
                        {
                            Game.cells[x, y] = null;
                            Game.gamefield[x, y] = '\0';
                            lb.Font = new Font(lb.Font, FontStyle.Bold);
                            lb.Text = "ME\n" + Game.cDirection;
                            lb.BackColor = System.Drawing.Color.White;
                            lb.ForeColor = System.Drawing.Color.Red;

                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                            {           //update other players
                                if ((x == Game.otherPlayers[i].getX()) & (y == Game.otherPlayers[i].getY()))
                                {
                                    if (Game.otherPlayers[i].getwhetherShot().Equals("1"))
                                    {
                                        //remove the dead player from map
                                        Game.cells[x, y] = null;
                                        Game.gamefield[x, y] = '\0';
                                        lb.Font = new Font(lb.Font, FontStyle.Regular);
                                        lb.Text = "" + x + y;
                                        lb.BackColor = System.Drawing.Color.White;
                                        lb.ForeColor = System.Drawing.Color.Black;
                                    }
                                    else
                                    {
                                        Game.cells[x, y] = null;
                                        Game.gamefield[x, y] = '\0';
                                        lb.Font = new Font(lb.Font, FontStyle.Bold);
                                        lb.BackColor = System.Drawing.Color.White;
                                        lb.ForeColor = System.Drawing.Color.Black;
                                        lb.Text = Game.otherPlayers[i].getid() + "\n" + Game.otherPlayers[i].getcDirection();
                                    }

                                }

                            }

                        }

                    }

                }
            }
        }
        public void updateGUI()
        {
           // timer++;
            Control.CheckForIllegalCrossThreadCalls = false;
            labelPlayerID.Text = Game.playerID;
            labelCoins.Text = Game.coins.ToString();
            labelHealth.Text = Game.health.ToString();
            labelPoints.Text = Game.points.ToString();
            labelShot.Text = Game.whetherShot;
            labelcDirection.Text = Game.cDirection;
            textBoxMessages.Text = Game.message;
            updateField();

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void GameGUI_Load(object sender, EventArgs e)
        {
            /*label104.Text = game.getPID();
            label102.Text = game.getPoints().ToString();
            label107.Text = game.getHealth().ToString();
            label109.Text = game.getWhetherShot();*/

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void GameGUI_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
