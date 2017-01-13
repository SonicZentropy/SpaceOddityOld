// /** 
//  * Cameraengine.cs
//  * Dylan Bailey
//  * 20161209
// */


#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zenobit.Common.Camera
{
    #region Dependencies

    using UnityEngine;

    #endregion

    public class Cameraengine : MonoBehaviour
    {
        public bool AdaptToTerrainHeight = true;
        public bool AllowDoubleClickMovement = false;
        public bool AllowScreenEdgeMovement = true;

        //Internal

        private Camera _cam;
        public float CameraRotationSpeed = 10f;
        public float CameraScrollSpeed = 10f;
        public Vector3 CameraTarget;
        public float CameraZoomSpeed = 10f;
        private float _camV;
        private float _camH;
        private float _camZoom;
        private float _camRot;
        private float _camPitch;
        public bool CorrectZoomingOutRatio = true;

// -------------------------- Private Attributes --------------------------
        private float _currentCameraDistance;
        private bool _doingAutoMovement;
        private Vector3 _goingToCameraTarget = Vector3.zero;
        public float GoToSpeed = 0.1f;
        public bool IncreaseSpeedWhenZoomedOut = true;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _lastMousePos;
        private Vector3 _lastPanSpeed = Vector3.zero;
        public float MaxZoomDistance = 200.0f;
        public float MinZoomDistance = 20.0f;

        public float MousePanMultiplier = 0.1f;
        public float MouseRotationMultiplier = 0.2f;
        public float MouseZoomMultiplier = 5.0f;

        public GameObject ObjectToFollow;

        public int ScreenEdgeSize = 10;
        public float ScreenEdgeSpeed = 1.0f;
        public bool Smoothing = true;
        public float SmoothingFactor = 0.1f;

        //Camera config
        [SerializeField] private readonly UpdateType updateType = UpdateType.LateUpdate;

        public bool UseKeyboardInput = true;
        public bool UseMouseInput = true;

//private var doubleClickDetector : DoubleClickDetector;


        ///OnAwake happens once upon Instantiation, so after Awake and OnEnable
        public void OnAwake()
        {
            //if (!IsEnabled) return;
            //CameraVertical, CameraHorizontal, CameraZoom, CameraRotate, CameraPitch, CameraReset
            if (_cam == null) _cam = GetComponent<Camera>();
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        public void OnStart()
        {
            _currentCameraDistance = MinZoomDistance + (MaxZoomDistance - MinZoomDistance)/2.0f;
            _lastMousePos = Vector3.zero;
        }

        public void OnUpdate()
        {
            _camV = Input.GetAxisRaw("CameraVertical");
            _camH = Input.GetAxisRaw("CameraHorizontal");
            _camZoom = Input.GetAxis("CameraZoom");
            _camRot = Input.GetAxisRaw("CameraRotate");
            _camPitch = Input.GetAxisRaw("CameraPitchOn");

            if (Input.GetButtonDown("CameraReset"))
            {
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;
            }

            if (updateType == UpdateType.Update)
                MovementUpdate(Time.deltaTime);
        }

        private void MovementUpdate(float deltaTime)
        {
            UpdatePanning(deltaTime);
            UpdateRotation(deltaTime);
            UpdateZooming(deltaTime);
            UpdatePosition(deltaTime);
            UpdateAutoMovement(deltaTime);
            _lastMousePos = Input.mousePosition;
        }

        public void OnFixedUpdate()
        {
            if (updateType == UpdateType.FixedUpdate)
                MovementUpdate(Time.fixedDeltaTime);
        }

        public void OnLateUpdate()
        {
            if (updateType == UpdateType.LateUpdate)
                MovementUpdate(Time.deltaTime);
        }


        public void GoTo(Vector3 position)
        {
            _doingAutoMovement = true;
            _goingToCameraTarget = position;
            ObjectToFollow = null;
        }

        public void Follow(GameObject gameObjectToFollow)
        {
            ObjectToFollow = gameObjectToFollow;
        }


        private void UpdatePanning(float deltaTime)
        {
            var moveVector = new Vector3(0, 0, 0);
            if (UseKeyboardInput)
            {
                moveVector += new Vector3(
                    _camH*CameraScrollSpeed*deltaTime,
                    _camZoom*CameraZoomSpeed*deltaTime,
                    _camV*CameraScrollSpeed*deltaTime
                );
            }

            if (AllowScreenEdgeMovement)
            {
                if (Input.mousePosition.x < ScreenEdgeSize)
                {
                    moveVector.x -= ScreenEdgeSpeed;
                }
                else if (Input.mousePosition.x > Screen.width - ScreenEdgeSize)
                {
                    moveVector.x += ScreenEdgeSpeed;
                }

                if (Input.mousePosition.y < ScreenEdgeSize)
                {
                    moveVector.z -= ScreenEdgeSpeed;
                }
                else if (Input.mousePosition.y > Screen.height - ScreenEdgeSize)
                {
                    moveVector.z += ScreenEdgeSpeed;
                }
            }

            if (UseMouseInput)
            {
                if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftShift))
                {
                    var deltaMousePos = Input.mousePosition - _lastMousePos;
                    moveVector += new Vector3(-deltaMousePos.x, 0, -deltaMousePos.y)*MousePanMultiplier;
                }
            }

            if (moveVector != Vector3.zero)
            {
                ObjectToFollow = null;
                _doingAutoMovement = false;
            }

            var effectivePanSpeed = moveVector;
            if (Smoothing)
            {
                effectivePanSpeed = Vector3.Lerp(_lastPanSpeed, moveVector, SmoothingFactor);
                _lastPanSpeed = effectivePanSpeed;
            }

            var oldRotation = transform.localEulerAngles.x;
            transform.localEulerAngles = new Vector3(0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            var panMultiplier = IncreaseSpeedWhenZoomedOut ? Mathf.Sqrt(_currentCameraDistance) : 1.0f;
            CameraTarget = CameraTarget +
                           transform.TransformDirection(effectivePanSpeed)*CameraScrollSpeed*panMultiplier*
                           Time.deltaTime;
            transform.localEulerAngles = new Vector3(
                oldRotation,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z);
        }

        private void UpdateRotation(float deltaTime)
        {
            var deltaAngleH = 0.0f;
            var deltaAngleV = 0.0f;

            if (UseKeyboardInput)
            {
                deltaAngleH = _camRot*deltaTime*CameraRotationSpeed;
            }

            if (UseMouseInput)
            {
                if (Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftShift))
                {
                    var deltaMousePos = Input.mousePosition - _lastMousePos;
                    deltaAngleH += deltaMousePos.x*MouseRotationMultiplier;
                    deltaAngleV -= deltaMousePos.y*MouseRotationMultiplier;
                }
            }
            transform.localEulerAngles = new Vector3(
                Mathf.Min(
                    80.0f,
                    Mathf.Max(5.0f, transform.localEulerAngles.x + deltaAngleV*Time.deltaTime*CameraRotationSpeed)),
                transform.localEulerAngles.y + deltaAngleH*Time.deltaTime*CameraRotationSpeed,
                0
            );
        }

        private void UpdateZooming(float deltaTime)
        {
            var deltaZoom = 0.0f;

            if (UseMouseInput)
            {
                var scroll = Input.GetAxis("Mouse ScrollWheel");
                deltaZoom -= scroll*MouseZoomMultiplier;
            }
            var zoomedOutRatio = CorrectZoomingOutRatio
                ? (_currentCameraDistance - MinZoomDistance)/(MaxZoomDistance - MinZoomDistance)
                : 0.0f;
            _currentCameraDistance = Mathf.Max(
                MinZoomDistance,
                Mathf.Min(
                    MaxZoomDistance,
                    _currentCameraDistance + deltaZoom*Time.deltaTime*CameraZoomSpeed*(zoomedOutRatio*2.0f + 1.0f)));
        }

        private void UpdatePosition(float deltaTime)
        {
            if (ObjectToFollow != null)
            {
                CameraTarget = Vector3.Lerp(
                    CameraTarget,
                    ObjectToFollow.transform.position,
                    GoToSpeed);
            }

            transform.position = CameraTarget;
            transform.Translate(Vector3.back*_currentCameraDistance);

            if ((transform.position.y < MinZoomDistance) || (transform.position.y > MaxZoomDistance))
                transform.position = new Vector3(
                    transform.position.x,
                    Mathf.Clamp(transform.position.y, MinZoomDistance, MaxZoomDistance),
                    transform.position.z);
        }

        private void UpdateAutoMovement(float deltaTime)
        {
            if (!_doingAutoMovement) return;

            CameraTarget = Vector3.Lerp(CameraTarget, _goingToCameraTarget, GoToSpeed);
            if (Vector3.Distance(_goingToCameraTarget, CameraTarget) < 1.0f)
            {
                _doingAutoMovement = false;
            }
        }
    }


    
}