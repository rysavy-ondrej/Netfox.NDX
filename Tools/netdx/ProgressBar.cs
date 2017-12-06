using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Netdx
{
    /// <summary>
    /// An ASCII progress bar by Daniel S. Wolf.
    /// <see cref="https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54"/>
    /// </summary>
    public class ProgressBar : IDisposable, IProgress<double>
    {
        private const int blockCount = 20;
        private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);
        private const string animation = @"░▒▓█▓▒"; //@"|/-\";
        private Stopwatch stopwatch = new Stopwatch(); 
        private readonly Timer timer;

        private double currentProgress = 0;
        private string currentText = string.Empty;
        private bool disposed = false;
        private int animationIndex = 0;

        public ProgressBar()
        {
            timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                ResetTimer();
            }
        }

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public void Report(double value)
        {
            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref currentProgress, value);
            if (value == 1)
                CreateAndUpdateText();
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                if (disposed) return;

                CreateAndUpdateText();

                ResetTimer();
            }
        }

        private void CreateAndUpdateText()
        {
            int progressBlockCount = (int)(currentProgress * blockCount);
            int percent = (int)(currentProgress * 100);
            var elapsed = stopwatch.Elapsed;
            var estimated = currentProgress > 0 ? TimeSpan.FromMilliseconds(stopwatch.Elapsed.TotalMilliseconds / currentProgress) : TimeSpan.FromMilliseconds(0);
            var remaining = estimated - elapsed;
            var animatedChar = currentProgress < 1 ? animation[animationIndex++ % animation.Length] : '▓';
            string text = string.Format("[{0}{1}{2}] {3,3}% ■ Elapsed: {4} ■ Remaining: {5}",
                new string('▓', progressBlockCount), 
                new string(animatedChar, 1), 
                new string('░', blockCount - progressBlockCount),
                percent,
                elapsed.ToString(@"hh\:mm\:ss"), remaining.ToString(@"hh\:mm\:ss")
                );
            UpdateText(text);
        }

        private void UpdateText(string text)
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength])
            {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0)
            {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
        }

        private void ResetTimer()
        {
            timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
        }

        public void Dispose()
        {
            lock (timer)
            {
                disposed = true;
                UpdateText(string.Empty);
            }
        }
    }
}