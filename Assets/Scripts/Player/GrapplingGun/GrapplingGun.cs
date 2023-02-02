using System;
using Environment;
using Player.Movement;
using UnityEngine;

namespace Player.GrapplingGun
{
    public class GrapplingGun : MonoBehaviour
    {
        public static event Action Swing, ExitSwing, EndRope;
        public Transform firePoint;
        public GameObject controlSupport;

        [SerializeField] GrapplingRope grappleRope;
        [SerializeField] private PlayerMovement movement;

        [SerializeField] private State state = State.Swing;
        
        [SerializeField] private bool grappleToAll;
        [SerializeField] private LayerMask grappableLayerNumber;

        [SerializeField] Camera mCamera;

        [SerializeField] Transform gunHolder;
        [SerializeField] Transform gunPivot;
        [SerializeField] private GameObject arm;

        [SerializeField] SpringJoint2D mSpringJoint2D;
        [SerializeField] Rigidbody2D mRigidbody;
        
        [SerializeField] private bool hasMaxDistance;
        [SerializeField] private float maxDistance = 20;

        [SerializeField] private bool launchToPoint;
        [SerializeField] private float launchSpeed = 1;
        
        [SerializeField] private bool autoConfigureDistance;
        [SerializeField] private float targetDistance = 3;
        [SerializeField] private float targetFrequency = 1;
        
        [SerializeField]private ArmRotation armRot;

        [HideInInspector] public Vector2 grapplePoint;
        [HideInInspector] public Vector2 grappleDistanceVector;

        private MovableBlock block;
        private bool isControlling;

        private void Start()
        {
            grappleRope.enabled = false;
            mSpringJoint2D.enabled = false;
        }
        private void Update()
        {
            StateControl();
            if (state == State.Control)
            {
                if (block && Input.GetKey(KeyCode.Mouse0) && grappleRope.IsReady)
                {
                    block.MoveBlock(mCamera.ScreenToWorldPoint(Input.mousePosition));
                }

                if (controlSupport)
                {
                    grapplePoint = controlSupport.transform.position;
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    KillRope();   
                }
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (SetGrapplePoint())
                {
                    Swing?.Invoke();
                    arm.SetActive(false);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) && state != State.Control)
            {
                KillRope();
            }

        }

        bool SetGrapplePoint()
        {
            Vector2 direction = mCamera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction.normalized, Mathf.Infinity, grappableLayerNumber);
            if (!hit) return false;
            if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance || !hasMaxDistance) 
            {
                if (hit.transform.TryGetComponent(out block))
                {
                    state = State.Control;
                    controlSupport = new GameObject();
                    controlSupport.transform.position = hit.point;
                    controlSupport.transform.parent = hit.transform;
                }
                else
                {
                    state = State.Swing;
                }
                if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
                {
                    if(state != State.Control) grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                    return true; 
                    
                }
            }
            return false;
        }

        public void Grapple()
        {
            mSpringJoint2D.autoConfigureDistance = false;
            if (!launchToPoint && !autoConfigureDistance)
            {
                mSpringJoint2D.distance = targetDistance;
                mSpringJoint2D.frequency = targetFrequency;
            }
            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    mSpringJoint2D.autoConfigureDistance = true;
                    mSpringJoint2D.frequency = 0;
                }

                mSpringJoint2D.connectedAnchor = grapplePoint;
                mSpringJoint2D.enabled = true;
            }
            else
            {
                mSpringJoint2D.connectedAnchor = grapplePoint;

                Vector2 distanceVector = firePoint.position - gunHolder.position;

                mSpringJoint2D.distance = distanceVector.magnitude;
                mSpringJoint2D.frequency = launchSpeed;
                mSpringJoint2D.enabled = true;
            }
        }

        void StateControl()
        {
            if (state == State.Control)
            { 
                if (launchToPoint) launchToPoint = false;
                autoConfigureDistance = true;
                if (!movement.GroundCheck())
                {
                    KillRope();
                }
                movement.enabled = false;
                armRot.enabled = false;
            }
            else if (state == State.Swing)
            {
                autoConfigureDistance = false;
                movement.enabled = true;
                armRot.enabled = true;
                if(controlSupport) Destroy(controlSupport);
            }
            else
            {
                autoConfigureDistance = false;
                movement.enabled = true;
                armRot.enabled = true;
                if(controlSupport) Destroy(controlSupport);
            }

        }

        void KillRope()
        {
            grappleRope.enabled = false;
            mSpringJoint2D.enabled = false;
            mRigidbody.gravityScale = 2;
            arm.SetActive(true);
            if(state == State.Swing) ExitSwing?.Invoke();
            else
            {
                EndRope?.Invoke();
            }
            state = State.Default;
        }

        void SwitchRope()
        {
            //launchToPoint = !launchToPoint;
        }

        public enum State
        {
            Swing,
            Control,
            Default
        }
    }
}