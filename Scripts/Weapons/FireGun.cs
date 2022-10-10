using UnityEngine;
using UnityEngine.UI;

public class FireGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Text ammoText;
    private SoundManager soundMNG;

    [Header("Weapon configurations")]
    [SerializeField] bool isAutoGun;
    [SerializeField] float range, damage, fireTime, fireRate; 
    [SerializeField] int bulletsPerMag, bulletsLeft; 
    int currentBullets; 
    bool isReloading, isShooting;

    private void OnEnable() => UpdateAmmoText();
    private void Start()
    {
        soundMNG = GetComponent<SoundManager>();
        currentBullets = bulletsPerMag;
        UpdateAmmoText();
    }
    private void Update()
    {
        if (isAutoGun) isShooting = Input.GetKey(KeyCode.Mouse0);
        else isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (isShooting) 
        {
            if (currentBullets > 0) Shoot();
            else if (bulletsLeft > 0) Reload(); //automatically reloading
        }
        if (Input.GetKeyDown(KeyCode.R)) //manually reloading
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0) Reload();
        }
        if (fireTime < fireRate) 
            fireTime += Time.deltaTime;
    }

    private void UpdateAmmoText() => ammoText.text = "Ammo: " + currentBullets + "/" + bulletsLeft;
    private void Shoot()
    {
        if (fireTime < fireRate || currentBullets <= 0) return;

        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
                hit.collider.GetComponent<EnemyAI>().takeDamageEnemy(damage);
        }

        soundMNG.PlaySound("shoot");
        currentBullets--;
        UpdateAmmoText();
        fireTime = 0f; 
    }
    private void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;
        bulletsLeft -= bulletToDeduct;
        currentBullets += bulletToDeduct;

        soundMNG.PlaySound("reload");
        UpdateAmmoText();
    }
}
