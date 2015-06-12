using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using WorkWork.Bot;

namespace WorkWork
{
    class Statistics
    {
        Main bot;
        Label mobs, died, averageHealth, averageMana, loopLabel;
        public bool changed = false;
        public Statistics(Main bot, Label mobs, Label died, Label averageHealth, Label averageMana, Label loopLabel)
        {
            this.bot = bot;
            this.mobs = mobs;
            this.averageHealth = averageHealth;
            this.averageMana = averageMana;
            this.died = died;
            this.loopLabel = loopLabel;
        }
        private volatile bool halt = false;
        public void DoWork()
        {
            //get them
            string mobsText = bot.MobsKilled + "";
            string diedText = bot.TimesDied + "";
            string averageHealthText = bot.AverageHealth + "";
            string averageManaText = bot.AverageMana + "";
            string loops = bot.Loops + "";
            
            if (mobs.InvokeRequired)
            {
                mobs.Invoke(new MethodInvoker(delegate { mobs.Text = mobsText; }));
            }
            if (died.InvokeRequired)
            {
                died.Invoke(new MethodInvoker(delegate { died.Text = diedText; }));
            }
            if (averageHealth.InvokeRequired)
            {
                averageHealth.Invoke(new MethodInvoker(delegate { averageHealth.Text = averageHealthText; }));
            }
            if (averageMana.InvokeRequired)
            {
                averageMana.Invoke(new MethodInvoker(delegate { averageMana.Text = averageManaText; }));
            }
            if (loopLabel.InvokeRequired)
            {
                loopLabel.Invoke(new MethodInvoker(delegate { loopLabel.Text = loops; }));
            }
            while (!halt)
            {
                if (Changed)
                {
                    mobsText = bot.MobsKilled + "";
                    diedText = bot.TimesDied + "";
                    averageHealthText = bot.AverageHealth + "";
                    averageManaText = bot.AverageMana + "";
                    loops = bot.Loops + "";
                    if (mobs.InvokeRequired)
                    {
                        mobs.Invoke(new MethodInvoker(delegate { mobs.Text = mobsText; }));
                    }
                    if (died.InvokeRequired)
                    {
                        died.Invoke(new MethodInvoker(delegate { died.Text = diedText; }));
                    }
                    if (averageHealth.InvokeRequired)
                    {
                        averageHealth.Invoke(new MethodInvoker(delegate { averageHealth.Text = averageHealthText; }));
                    }
                    if (averageMana.InvokeRequired)
                    {
                        averageMana.Invoke(new MethodInvoker(delegate { averageMana.Text = averageManaText; }));
                    }
                    if (loopLabel.InvokeRequired)
                    {
                        loopLabel.Invoke(new MethodInvoker(delegate { loopLabel.Text = loops; }));
                    }
                    Changed = false;
                }
            }
        }
        public void Halt()
        {
            halt = true;
        }
        public bool Changed
        {
            get { return changed; }
            set { changed = value; }
        }
    }
}
