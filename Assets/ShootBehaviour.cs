using Fyp.Game.PlayerControl;
using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootBehaviour : Photon.PunBehaviour
{
    public HandModelBase handModel;
    private Leap.Unity.ExtendedFingerDetector shootGestureDetector;
    private GameObject player;
    public LineRenderer gunTrail;
    private GameObject crosshair;

    public float timeBetweenBullets = 0.75f;
    private bool isShooting;
    float shootTimer;

    /**
     * Specifies how to interprete the direction specified by PointingDirection.
     * 
     * - RelativeToCamera -- the target direction is defined relative to the camera's forward vector, i.e. (0, 0, 1) is the cmaera's 
     *                       local forward direction.
     * - RelativeToHorizon -- the target direction is defined relative to the camera's forward vector, 
     *                        except that it does not change with pitch.
     * - RelativeToWorld -- the target direction is defined as a global direction that does not change with camera movement. For example,
     *                      (0, 1, 0) is always world up, no matter which way the camera is pointing.
     * - AtTarget -- a target object is used as the pointing direction (The specified PointingDirection is ignored).
     * 
     * In VR scenes, RelativeToHorizon with a direction of (0, 0, 1) for camera forward and RelativeToWorld with a direction
     * of (0, 1, 0) for absolute up, are often the most useful settings.
     * @since 4.1.2
     */
    [Header("Direction Settings")]
    [Tooltip("How to treat the target direction.")]
    private PointingType PointingType = PointingType.RelativeToWorld;

    /**
     * The target direction as interpreted by the PointingType setting.
     * Ignored when Pointingtype is "AtTarget."
     * @since 4.1.2
     */
    [Tooltip("The target direction.")]
    private Vector3 PointingDirection = Vector3.forward;

    private UnityAction startShootAction;
    private UnityAction stopShootAction;

    private void startShooting() { this.isShooting = true; Debug.Log("Start Shooting"); }
    private void stopShooting() { this.isShooting = false; Debug.Log("Stop Shooting"); }

    // Start is called before the first frame update
    void Start()
    {
        crosshair = PhotonNetwork.Instantiate("Crosshair", new Vector3(0, 0, 0), Quaternion.identity, 0);

        // HACK: relies on shooting gesture detector being the last detector
        shootGestureDetector = handModel.GetComponents<Leap.Unity.ExtendedFingerDetector>()[handModel.GetComponents<Leap.Unity.ExtendedFingerDetector>().Length - 1];

        startShootAction += this.startShooting;
        stopShootAction += this.stopShooting;
        shootGestureDetector.OnActivate.AddListener(startShootAction);
        shootGestureDetector.OnDeactivate.AddListener(stopShootAction);
    }

    // Update is called once per frame
    void Update()
    {
        //gunTrail.gameObject.SetActive(isShooting);
        player = this.gameObject.GetComponent<Fyp.Game.Carmera.PlayerCamera>().player;
        //Debug.Log("UPdate shooting? " + isShooting);
        if (player.GetPhotonView().isMine)
        {
            //Debug.Log("Detecting shooting" + player.ToString());
            shootTimer += Time.deltaTime;

           
                //shootTimer = 0;
                Hand hand;
                Vector3 fingerDirection;
                Vector3 targetDirection;
                int selectedFinger = (int)Finger.FingerType.TYPE_INDEX;
                hand = handModel.GetLeapHand();

            try
            {
                targetDirection = selectedDirection(hand.Fingers[selectedFinger].TipPosition.ToVector3());
                fingerDirection = hand.Fingers[selectedFinger].Bone(Bone.BoneType.TYPE_PROXIMAL).Direction.ToVector3();
            //float angleTo = Vector3.Angle(fingerDirection, targetDirection);

            Debug.Log("finger tip? " + hand.Fingers[selectedFinger].TipPosition.ToVector3().ToString());

                //gunTrail.SetPosition(0, hand.Fingers[selectedFinger].TipPosition.ToVector3());

            RaycastHit hit;

                //float gun_range = 1;

                if (Physics.Raycast(hand.Fingers[selectedFinger].TipPosition.ToVector3(), fingerDirection, out hit/*, gun_range*/))
                {

//                Vector3 lineOrigin = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ViewportToWorldPoint(hit.point);
 //               Debug.DrawRay(lineOrigin, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform.forward * 1000, Color.green);

                //if (crosshair != null)
                //{
                //    crosshair.transform.position = hit.point;
                //    crosshair.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //} else
                //{
                //    crosshair = PhotonNetwork.Instantiate("Crosshair", hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), 0);
                //}

                //crosshair.transform.position = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(hit.point);

                //      gunTrail.SetPosition(1, hit.point);
                if (isShooting && shootTimer >= timeBetweenBullets)
                {
                    shootTimer = 0;
                    StartCoroutine(AutoDestroy(PhotonNetwork.Instantiate("BulletHole", hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), 0)));
                    Debug.ClearDeveloperConsole();
                    Debug.Log("Gun hit something! " + hit.collider.name);
                    player.GetComponent<ControlScript>().DealDamage(hit.collider.gameObject);

                }
            }

            }
            catch { }
        }
    }

    private IEnumerator AutoDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(10);
        Destroy(obj);
    }

    private Vector3 selectedDirection(Vector3 tipPosition)
    {
        switch (PointingType)
        {
            case PointingType.RelativeToHorizon:
                Quaternion cameraRot = Camera.main.transform.rotation;
                float cameraYaw = cameraRot.eulerAngles.y;
                Quaternion rotator = Quaternion.AngleAxis(cameraYaw, Vector3.up);
                return rotator * PointingDirection;
            case PointingType.RelativeToCamera:
                return Camera.main.transform.TransformDirection(PointingDirection);
            case PointingType.RelativeToWorld:
                return PointingDirection;
//            case PointingType.AtTarget:
  //              return TargetObject.position - tipPosition;
            default:
                return PointingDirection;
        }
    }
}
