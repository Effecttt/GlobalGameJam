using UnityEngine;

namespace Controllers
{
    [CreateAssetMenu(menuName = "InputControllers/PlayerController", fileName = "PlayerController")]
    public class PlayerController : InputController
    {
        public override float RetrieveMoveInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        public override bool RetrieveJumpInput()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public override bool RetrieveJumpHoldInput()
        {
            return Input.GetKey(KeyCode.Space);
        }
    }
}