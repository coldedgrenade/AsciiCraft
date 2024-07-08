using System;
using System.Threading;

namespace GameAudio
{
    public class GameAudio
    {
        public enum Tone
        {
            REST = 0,
            C = 261,
            D = 294,
            E = 329,
            F = 349,
            G = 392,
            A = 440,
            B = 493,
            Csharp = 277,
            Dsharp = 311,
            Fsharp = 370,
            Gsharp = 415,
            Asharp = 466
        }

        public enum Duration
        {
            WHOLE = 1600,
            HALF = WHOLE / 2,
            QUARTER = HALF / 2,
            EIGHTH = QUARTER / 2,
            SIXTEENTH = EIGHTH / 2
        }

        public struct Note
        {
            Tone toneVal;
            Duration durVal;

            public Note(Tone frequency, Duration time)
            {
                toneVal = frequency;
                durVal = time;
            }

            public Tone NoteTone { get { return toneVal; } }
            public Duration NoteDuration { get { return durVal; } }
        }

        public void Play(Note[] tune)
        {
            foreach (Note n in tune)
            {
                if (n.NoteTone == Tone.REST)
                    Thread.Sleep((int)n.NoteDuration);
                else
                    Console.Beep((int)n.NoteTone, (int)n.NoteDuration);
            }
        }

        public class Sample
        {
            
        }
    }
}
