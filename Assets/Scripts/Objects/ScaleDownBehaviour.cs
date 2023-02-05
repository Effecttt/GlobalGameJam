using UnityEngine;

namespace Objects
{
    public class ScaleDownBehaviour : MonoBehaviour
    {
        private bool canScaleDown;
        private float scale = 1;

        [SerializeField] private float speed = 1;
        
        void Update()
        {
            if (canScaleDown)
            {
                scale -= Time.deltaTime * speed;
                transform.localScale = new Vector3(1, scale, 1);
                if (transform.localScale.y <= 0)
                {
                    transform.localScale = new Vector3(1, 0, 1);
                    canScaleDown = false;
                    gameObject.SetActive(false);
                }
            }
        }

        public void Activate()
        {
            canScaleDown = true;
        }
    }
}
