using System.Windows.Forms;
using WorkWork.Bot;

namespace WorkWork
{
    internal class Statistics
    {
        private readonly Main _bot;
        private readonly Label _mobs;
        private readonly Label _died;
        public readonly Label AverageHealth;
        public readonly Label AverageMana;
        public readonly Label LoopLabel;

        public Statistics(Main bot, Label mobs, Label died, Label averageHealth, Label averageMana, Label loopLabel)
        {
            _bot = bot;
            _mobs = mobs;
            AverageHealth = averageHealth;
            AverageMana = averageMana;
            _died = died;
            LoopLabel = loopLabel;
        }
        private volatile bool _halt;
        public void DoWork()
        {
            //get them
            var mobsText = _bot.MobsKilled + "";
            var diedText = _bot.TimesDied + "";
            var averageHealthText = _bot.AverageHealth + "";
            var averageManaText = _bot.AverageMana + "";
            var loops = _bot.Loops + "";
            
            if (_mobs.InvokeRequired)
            {
                var text = mobsText;
                _mobs.Invoke(new MethodInvoker(delegate { _mobs.Text = text; }));
            }
            if (_died.InvokeRequired)
            {
                var text = diedText;
                _died.Invoke(new MethodInvoker(delegate { _died.Text = text; }));
            }
            if (AverageHealth.InvokeRequired)
            {
                var text = averageHealthText;
                AverageHealth.Invoke(new MethodInvoker(delegate { AverageHealth.Text = text; }));
            }
            if (AverageMana.InvokeRequired)
            {
                var text = averageManaText;
                AverageMana.Invoke(new MethodInvoker(delegate { AverageMana.Text = text; }));
            }
            if (LoopLabel.InvokeRequired)
            {
                var loops1 = loops;
                LoopLabel.Invoke(new MethodInvoker(delegate { LoopLabel.Text = loops1; }));
            }
            while (!_halt)
            {
                if (!Changed) continue;
                mobsText = _bot.MobsKilled + "";
                diedText = _bot.TimesDied + "";
                averageHealthText = _bot.AverageHealth + "";
                averageManaText = _bot.AverageMana + "";
                loops = _bot.Loops + "";
                if (_mobs.InvokeRequired)
                {
                    var text = mobsText;
                    _mobs.Invoke(new MethodInvoker(delegate { _mobs.Text = text; }));
                }
                if (_died.InvokeRequired)
                {
                    var text = diedText;
                    _died.Invoke(new MethodInvoker(delegate { _died.Text = text; }));
                }
                if (AverageHealth.InvokeRequired)
                {
                    var text = averageHealthText;
                    AverageHealth.Invoke(new MethodInvoker(delegate { AverageHealth.Text = text; }));
                }
                if (AverageMana.InvokeRequired)
                {
                    var text = averageManaText;
                    AverageMana.Invoke(new MethodInvoker(delegate { AverageMana.Text = text; }));
                }
                if (LoopLabel.InvokeRequired)
                {
                    var loops1 = loops;
                    LoopLabel.Invoke(new MethodInvoker(delegate { LoopLabel.Text = loops1; }));
                }
                Changed = false;
            }
        }
        public void Halt()
        {
            _halt = true;
        }
        public bool Changed { get; set; }
    }
}
