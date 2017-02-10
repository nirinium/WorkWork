using System.Windows.Forms;
using WorkWork.Bot;

namespace WorkWork
{
    class Statistics
    {
        Main bot;
        Label mobs, died, averageHealth, averageMana, loopLabel;
        public bool changed;
        public Statistics(Main bot, Label mobs, Label died, Label averageHealth, Label averageMana, Label loopLabel)
        {
            this.bot = bot;
            this.mobs = mobs;
            this.averageHealth = averageHealth;
            this.averageMana = averageMana;
            this.died = died;
            this.loopLabel = loopLabel;
        }
        private volatile bool _halt;
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
                var text = mobsText;
                mobs.Invoke(new MethodInvoker(delegate { mobs.Text = text; }));
            }
            if (died.InvokeRequired)
            {
                var text = diedText;
                died.Invoke(new MethodInvoker(delegate { died.Text = text; }));
            }
            if (averageHealth.InvokeRequired)
            {
                var text = averageHealthText;
                averageHealth.Invoke(new MethodInvoker(delegate { averageHealth.Text = text; }));
            }
            if (averageMana.InvokeRequired)
            {
                var text = averageManaText;
                averageMana.Invoke(new MethodInvoker(delegate { averageMana.Text = text; }));
            }
            if (loopLabel.InvokeRequired)
            {
                var loops1 = loops;
                loopLabel.Invoke(new MethodInvoker(delegate { loopLabel.Text = loops1; }));
            }
            while (!_halt)
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
                        var text = mobsText;
                        mobs.Invoke(new MethodInvoker(delegate { mobs.Text = text; }));
                    }
                    if (died.InvokeRequired)
                    {
                        var text = diedText;
                        died.Invoke(new MethodInvoker(delegate { died.Text = text; }));
                    }
                    if (averageHealth.InvokeRequired)
                    {
                        var text = averageHealthText;
                        averageHealth.Invoke(new MethodInvoker(delegate { averageHealth.Text = text; }));
                    }
                    if (averageMana.InvokeRequired)
                    {
                        var text = averageManaText;
                        averageMana.Invoke(new MethodInvoker(delegate { averageMana.Text = text; }));
                    }
                    if (loopLabel.InvokeRequired)
                    {
                        var loops1 = loops;
                        loopLabel.Invoke(new MethodInvoker(delegate { loopLabel.Text = loops1; }));
                    }
                    Changed = false;
                }
            }
        }
        public void Halt()
        {
            _halt = true;
        }
        public bool Changed
        {
            get { return changed; }
            set { changed = value; }
        }
    }
}
