using System;

namespace Clicker.GameKit {

    public class TimeAccumulator {
        public float t = 0;

        public void Frame(float dt){
            t += dt;
        }
    }
}
