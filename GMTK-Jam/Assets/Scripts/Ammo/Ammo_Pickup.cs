using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Pickup : MonoBehaviour
{
    [SerializeField]
    private float _ammoBoxRotateSpeed = 15.0f;
    [SerializeField]
    private int _ammoCount = 15;

    private CapsuleCollider _playerCapCollider;

    private void Update()
    {
        transform.Rotate(Vector3.forward * _ammoBoxRotateSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time), transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            Player_Controller player = other.GetComponentInParent<Player_Controller>();
            _playerCapCollider = other.GetComponent<CapsuleCollider>();
            if (player != null && _playerCapCollider != null)
            {
                AmmoBag ammoBag = player.GetAmmoBag;
                if (ammoBag != null)
                {
                    ammoBag.RestockAmmo(_ammoCount);
                }
            }
            Destroy(gameObject);
        }
    }
}
