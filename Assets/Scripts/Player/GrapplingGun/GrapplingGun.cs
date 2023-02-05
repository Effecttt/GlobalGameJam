using System;
using Checks;
using Environment;
using Player.Capabilities;
using Player.Movement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.GrapplingGun
{
    public class GrapplingGun : MonoBehaviour
    {
        public static event Action Swing, ExitSwing, EndRope;
        public Transform firePoint;
        [HideInInspector]public GameObject controlSupport;

        [SerializeField] GrapplingRope grappleRope;
        [SerializeField] private Move movement;

        [SerializeField] private State state = State.Swing;
        
        [SerializeField] private bool grappleToAll;
        [SerializeField] private LayerMask grappableArea;

        [FormerlySerializedAs("mCamera")] [SerializeField] Camera cam;

        [SerializeField] Transform playerTransform;
        [SerializeField] Transform gunPivot;
        [SerializeField] private GameObject arm;

        [FormerlySerializedAs("mSpringJoint2D")] [SerializeField] SpringJoint2D sprintJoint;
        [FormerlySerializedAs("mRigidbody")] [SerializeField] Rigidbody2D rb;
        
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
            sprintJoint.enabled = false;
        }
        private void Update()
        {
            StateControl();
            if (state == State.Control)
            {
                if (block && Input.GetKey(KeyCode.Mouse0) && grappleRope.IsReady)
                {
                    block.MoveBlock(cam.ScreenToWorldPoint(Input.mousePosition));
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
            Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, Mathf.Infinity, grappableArea);
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
                if (hit.transform.GetComponent<Grappable>() || grappleToAll)
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
            sprintJoint.autoConfigureDistance = false;
            if (!launchToPoint && !autoConfigureDistance)
            {
                sprintJoint.distance = targetDistance;
                sprintJoint.frequency = targetFrequency;
            }
            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    sprintJoint.autoConfigureDistance = true;
                    sprintJoint.frequency = 0;
                }

                sprintJoint.connectedAnchor = grapplePoint;
                sprintJoint.enabled = true;
            }
            else
            {
                sprintJoint.connectedAnchor = grapplePoint;

                Vector2 distanceVector = firePoint.position - playerTransform.position;

                sprintJoint.distance = distanceVector.magnitude;
                sprintJoint.frequency = launchSpeed;
                sprintJoint.enabled = true;
            }
        }

        void StateControl()
        {
            if (state == State.Control)
            { 
                if (launchToPoint) launchToPoint = false;
                autoConfigureDistance = true;
                if (!playerTransform.GetComponent<Ground>().GetOnGround())
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
            sprintJoint.enabled = false;
            rb.gravityScale = 1;
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