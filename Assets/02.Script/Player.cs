using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    #region Gun
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPos;
    #endregion

    #region Flash
    [SerializeField] private GameObject flashObject;
    [SerializeField] private GameObject flash;
    [SerializeField] private GameObject flashPos;
    private bool isTurnOn = false;
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
        Fire();
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

    private void Flash()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTurnOn = !isTurnOn;
            flash.GetComponent<Light>().enabled = isTurnOn;
        }
    }
}
