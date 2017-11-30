using System;

namespace Clicker.GameKit {
    /// <summary>
    /// Simple class that keeps track of the current time by adding each discrete
    /// time step. Useful when using temporal functions for the movement of objects.
    /// </summary>
    public class TimeAccumulator {
        public float t = 0;

        public void Frame(float dt){
            t += dt;
        }

        public void Reset(){
            t = 0;
        }
    }
}
