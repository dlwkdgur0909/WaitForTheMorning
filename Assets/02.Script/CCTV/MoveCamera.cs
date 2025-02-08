using System.Collections;
using UnityEngine;
using DG.Tweening;
using static Player;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance;
    [SerializeField] private Camera mainCamera;
    public GameObject[] cameras;

    #region CameraPos
    public GameObject playerPos;

    private Vector3 CenterPos = new Vector3(0, 10.5f, -3.3f);

    private Vector3 RightPos = new Vector3(4f, 12.31f, -2.3f);
    private Vector3 RightRot = new Vector3(-4.2f, 34, 0);

    private Vector3 LeftPos = new Vector3(-4f, 12.31f, -2.3f);
    private Vector3 LeftRot = new Vector3(-4.2f, -34, 0);

    private Vector3 TowerPos = new Vector3(3.8f, 9.3f, -2.26f);
    private Vector3 TowerRot = new Vector3(5.6f, 35.6f, 0f);
    #endregion

    //현재 상태가 CCTV를 보고있는 상태인지 확인하는 변수
    public bool isOnCamera = false;

    public enum CameraState
    {
        Original, Center, Right, Left, Tower
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentState = CameraState.Original;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveCameraState(currentState);
            CurrentState = CameraState.Original;
        }

        if (isOnCamera == true) instance.gunObject.SetActive(false);
        else instance.gunObject.SetActive(true);
    }

    //cameraIndex에 따라 메인 카메라가 선택된 cameraIndex로 이동
    IEnumerator CameraChange(CameraState cameraIndex)
    {
        yield return new WaitForSeconds(0.5f);
        for (CameraState i = CameraState.Original; i <= CameraState.Tower; ++i)
        {
            if (i == cameraIndex)
            {
                if (i == CameraState.Original)
                {
                    mainCamera.transform.position = cameras[(int)i].transform.position;
                    mainCamera.transform.localRotation = cameras[(int)i].transform.localRotation;
                    isOnCamera = false;
                }
                else
                {
                    mainCamera.transform.position = cameras[(int)i].transform.position;
                    mainCamera.transform.localRotation = cameras[(int)i].transform.localRotation;
                    isOnCamera = true;
                }
            }
        }
    }

    //스페이스바를 눌렀을 때 선택된 cameraIndex의 회전값을 메인카메라에게 저장함
    public void SaveCameraState(CameraState cameraIndex)
    {
        for (CameraState i = CameraState.Original; i <= CameraState.Tower; ++i)
        {
            if (i == cameraIndex)
            {
                cameras[(int)i].transform.rotation = mainCamera.transform.rotation;
            }
        }
    }

    [SerializeField] public CameraState currentState = CameraState.Original;
    public CameraState CurrentState { get { return currentState; } set { currentState = value; CameraMove(); } }

    private void CameraMove()
    {
        switch (currentState)
        {
            case CameraState.Original:
                mainCamera.transform.DOMove(playerPos.transform.position, 0.0001f).SetEase(Ease.OutExpo);
                mainCamera.transform.DORotate(new Vector3(0, 0, 0), 0.0001f).SetEase(Ease.OutExpo);
                StartCoroutine(CameraChange(CameraState.Original));
                SaveCameraState(CameraState.Original);
                break;
            case CameraState.Center:
                {
                    mainCamera.transform.DOMove(CenterPos, 0.5f).SetEase(Ease.OutExpo);
                    StartCoroutine(CameraChange(CameraState.Center));
                    SaveCameraState(CameraState.Center);
                };
                break;
            case CameraState.Right:
                {
                    mainCamera.transform.DOMove(RightPos, 0.5f).SetEase(Ease.OutExpo);
                    mainCamera.transform.DORotate(RightRot, 0.5f).SetEase(Ease.OutExpo);
                    StartCoroutine(CameraChange(CameraState.Right));
                    SaveCameraState(CameraState.Right);
                };
                break;
            case CameraState.Left:
                {
                    mainCamera.transform.DOMove(LeftPos, 0.5f).SetEase(Ease.OutExpo);
                    mainCamera.transform.DORotate(LeftRot, 0.5f).SetEase(Ease.OutExpo);
                    StartCoroutine(CameraChange(CameraState.Left));
                    SaveCameraState(CameraState.Left);
                };
                break;
            case CameraState.Tower:
                {
                    mainCamera.transform.DOMove(TowerPos, 0.5f).SetEase(Ease.OutExpo);
                    mainCamera.transform.DORotate(TowerRot, 0.5f).SetEase(Ease.OutExpo);
                    StartCoroutine(CameraChange(CameraState.Tower));
                    SaveCameraState(CameraState.Tower);
                }
                break;
        }
    }
}
