using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Magic;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using WorkWork.Bot;
using WorkWork.Memory;
using WorkWork.Profiles;
using WorkWork.Settings;

namespace WorkWork
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        private bool profileLoaded = false;
        private bool attached = false;
        private bool spellsLoaded = false;
        private string lastProfile = null;
        private string lastSpells = null;
        private BlackMagic magic;
        private Settings settings = new Settings();
        private Profile profile;
        private Spells spells;
        private Main bot;
        private CreateProfile newProfile;
        private Spells newSpells = new Spells();
        private SetKeys keys;
        private bool wpoint = false, gpoint = false, spoint = false;
        private int start = 0;
        private Thread profileThread;
        private Thread botThread;
        private int channel = 0;
        public MainForm()
        {

            InitializeComponent();
            initialize();
            this.Opacity = 0.99;
        }
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();
            Application.Run(mainForm);

        }
        private IntPtr handle;
        public void initialize()
        {

            try
            {
                settings.Load();
                if ((lastProfile = settings.Profile) != null)
                {
                    profile = new Profile();
                    profile.Load(settings.Profile);
                    profileLoaded = true;
                    
                    profileLabel2.Text = Path.GetFileName(settings.Profile);
                }
                if ((lastSpells = settings.Spells) != null)
                {
                    spells = new Spells();
                    spells.Load(lastSpells);
                    spellsLoaded = true;
                    spellsLabel2.Text = Path.GetFileName(settings.Spells);

                }
                if (settings.IgnoreMobs)
                {
                    mobsBox.Checked = true;
                }
                if (settings.IgnorePlayers)
                {
                    playersBox.Checked = true;
                }
                if (settings.Skinning)
                {
                    skinningBox.Checked = true;
                }
                if (settings.Looting)
                {
                    lootBox1.Checked = true;
                }
                if (settings.Linear)
                {
                    linearBox.Checked = true;
                }
                if (settings.Sell)
                {
                    sellingBox.Checked = true;
                }
                if (settings.Mining)
                {
                    gatheringBox2.Checked = true;
                }
                if (settings.Herbing)
                {
                    gatheringBox1.Checked = true;
                }
                sellTextBox.Text = settings.SellLoops+"";
                mountBox.Text = settings.GeneralKeybinds[0, 1];
                forwardBox.Text = settings.GeneralKeybinds[1, 1];
                leftBox.Text = settings.GeneralKeybinds[2, 1];
                rightBox.Text = settings.GeneralKeybinds[3, 1];
                downBox.Text = settings.GeneralKeybinds[4, 1];
                upBox.Text = settings.GeneralKeybinds[5, 1];
                tabBox.Text = settings.GeneralKeybinds[6, 1];
                lootBox.Text = settings.GeneralKeybinds[7, 1];
                drinkBox.Text = settings.GeneralKeybinds[8, 1];
                eatBox.Text = settings.GeneralKeybinds[9, 1];
                escBox.Text = settings.GeneralKeybinds[10, 1];
                retrieveBox.Text = settings.GeneralKeybinds[12, 1];
                releaseBox.Text = settings.GeneralKeybinds[11, 1];
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
            }
            catch (Exception ex)
            {
                attachLabel.Text = "Loading settings failed.";
                errorLabel.Text = ex.Message;
            }
            startButton.Hide();
            stopButton.Hide();
            keys = new SetKeys(mountBox, 0, settings); //Check this.
            panel1.Show();
            panel2.Hide();
            panel3.Hide();
            loadButton2.Show();
            loadButton1.Show();
            attachButton.Show();
            newSpellLabel.Hide();
            addSpellLabel.Hide();
            saveLabel2.Hide();
            wstartLabel.Hide();
            gstartLabel.Hide();
            sstartLabel.Hide();
            mpointLabel.Hide();
            sstopLabel.Hide();
            newProfileLabel.Hide();
            saveLabel.Hide();
            menuButton2.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            menuButton1.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
            menuButton3.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            menuButton3.Hide();
            openFile.Hide();
            sendMsgBox.Hide();
            sendMsgButton.Hide();
            sayLabel.Hide();
            whisperLabel.Hide();
            replyLabel.Hide();
            partyLabel.Hide();
            guildLabel.Hide();
            /*magic = new BlackMagic();
            magic.OpenProcessAndThread(SProcess.GetProcessFromProcessName("WoW"));
                testLabel.Text = magic.ReadShort((uint)TbcOffsets.General.Cursor)+"";*/
        }
        private void button1_Click(object sender, EventArgs e)
        {
            magic = new BlackMagic();
            if (magic.OpenProcessAndThread(SProcess.GetProcessFromProcessName("WoW")))
            {
                handle = FindWindow(null,"World of Warcraft");
                attachLabel.Text = "Attached to WoW";
                if (profileLoaded && spellsLoaded)
                {
                    startButton.Show();
                    start = 3;
                }
                attached = true;
                menuButton3.Show();
                newProfile = new CreateProfile(magic, pointsTextBox);
                // //check this
                profileThread = new Thread(newProfile.DoWork);
                profileThread.Start();
                sendMsgBox.Show();
                sendMsgButton.Show();
                sayLabel.Show();
                whisperLabel.Show();
                replyLabel.Show();
                partyLabel.Show();
                guildLabel.Show();
                //testLabel.Text = magic.ReadShort((uint)TbcOffsets.General.Cursor)+"";
            }
            else
            {
                attachLabel.Text = "World of Warcraft does not appear to be running";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }





        private void stopButton_Click(object sender, EventArgs e)
        {
            bot.Halt();
            attachLabel.Text = "Stopped";
            start = 2;
            startButton.Show();
            stopButton.Hide();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (profileLoaded && spellsLoaded)
                {
                    
                    bot = new Main(profile, spells, settings, magic, mobs, died, eat, drink, loopsLabel);
                    botThread = new Thread(bot.DoWork);
                    botThread.Start();
                    attachLabel.Text = "Started";
                    stopButton.Show();
                    startButton.Hide();
                    start = 1;
                }
                else
                {
                    attachLabel.Text = "Profile and spells not loaded";
                }
            }
            catch (Exception ex)
            {
                attachLabel.Text = "Error starting bot";
                errorLabel.Text = ex.Message;
            }
        }

        private void skinningBox_CheckedChanged(object sender, EventArgs e)
        {
            if (skinningBox.Checked)
            {
                settings.Skinning = true;
            }
            else
            {
                settings.Skinning = false;
            }
        }

        private void mobsBox_CheckedChanged(object sender, EventArgs e)
        {
            if (mobsBox.Checked)
            {
                settings.IgnoreMobs = true;
            }
            else
            {
                settings.IgnoreMobs = false;
            }
        }

        private void playersBox_CheckedChanged(object sender, EventArgs e)
        {
            if (playersBox.Checked)
            {
                settings.IgnorePlayers = true;
            }
            else
            {
                settings.IgnorePlayers = false;
            }
        }

        private void loadProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Profile files (.profile)|*.profile|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                try
                {
                    profile = new Profile();
                    profile.Load(filename);
                    profileLoaded = true;
                    lastProfile = filename;
                    profileLabel2.Text = Path.GetFileName(filename);
                    settings.Profile = lastProfile;
                    settings.Save();
                }
                catch (Exception ex)
                {
                    profileLabel2.Text = "Error loading file";
                    errorLabel.Text = ex.Message;
                }
            }
            if (profileLoaded && spellsLoaded && attached)
            {
                startButton.Show();
            }
        }

        private void loadSpells_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Spells' profile files (.spell)|*.spell|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Multiselect = false;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                try
                {
                    spells = new Spells();
                    spells.Load(filename);
                    spellsLoaded = true;
                    lastSpells = filename;
                    spellsLabel2.Text = Path.GetFileName(filename);
                    settings.Spells = lastSpells;
                    settings.Save();
                }
                catch (Exception ex)
                {
                    spellsLabel2.Text = "Error loading file";
                    errorLabel.Text = ex.Message;
                }
            }
            if (profileLoaded && spellsLoaded && attached)
            {
                startButton.Show();
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            // When the application is exiting, write the application data to the 
            // user file and close it.





        }

        private void ZBox_CheckedChanged(object sender, EventArgs e)
        {
            if (zBox.Checked)
            {
                newProfile.IgnoreZ = true;
            }
            else
            {
                newProfile.IgnoreZ = false;
            }
        }

        private void loopBox_CheckedChanged(object sender, EventArgs e)
        {
            if (loopBox.Checked)
            {
                newProfile.Loop = true;
            }
            else
            {
                newProfile.Loop = false;
            }
        }

        private void MButton_Click(object sender, EventArgs e)
        {
            newProfile.AddMountPoint();
        }

        private void SButton_Click(object sender, EventArgs e)
        {
            if (spoint)
            {
                sstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
                spoint = false;
            }
            else if (!spoint && !wpoint && !gpoint)
            {
                sstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
                spoint = true;
            }
            newProfile.SetMode(2);
        }

        private void GButton_Click(object sender, EventArgs e)
        {
            if (gpoint)
            {
                gstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
                gpoint = false;
            }
            else if (!spoint && !wpoint && !gpoint)
            {
                gstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
                gpoint = true;
            }
            newProfile.SetMode(1);
        }

        private void WButton_Click(object sender, EventArgs e)
        {
            if (wpoint)
            {
                wstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
                wpoint = false;
            }
            else if (!spoint && !wpoint && !gpoint)
            {
                wstartLabel.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
                wpoint = true;
            }
            newProfile.SetMode(0);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            zBox.Checked = false;
            loopBox.Checked = false;
            pointsTextBox.Clear();
            newProfile.Halt();
            newProfile = new CreateProfile(magic, pointsTextBox);
            profileThread = new Thread(newProfile.DoWork);
            profileThread.Start();
            

        }

        private void newSpellsButton_Click(object sender, EventArgs e)
        {
            newSpells = new Spells();
        }

        private void saveButton2_Click(object sender, EventArgs e)
        {
            try
            {
                newSpells.SpellNames.Add(spellName.Text);
                newSpells.SpellKeys.Add(key.Text);
                newSpells.Health.Add(Convert.ToInt32(playerHp.Text));
                newSpells.CastTime.Add(Convert.ToInt32(cast.Text));
                newSpells.Mana.Add(Convert.ToInt32(mana.Text));
                newSpells.Range.Add(Convert.ToInt32(maxRange.Text));
                newSpells.Type.Add(type.Text);
                newSpells.Combo.Add(Convert.ToInt32(combo.Text));
                newSpells.MinDistance.Add(Convert.ToInt32(minRange.Text));
                newSpells.EnemyHealth.Add(Convert.ToInt32(enemyHp.Text));
            }
            catch (Exception ex)
            {
                //Fill later.
                errorLabel.Text = ex.Message;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Profile files (.profile)|*.profile|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                try
                {
                    newProfile.FileName = filename;
                    newProfile.Halt();
                    newProfile.Save();

                }
                catch (Exception ex)
                {
                    attachLabel.Text = "Error saving profile";
                    errorLabel.Text = ex.Message;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Spells' profile files (.spell)|*.spell|All Files (*.*)|*.*";
            ofd.FilterIndex = 1;
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;
                try
                {
                    newSpells.Save(filename);

                }
                catch (Exception ex)
                {
                    attachLabel.Text = "Error saving spells";
                    errorLabel.Text = ex.Message;
                }
            }
        }

        private void pointsTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void stopButton3_Click(object sender, EventArgs e)
        {
            newProfile.Halt();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (lootBox1.Checked)
            {
                settings.Looting = true;
            }
            else
            {
                settings.Looting = false;
            }
        }
        private void mountBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(mountBox, 0, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void forwardBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(forwardBox, 1, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void leftBox_Click(object sender, EventArgs e)
        {

            keys.Halt();
            keys = new SetKeys(leftBox, 2, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void rightBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(rightBox, 3, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void downBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(downBox, 4, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void upBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(upBox, 5, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void tabBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(tabBox, 6, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void lootBox_Click(object sender, EventArgs e)
        {
            keys.Halt();
            keys = new SetKeys(lootBox, 7, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void drinkBox_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(drinkBox, 8, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void eatBox_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(eatBox, 9, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void escBox_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(escBox, 10, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void key_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(key, 13, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        { //Check this
            settings.Save();
            try
            {
                bot.Halt();
                botThread.Abort();
            }
            catch(Exception ex){
                //OMG EMPTY CATCH
            }
            try
            {
                newProfile.Halt();
                profileThread.Abort();
            }
            catch (Exception ex)
            {
                //ANOTHER ONE!? THE BAD PRACTICES ARE REAL!!!!
            }
            keys.Halt();
            Process.GetCurrentProcess().Kill(); //Can't seem to be able to turn it off otherwise.
        }

        private void menuButton1_Click(object sender, EventArgs e)
        {
            menuButton2.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            menuButton1.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
            menuButton3.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            panel1.Show();
            panel2.Hide();
            panel3.Hide();
            loadButton2.Show();
            loadButton1.Show();
            attachButton.Show();
            if (start == 1)
            {
                stopButton.Show();
            }
            else if (start == 2 || start == 3)
            {
                startButton.Show();
            }


            newSpellLabel.Hide();
            addSpellLabel.Hide();
            saveLabel2.Hide();
            wstartLabel.Hide();
            gstartLabel.Hide();
            sstartLabel.Hide();
            mpointLabel.Hide();
            sstopLabel.Hide();
            newProfileLabel.Hide();
            saveLabel.Hide();
            openFile.Hide();

        }

        private void menuButton2_Click(object sender, EventArgs e)
        {
            menuButton1.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            menuButton2.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
            menuButton3.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            panel1.Hide();
            panel2.Show();
            panel3.Hide();
            loadButton2.Hide();
            loadButton1.Hide();
            attachButton.Hide();
            startButton.Hide();
            stopButton.Hide();
            newSpellLabel.Show();
            addSpellLabel.Show();
            saveLabel2.Show();
            wstartLabel.Hide();
            gstartLabel.Hide();
            sstartLabel.Hide();
            mpointLabel.Hide();
            sstopLabel.Hide();
            newProfileLabel.Hide();
            saveLabel.Hide();
            openFile.Show();
        }

        private void menuButton1_MouseEnter(object sender, EventArgs e)
        {

            menuButton1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void menuButton1_MouseLeave(object sender, EventArgs e)
        {

            menuButton1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void menuButton2_MouseEnter(object sender, EventArgs e)
        {
            menuButton2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void menuButton2_MouseLeave(object sender, EventArgs e)
        {
            menuButton2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void menuButton3_MouseEnter(object sender, EventArgs e)
        {
            menuButton3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void menuButton3_MouseLeave(object sender, EventArgs e)
        {
            menuButton3.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void loadButton2_MouseEnter(object sender, EventArgs e)
        {
            loadButton2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void loadButton2_MouseLeave(object sender, EventArgs e)
        {
            loadButton2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void loadButton1_MouseEnter(object sender, EventArgs e)
        {
            loadButton1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void loadButton1_MouseLeave(object sender, EventArgs e)
        {
            loadButton1.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void attachButton_MouseEnter(object sender, EventArgs e)
        {
            attachButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void attachButton_MouseLeave(object sender, EventArgs e)
        {
            attachButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void startButton_MouseEnter(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void startButton_MouseLeave(object sender, EventArgs e)
        {
            startButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void stopButton_MouseEnter(object sender, EventArgs e)
        {
            stopButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void stopButton_MouseLeave(object sender, EventArgs e)
        {
            stopButton.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            newSpellLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            newSpellLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            addSpellLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            addSpellLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            saveLabel2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            saveLabel2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            sstopLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            sstopLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            mpointLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            mpointLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            wstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            wstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            sstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            sstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            gstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            gstartLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            saveLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            saveLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void label9_MouseEnter(object sender, EventArgs e)
        {
            newProfileLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void label9_MouseLeave(object sender, EventArgs e)
        {
            newProfileLabel.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void menuButton3_Click(object sender, EventArgs e)
        {
            menuButton1.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            menuButton3.BackColor = System.Drawing.ColorTranslator.FromHtml("#2E2F33 ");
            menuButton2.BackColor = System.Drawing.ColorTranslator.FromHtml("#222326 ");
            panel1.Hide();
            panel2.Hide();
            panel3.Show();
            loadButton2.Hide();
            loadButton1.Hide();
            attachButton.Hide();
            startButton.Hide();
            stopButton.Hide();
            newSpellLabel.Hide();
            addSpellLabel.Hide();
            saveLabel2.Hide();
            newProfileLabel.Show();
            gstartLabel.Show();
            sstartLabel.Show();
            mpointLabel.Show();
            sstopLabel.Show();
            wstartLabel.Show();
            saveLabel.Show();
            openFile.Show();

        }

        private void skinningBox_MouseEnter(object sender, EventArgs e)
        {
            skinningBox.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void skinningBox_MouseLeave(object sender, EventArgs e)
        {
            skinningBox.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void mobsBox_MouseEnter(object sender, EventArgs e)
        {
            mobsBox.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void mobsBox_MouseLeave(object sender, EventArgs e)
        {
            mobsBox.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void playersBox_MouseEnter(object sender, EventArgs e)
        {
            playersBox.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void playersBox_MouseLeave(object sender, EventArgs e)
        {
            playersBox.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void gatheringBox1_MouseEnter(object sender, EventArgs e)
        {
            gatheringBox1.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void gatheringBox1_MouseLeave(object sender, EventArgs e)
        {
            gatheringBox1.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void gatheringBox2_MouseEnter(object sender, EventArgs e)
        {
            gatheringBox2.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void gatheringBox2_MouseLeave(object sender, EventArgs e)
        {
            gatheringBox2.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void lootBox1_MouseEnter(object sender, EventArgs e)
        {
            lootBox1.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void lootBox1_MouseLeave(object sender, EventArgs e)
        {
            lootBox1.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void loopBox_MouseEnter(object sender, EventArgs e)
        {
            loopBox.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void loopBox_MouseLeave(object sender, EventArgs e)
        {
            loopBox.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void zBox_MouseEnter(object sender, EventArgs e)
        {
            zBox.Font = new Font(skinningBox.Font.Name, 12, FontStyle.Underline);
        }

        private void zBox_MouseLeave(object sender, EventArgs e)
        {
            zBox.Font = new Font(skinningBox.Font.Name, 12);
        }

        private void ignoreMobLabel_Click(object sender, EventArgs e)
        {
            if (ignoredMobBox.Text != "" || ignoredMobBox != null)
            {
                newProfile.AddIgnoredMob(ignoredMobBox.Text);
                ignoredMobBox.Text="";
            }
        }

        private void ignoreMobLabel_MouseEnter(object sender, EventArgs e)
        {
            ignoreMobLabel.Font = new Font(ignoreMobLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void ignoreMobLabel_MouseLeave(object sender, EventArgs e)
        {
            ignoreMobLabel.Font = new Font(ignoreMobLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void addGhostPathNew_Click(object sender, EventArgs e)
        {
            newProfile.Ghostpoint += 1;
            ghostNumber.Text = newProfile.Ghostpoint+"";
        }

        private void addGhostPathNew_MouseEnter(object sender, EventArgs e)
        {
            addGhostPathNew.Font = new Font(addGhostPathNew.Font.Name, 12, FontStyle.Underline);
        }

        private void addGhostPathNew_MouseLeave(object sender, EventArgs e)
        {
            addGhostPathNew.Font = new Font(addGhostPathNew.Font.Name, 12);
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (panel2.Visible)
            {
                ofd.Filter = "Spell files (.spell)|*.spell|All Files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Multiselect = false;
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string filename = ofd.FileName;
                    try
                    {
                        newSpells = new Spells();
                        newSpells.Load(filename);
                    }
                    catch (Exception ex)
                    {
                        profileLabel2.Text = "Error opening file";
                        errorLabel.Text = ex.Message;
                    }
                }
            }
            else
            {
                
                ofd.Filter = "Profile files (.profile)|*.profile|All Files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.Multiselect = false;
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string filename = ofd.FileName;
                    try
                    {
                        
                        newProfile = new CreateProfile(magic, pointsTextBox);
                        newProfile.Load(filename);
                        profileThread = new Thread(newProfile.DoWork);
                        profileThread.Start();
                        pointsTextBox.Clear();
                        ghostNumber.Text = newProfile.Ghostpoint+"";
                        if (newProfile.IgnoreZ)
                        {
                            zBox.Checked = true;
                        }
                        else
                        {
                            zBox.Checked = false;

                        }
                        if (newProfile.Loop)
                        {
                            loopBox.Checked = true;
                        }
                        else
                        {
                            loopBox.Checked = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        profileLabel2.Text = "Error opening file";
                        errorLabel.Text = ex.Message;
                    }
                }
            }
        }

        private void openFile_MouseEnter(object sender, EventArgs e)
        {
            openFile.ForeColor = System.Drawing.ColorTranslator.FromHtml("#D5D6DC ");
        }

        private void openFile_MouseLeave(object sender, EventArgs e)
        {
            openFile.ForeColor = System.Drawing.ColorTranslator.FromHtml("#7D7E80 ");
        }

        private void macroLabel_Click(object sender, EventArgs e)
        {

        }

        private void releaseBox_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(releaseBox, 11, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void retrieveBox_Click(object sender, EventArgs e)
        {
            keys.Halt();

            keys = new SetKeys(retrieveBox, 12, settings);
            Thread thread = new Thread(keys.DoWork);

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void ignoreSBox_MouseEnter(object sender, EventArgs e)
        {
            ignoreSBox.Font = new Font(ignoreSBox.Font.Name, 12, FontStyle.Underline);
        }

        private void ignoreSBox_Click(object sender, EventArgs e)
        {
            ulong target = magic.ReadUInt64((uint)TbcOffsets.General.TargetGuid);
            newProfile.AddIgnoredMobGuid(target);
        }

        private void ignoreSBox_MouseLeave(object sender, EventArgs e)
        {
            ignoreSBox.Font = new Font(ignoreSBox.Font.Name, 12);
        }

        private void sendMsgButton_Click(object sender, EventArgs e)
        {
            //
            KeyboardSim keyboardSim = new KeyboardSim();
            keyboardSim.SendToChat(sendMsgBox.Text, channel);
        }

        private void sendMsgButton_MouseEnter(object sender, EventArgs e)
        {
            sendMsgButton.Font = new Font(sendMsgButton.Font.Name, 12, FontStyle.Underline);
        }

        private void sendMsgButton_MouseLeave(object sender, EventArgs e)
        {
            sendMsgButton.Font = new Font(sendMsgButton.Font.Name, 12);
        }

        private void sellingBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sellingBox.Checked)
            {
                settings.Sell = true;
            }
            else
            {
                settings.Sell = false;
            }
        }

        private void linearBox_CheckedChanged(object sender, EventArgs e)
        {
            if (linearBox.Checked)
            {
                settings.Linear = true;
            }
            else
            {
                settings.Linear = false;
            }
        }
        private void setLoops_MouseEnter(object sender, EventArgs e)
        {
            setLoops.Font = new Font(setLoops.Font.Name, 12, FontStyle.Underline);
        }

        private void setLoops_MouseLeave(object sender, EventArgs e)
        {
            setLoops.Font = new Font(setLoops.Font.Name, 12);
        }

        private void sellingBox_MouseEnter(object sender, EventArgs e)
        {
            sellingBox.Font = new Font(sellingBox.Font.Name, 12, FontStyle.Underline);
        }

        private void sellingBox_MouseLeave(object sender, EventArgs e)
        {
            sellingBox.Font = new Font(sellingBox.Font.Name, 12);
        }

        private void setLoops_Click(object sender, EventArgs e)
        {
            settings.SellLoops = Convert.ToInt32(sellTextBox.Text);
        }

        private void linearBox_MouseEnter(object sender, EventArgs e)
        {
            linearBox.Font = new Font(linearBox.Font.Name, 12, FontStyle.Underline);
        }

        private void linearBox_MouseLeave(object sender, EventArgs e)
        {
            linearBox.Font = new Font(linearBox.Font.Name, 12);
        }

        private void sayLabel_Click(object sender, EventArgs e)
        {
            channel = 0;
        }

        private void partyLabel_Click(object sender, EventArgs e)
        {
            channel = 1;
        }

        private void guildLabel_Click(object sender, EventArgs e)
        {
            channel = 2;
        }

        private void whisperLabel_Click(object sender, EventArgs e)
        {
            channel = 3;
        }

        private void replyLabel_Click(object sender, EventArgs e)
        {
            channel = 4;
        }

        private void replyLabel_MouseEnter(object sender, EventArgs e)
        {
            replyLabel.Font = new Font(replyLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void replyLabel_MouseLeave(object sender, EventArgs e)
        {
            replyLabel.Font = new Font(replyLabel.Font.Name, 12);
        }

        private void whisperLabel_MouseEnter(object sender, EventArgs e)
        {
            whisperLabel.Font = new Font(whisperLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void whisperLabel_MouseLeave(object sender, EventArgs e)
        {
            whisperLabel.Font = new Font(whisperLabel.Font.Name, 12);
        }

        private void guildLabel_MouseEnter(object sender, EventArgs e)
        {
            guildLabel.Font = new Font(guildLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void guildLabel_MouseLeave(object sender, EventArgs e)
        {
            guildLabel.Font = new Font(guildLabel.Font.Name, 12);
        }

        private void partyLabel_MouseEnter(object sender, EventArgs e)
        {
            partyLabel.Font = new Font(partyLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void partyLabel_MouseLeave(object sender, EventArgs e)
        {
            partyLabel.Font = new Font(partyLabel.Font.Name, 12);
        }

        private void sayLabel_MouseEnter(object sender, EventArgs e)
        {
            sayLabel.Font = new Font(sayLabel.Font.Name, 12, FontStyle.Underline);
        }

        private void sayLabel_MouseLeave(object sender, EventArgs e)
        {
            sayLabel.Font = new Font(sayLabel.Font.Name, 12);
        }

        private void gatheringBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (gatheringBox1.Checked)
            {
                settings.Herbing = true;
            }
            else
            {
                settings.Herbing = false;
            }
        }

        private void gatheringBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (gatheringBox2.Checked)
            {
                settings.Mining = true;
            }
            else
            {
                settings.Mining = false;
            }
        }
    }
}
