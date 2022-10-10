using UnityEngine;
using UnityEngine.UI;

public class ProjectileGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject projectile; 
    [SerializeField] Text ammoText;
    private SoundManager soundMNG;

    [Header("Weapon configurations")]
    [SerializeField] float range, damage, fireTime, fireRate; 
    [SerializeField] int bulletsPerMag, bulletsLeft; 
    int currentBullets; 
    bool isReloading;

    [Header("Projectile configurations")]
    [SerializeField] float forwardSpeed, upwardForce;

    private void OnEnable() => UpdateAmmoText(); 
    private void Start()
    {
        soundMNG = GetComponent<SoundManager>();
        currentBullets = bulletsPerMag;
        UpdateAmmoText();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if (currentBullets > 0) Shoot();
            else if (bulletsLeft > 0) Reload();
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0) Reload();
        }
        if (fireTime < fireRate) 
            fireTime += Time.deltaTime;
    }

    private void UpdateAmmoText() => ammoText.text = "Ammo: " + currentBullets + "/" + bulletsLeft;
    private void Shoot()
    {
        if (fireTime < fireRate || currentBullets <= 0 ) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0, 0, 0)); // a ray through the middle of view
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 direction = targetPoint - shootPoint.position;

        GameObject currentProjectile = Instantiate(projectile, shootPoint.position, Quaternion.identity); 
        currentProjectile.GetComponent<Rigidbody>().AddForce(cam.transform.forward * forwardSpeed, ForceMode.Impulse);
        currentProjectile.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);
        Destroy(currentProjectile, 3f);

        currentBullets--;
        soundMNG.PlaySound("shoot");
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

        UpdateAmmoText();
    }
}
