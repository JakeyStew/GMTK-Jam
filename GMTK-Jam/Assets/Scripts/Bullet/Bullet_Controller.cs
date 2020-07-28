using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    [SerializeField]
    private int _bulletDamage = 1;
    [SerializeField]
    private float _weaponRange = 50.0f;
    //[SerializeField]
    //private float _hitForce = 100.0f;
    [SerializeField]
    private Transform _firePoint = null;
    [SerializeField]
    private GameObject _ShockwavePrefab = null;

    private WaitForSeconds _shotDuration = new WaitForSeconds(0.07f);    
    private LineRenderer _laserLine;                   

    public LayerMask layerMask;

    private void Start()
    {
        // Get and store a reference to our LineRenderer component
        _laserLine = GetComponent<LineRenderer>();
    }

    public void Shootbullet()
    {
        // Start our ShotEffect coroutine to turn our laser line on and off
        StartCoroutine(ShotEffect());
        Instantiate(_ShockwavePrefab, transform.position, Quaternion.identity);

        RaycastHit hit;
        _laserLine.SetPosition(0, _firePoint.position);

        // Check if our raycast has hit anything
        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hit, _weaponRange,layerMask))
        {
            _laserLine.SetPosition(1, hit.point);
            if(hit.collider.CompareTag("EnemyBody")) 
            {
                EnemyScript health = hit.collider.transform.parent.GetComponent<EnemyScript>();
                if (health != null)
                {
                    health.Damage(_bulletDamage);
                }
            }
        }
        else
        {
            _laserLine.SetPosition(1, _firePoint.position + (_firePoint.forward * _weaponRange));
        }
    }

    private IEnumerator ShotEffect()
    {
        _laserLine.enabled = true;
        yield return _shotDuration;
        _laserLine.enabled = false;
    }
}
