using System;
using System.Collections.Generic;

using SFML.Audio;

using Clicker.GameKit;

namespace Clicker.Game {
    public class SoundManager {
        private class Track {
            internal Music intro;
            internal Music loop;

            public void Load(uint index){
                intro = new Music("Assets/Music/Track.Intro." + index + ".ogg");
                intro.Loop = false;

                loop = new Music("Assets/Music/Track.Loop." + index + ".ogg");
                loop.Loop = true;
            }

            public void Stop(){
                intro.Stop();
                loop.Stop();;
            }
        };

        private enum Status {
            Stopped,
            Intro,
            Loop,
        };

        public const uint TRACK_COUNT = 1;

        private Sound click;
        private List<Track> tracks = new List<Track>();
        private Status status = Status.Stopped;
        private TimeAccumulator time = new TimeAccumulator();
        private int currentTrack;

        public SoundManager() {
            click = new Sound(new SoundBuffer("Assets/SFX/Click.wav"));
            currentTrack = -1;

            for( uint i = 0; i < TRACK_COUNT; ++i ) {
                Track t = new Track();
                t.Load(i);
                tracks.Add(t);
            }
        }

        public void PlayClick(){
            click.Play();
        }

        public void PlayTrack(uint i){
            if( status != Status.Stopped ){
                foreach( Track curr in tracks )
                    curr.Stop();
            }

            tracks[(int) i].intro.Play();
            status = Status.Intro;
            currentTrack = (int) i;
        }

        public void Update(float dt){
            time.Frame(dt);

            // If an intro has completed, play the loop part.
            if( status == Status.Intro ){
                for( int i = 0; i < tracks.Count;  ++i){
                    Track curr = tracks[i];

                    if( i == currentTrack && curr.intro.Status == SoundStatus.Stopped ){
                        curr.loop.Play();
                        status = Status.Loop;
                    }
                }
            }
        }

        public void StopEverything(){
            click.Stop();

            foreach(Track curr in tracks){
                curr.intro.Stop();;
                curr.loop.Stop();;
            }
        }
    }
}
