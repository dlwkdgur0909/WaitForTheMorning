using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    private RaycastHit hit;

    #region Gun
    [SerializeField] private GameObject gunObject; //�� ������Ʈ
    [SerializeField] private GameObject gunPos; //�� ��ġ
    private Vector3 gunRot = new Vector3(-90f, 90f, 0f); //���� ȸ�� ����
    [SerializeField] private GameObject bulletPos; // �Ѿ� �߻� ��ġ
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount;
    [SerializeField] private float spreadAngle; //�Ѿ��� ������ ����
    [SerializeField] private float fireRate; //�߻� �ӵ�
    [SerializeField] private float nextFireTime;
    private bool isHaveGun = false; //���� ����ִ��� Ȯ���ϴ� ����
    #endregion

    #region Flash
    [SerializeField] private GameObject flashObject; //������ ������Ʈ
    [SerializeField] private GameObject flashLight; //��
    [SerializeField] private GameObject flashPos; //������ ��ġ
    private bool isTurnOn = false;
    private bool isHaveFlash = false; //�������� ����ִ��� Ȯ���ϴ� ����
    #endregion

    #region move
    [SerializeField] private float speed;
    #endregion

    #region camera
    public static float rotSpeed = 500f; //����

    private float limitMaxY = 50; //ī�޶� Y�� ȸ�� ����(�ִ�)
    private float limitMinY = -17; //ī�޶� Y�� ȸ�� ����(�ּ�)

    private float eulerAngleX; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
    private float eulerAngleY; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��
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

    //������ �״� ���� �ϴ� �Լ�
    private void Flash()
    {
        //�������� ������� ������ ����
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

    //�ֿ� ������Ʈ�� ������ �Լ�
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
                    //�÷��� �ݴ� �Ҹ�
                    flashObject.transform.SetPositionAndRotation(flashPos.transform.position, flashPos.transform.rotation);
                }
                if (objHit.name == "GunOBJ")
                {
                    if (isHaveFlash) return; 
                    isHaveGun = true;
                    //�� �ݴ� �Ҹ�(���� �Ҹ��� �ص� ��)
                    gunObject.transform.DOMove(gunPos.transform.position, 0.2f).SetEase(Ease.OutExpo);
                    gunObject.transform.DORotate(gunRot, 0.2f).SetEase(Ease.OutExpo);
                }
            }
        }
    }
}
