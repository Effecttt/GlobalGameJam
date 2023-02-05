using UnityEngine;

namespace Objects
{
    public class TransformDownBehaviour : MonoBehaviour
    {
        private bool canGoDown;
        [SerializeField]private float timer = 10;

        void Update()
        {
            if (canGoDown)
            {
                timer -= Time.deltaTime;
                transform.Translate(Vector3.down * .05f);
                if (timer <= 0)
                {
                    canGoDown = false;
                    gameObject.SetActive(false);
                }
            }
        }

        public void Activate()
        {
            canGoDown = true;
        }
    }
}
