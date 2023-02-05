using UnityEngine;

namespace Controllers
{
    public abstract class InputController : ScriptableObject
    {
        public abstract float RetrieveMoveInput();
        
        public abstract bool RetrieveJumpInput();
        
        public abstract bool RetrieveJumpHoldInput();
    }
}
