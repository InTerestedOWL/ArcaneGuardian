using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AG.Control {
    public enum POIStateId {
        FollowPlayer,
        Idle,
        Death
    }

    public interface POIState
    {
        AiStateId GetId();
        void Enter(POIController controller);
        void Update(POIController controller);
        void Exit(POIController controller);
    }    
}

