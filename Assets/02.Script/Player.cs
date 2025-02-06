using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    private RaycastHit hit;

    #region Gun
    [SerializeField] private GameObject gunObject; //총 오브젝트
    [SerializeField] private GameObject gunPos; //총 위치
    private Vector3 gunRot = new Vector3(-90f, 90f, 0f); //총의 회전 각도
    [SerializeField] private GameObject bulletPos; // 총알 발사 위치
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount;
    [SerializeField] private float spreadAngle; //총알의 퍼지는 각도
    [SerializeField] private float fireRate; //발사 속도
    [SerializeField] private float nextFireTime;
    private bool isHaveGun = false; //총을 들고있는지 확인하는 변수
    #endregion

    #region Flash
    [SerializeField] private GameObject flashObject; //손전등 오브젝트
    [SerializeField] private GameObject flashLight; //빛
    [SerializeField] private GameObject flashPos; //손전등 위치
    private bool isTurnOn = false;
    private bool isHaveFlash = false; //손전등을 들고있는지 확인하는 변수
    #endregion

    #region move
    [SerializeField] private float speed;
    #endregion

    #region camera
    public static float rotSpeed = 500f; //감도

    private float limitMaxY = 50; //카메라 Y축 회전 범위(최대)
    private float limitMinY = -17; //카메라 Y축 회전 범위(최소)

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우스 위 / 아래 이동으로 카메라 x축 회전
    #endregion

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Rotate();
        HaveGun();
        Flash();
        Ray();
        if (Input.GetKeyDown(KeyCode.G)) Throw();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0f, v).normalized;

        transform.Translate(speed * Time.deltaTime * dir, Space.Self);
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        eulerAngleY += mouseX * rotSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, eulerAngleY, 0);

        eulerAngleX -= mouseY * rotSpeed * Time.deltaTime;
        eulerAngleX = Mathf.Clamp(eulerAngleX, limitMinY, limitMaxY);

        Camera.main.transform.localRotation = Quaternion.Euler(eulerAngleX, 0, 0);
    }

    private void HaveGun()
    {
        if (!isHaveGun) return;
        gunObject.transform.position = gunPos.transform.position;
        gunObject.transform.rotation = gunPos.transform.rotation;

        Fire();
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            for (int i = 0; i < bulletCount; i++) 
            {
                float angle = Random.Range(-spreadAngle, spreadAngle);
                Quaternion rotation = Quaternion.Euler(0, 0, -angle);
                GameObject bullet = Instantiate(bulletPrefab, bulletPos.transform.position, bulletPos.transform.rotation * rotation);
                bullet.GetComponent<Bullet>().speed = 50;
            }
        }
    }

    //손전등 켰다 껐다 하는 함수
    private void Flash()
    {
        //손전등을 들고있지 않으면 리턴
        if (!isHaveFlash) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            isTurnOn = !isTurnOn;
            flashLight.GetComponent<Light>().enabled = isTurnOn;
        }

        if (isHaveFlash)
        {
            flashObject.transform.SetPositionAndRotation(flashPos.transform.position, flashPos.transform.rotation);
        }
    }

    //주운 오브젝트를 버리는 함수
    private void Throw()
    {
        if (isHaveFlash) 
            flashObject.GetComponent<Rigidbody>().AddForce(2 * Time.deltaTime * transform.forward , ForceMode.Impulse); isHaveFlash = false;
        if (isHaveGun)
            gunObject.GetComponent<Rigidbody>().AddForce(2 * Time.deltaTime * transform.forward, ForceMode.Impulse); isHaveGun = false;
    }

    private void Ray()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        bool raycastHit = Physics.Raycast(ray, out hit);

        if (raycastHit)
        {
            Transform objHit = hit.transform;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (objHit.name == "FlashLightOBJ")
                {
                    if (isHaveGun) return;
                    isHaveFlash = true;
                    //플레시 줍는 소리
                    flashObject.transform.SetPositionAndRotation(flashPos.transform.position, flashPos.transform.rotation);
                }
                if (objHit.name == "GunOBJ")
                {
                    if (isHaveFlash) return; 
                    isHaveGun = true;
                    //총 줍는 소리(장전 소리로 해도 됨)
                    gunObject.transform.DOMove(gunPos.transform.position, 0.2f).SetEase(Ease.OutExpo);
                    gunObject.transform.DORotate(gunRot, 0.2f).SetEase(Ease.OutExpo);
                }
            }
        }
    }
}
