using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Design;
using NDde.Client;
using System.Reactive.Linq;


namespace Bonsai.Videogame
{
    public class AviSoftPlayWAV : Sink
    {

        [FileNameFilter("WAV Files (*.wav)|*.wav")]
        [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", typeof(UITypeEditor))]
        [Description("The name of the wav sound file.")]
        public string FileName { get; set; }

        [Description("Play to start playing the wav file and Stop to stop playing.")]
        public Actions PlayOrStop { get; set; }

        DdeClient client = new DdeClient("RECORDER", "main");

        public enum Actions
        {
            /// <summary>
            /// Starts the specified wav file
            /// </summary>
            Play,

            /// <summary>
            /// Stops the wav file that is playing
            /// </summary>
            Stop
        }

        string command;

        public override IObservable<TSource> Process<TSource>(IObservable<TSource> source)
        {
          
            return Observable.Using(
                () =>
                {
                    if (PlayOrStop == Actions.Play)
                    {
                        command = "play " + FileName;
                    }
                    else if (PlayOrStop == Actions.Stop)
                    {
                        command = "play_stop";
                    }
                    var sendCommand = SendDDECommand(command, 60000);
                    var iterator = sendCommand.GetEnumerator();
                    iterator.MoveNext();
                    return iterator;
                },
                iterator => source.Do(input => iterator.Current(input.ToString()))
            );
        }

        private IEnumerable<Action<string>> SendDDECommand(string command, int timeout)
        {
            while (true)
            {
                yield return source =>
                {
                    client.Connect();
                    client.Execute(command, timeout);
                    client.Disconnect();
                };
            };

        }

    }
}
