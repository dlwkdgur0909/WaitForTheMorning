using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    private RaycastHit hit;

    #region Gun
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPos;
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
        Fire();
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

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, bulletPos.rotation);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward;

            Destroy(bullet, 2f);
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
        if (isHaveFlash) flashObject.GetComponent<Rigidbody>().AddForce(5 * Time.deltaTime * transform.forward , ForceMode.Impulse); isHaveFlash = false;
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
                    isHaveFlash = true;
                    flashObject.transform.SetPositionAndRotation(flashPos.transform.position, flashPos.transform.rotation);
                }
            }
        }
    }
}
