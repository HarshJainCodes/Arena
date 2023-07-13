using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [SerializeField]
    GunDataSO gunData;
    private float timeSinceLastShot;
    [SerializeField]
    private Transform playerCam;
   
    private void Start()
    {
        WeaponShootHandler.shootInput += Shoot;
        WeaponShootHandler.reloadInput += StartReload;
    }
    private bool CanShoot()
    {
        return !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    }
    public void Shoot()
    {
        if(gunData.currentAmmo>0)
        {
            if(CanShoot())
            {
                if(Physics.Raycast(playerCam.position,playerCam.forward,out RaycastHit hitInfo,gunData.maxDistance))
                {
                    IDamagable damagable = hitInfo.transform.GetComponent<IDamagable>();
                    damagable?.Damage(gunData.damage);
                }
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }
    private void OnDisable()
    {
        gunData.reloading = false;
    }
    public void StartReload()
    {
        if(!gunData.reloading && gunData.currentAmmo!=gunData.magSize && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        Debug.Log("realoading");
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(playerCam.position, playerCam.forward * gunData.maxDistance, Color.green);
    }
    private void OnGunShot()
    {

    }
}
