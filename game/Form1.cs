using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game
{

    public partial class Form1 : Form
    {
        bool goleft = false;
        bool goright = false;
        bool jumping = false;
        int force = 8;
        int jumpSpeed = 10;
        string facing = "right";
        double playerHealth = 500;
        Random rnd = new Random();
        int score = 0;
        bool end = false;
        int medcount = 0;
        int botcount = 3;
        public Form1()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            
            InitializeComponent();

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (end) return;
            if (e.KeyCode == Keys.A)
            {
                goleft = true;
                facing = "left";
                player.Image = Image.FromFile("vlevo.gif");
            }
            if (e.KeyCode == Keys.D)
            {
                goright = true;
                facing = "right";
                player.Image = Image.FromFile("vpravo.gif");
            }
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
                if (e.KeyCode == Keys.A)
                {
                    player.Image = Image.FromFile("left.png");
                }
                else if (e.KeyCode == Keys.D)
                {
                    player.Image = Image.FromFile("right.png");
                }

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (playerHealth < 1)
            {
                player.Image = Image.FromFile("dead.png");
                player.SizeMode = PictureBoxSizeMode.AutoSize;
                timer1.Stop();
                end = true;
            }
            if (botcount < 5 && botcount == 3)
            {
                makeZombies();
                botcount = 4;
            }
            if (playerHealth < 200 && medcount == 0)
            {
                Med_kit();
                medcount = 1;
            }
            label2.Text = "Kills: " + score.ToString();

            player.Top += jumpSpeed;

            if (jumping && force < 0)
            {
                jumping = false;
            }
            if (goleft)
            {
                player.Left -= 5;
            }
            if (goright)
            {
                player.Left += 5;
            }
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }
            label1.Text = Convert.ToString("Health: "+playerHealth);

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "platform")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && !jumping)
                    {
                        force = 8;
                        player.Top = x.Top - player.Height;
                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                foreach (Control y in this.Controls)
                {
                    if (x is PictureBox && (x.Tag == "bullet" || x.Tag == "enemybullet"))
                    {
                        if (y is PictureBox && y.Tag == "platform")
                        {
                            if (((PictureBox)x).Bounds.IntersectsWith(((PictureBox)y).Bounds))
                            {
                                this.Controls.Remove(((PictureBox)x));
                            }
                        }
                    }
                }

            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Tag == "med")
                {
                    if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(((PictureBox)x));
                        ((PictureBox)x).Dispose();
                        playerHealth += 250;
                        medcount = 0;
                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                foreach (Control y in this.Controls)
                {
                    if (x is PictureBox && x.Tag == "bullet")
                    {
                        if (y is PictureBox && y.Tag == "zombie")
                        {
                            if (((PictureBox)x).Bounds.IntersectsWith(((PictureBox)y).Bounds))
                            {
                                score++;
                                this.Controls.Remove(y);

                                botcount = 3;
                                this.Controls.Remove(((PictureBox)x));
                            }
                        }
                    }
                }
            }
            foreach (Control x in this.Controls)
            {
                foreach (Control y in this.Controls)
                {
                    if (x is PictureBox && x.Tag == "enemybullet")
                    {
                        if (((PictureBox)x).Bounds.IntersectsWith(player.Bounds))
                        {                          
                            this.Controls.Remove(x);
                            playerHealth -= 1;
                        }
                    }
                }
            }
                foreach (Control x in this.Controls)
                {
                   if(x is PictureBox && x.Tag == "zombie")
                   {
                     string direction;
                        if (x.Name == "right")
                         direction = "right";
                     else direction = "left";
                        if (rnd.Next(0, 100) > 1)
                            shoot(direction, "enemy", x.Left, x.Width, x.Top, x.Height);
                   }
                }
        }

        private void Med_kit()
        {
            PictureBox med = new PictureBox();
            med.Size = new Size(20,20);
            med.Image = Image.FromFile("kit.png");
            
                int value = rnd.Next(0, 2);
                if (value == 1)
                {
                    med.Location = new Point(18, 15);
                }
                else if (value == 0)
                {
                    med.Location = new Point(1140, 15);
                }
            
            
            med.BackColor = Color.Red;           
            
            med.Tag = "med";
            this.Controls.Add(med);
            med.BringToFront();
            player.BringToFront();
        }

        private void makeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            int value1 = rnd.Next(0, 5);
            if (value1 == 0)
            {
                zombie.Location = new Point(1030, 60);
                zombie.Image = Image.FromFile("bot_left.png");
                zombie.Name = "left";
            }
            else if (value1 == 1)
            {
                zombie.Location = new Point(50, 62);
                zombie.Image = Image.FromFile("bot_right.png");
                zombie.Name = "right";

            }
            else if (value1 == 2)
            {
                zombie.Location = new Point(350, 145);
                zombie.Image = Image.FromFile("bot_right.png");
                zombie.Name = "right";
            }
            else if (value1 == 3)
            {
                zombie.Location = new Point(750, 140);
                zombie.Image = Image.FromFile("bot_left.png");
                zombie.Name = "left";
            }
            else if (value1 == 4)
            {
                zombie.Location = new Point(700, 258);
                zombie.Image = Image.FromFile("bot_left.png");
                zombie.Name = "left";
            }
            
            
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(zombie);
            player.BringToFront();
        }


        private void shoot(string direct,string shooter,int left,int width,int top,int height)
        {
            bullet shoot = new bullet();
            shoot.direction = direct;
            shoot.bulletLeft = left + (width / 2);
            shoot.bulletTop = top + (height / 2);
            if (shooter == "player")
                shoot.teg = "bullet";
            else shoot.teg = "enemybullet";
            shoot.mkBullet(this);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (end) return;

            if (e.KeyCode == Keys.A)
            {
                goleft = false;
                player.Image = Image.FromFile("stand_left.png");
            }
            if (e.KeyCode == Keys.D)
            {
                goright = false;
                player.Image = Image.FromFile("stand.png");
            }
            if (jumping)
            {
                jumping = false;
            }
            if(e.KeyCode == Keys.Y)
            {
                shoot(facing,"player",player.Left,player.Width,player.Top,player.Height);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    class bullet
    {
        public string direction;
        public int speed = 20;
        PictureBox Bullet = new PictureBox();
        Timer tm = new Timer();
        public string teg;
        public int bulletLeft;
        public int bulletTop;

        public void mkBullet(Form form)
        {
            Bullet.BackColor = System.Drawing.Color.Black;
            Bullet.Size = new Size(5, 5);
            Bullet.Tag = teg;
            Bullet.Left = bulletLeft;
            Bullet.Top = bulletTop;
            Bullet.BringToFront();
            form.Controls.Add(Bullet);

            tm.Interval = speed;
            tm.Tick += new EventHandler(tm_Tick);
            tm.Start();
        }
        public void tm_Tick(object sender, EventArgs e)
        {
            
            if (direction == "left")
            {
                Bullet.Left -= speed;
            }
            
            if (direction == "right")
            {
                Bullet.Left += speed;
            }

            if (Bullet.Left < 25 || Bullet.Left > 1300 || Bullet.Top < 20 || Bullet.Top > 800)
            {
                tm.Stop();
                tm.Dispose();
                Bullet.Dispose();
                tm = null;
                Bullet = null;
            }
        }
    }
}
        

     
            

        

        

