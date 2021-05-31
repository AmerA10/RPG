using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving {
    public interface ISaveable {
        // Start is called before the first frame update
        object CaptureState();

        void RestoreState(object state);
    }


}


