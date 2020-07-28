using UnityEngine;
using UnityEngine.UI;

public class AmmoBag : MonoBehaviour
{
    public int ammoCount;
    public int maxAmmo;
    public Text ammoText;

    private void OnValidate()
    {
        FloorAmmoCountToMax();        
    }

    public bool CanFire()
    {
        return ammoCount > 0;
    }
    public bool CanFire(int bulletAmmount)
    {
        return ammoCount >= bulletAmmount;
    }

    public bool Fire()
    {
        if (CanFire())
        {
            ammoCount -= 1;
            updateAmmoUI();
            return true;
        }
        return false;
    }
    public bool FireMultiple(int bulletAmount)
    {
        if (CanFire(bulletAmount))
        {
            ammoCount -= bulletAmount;
            updateAmmoUI();
            return true;
        }
        return false;
    }

    public void RestockAmmo(int bulletCount)
    {
        ammoCount += bulletCount;
        FloorAmmoCountToMax();
        updateAmmoUI();
    }
    public void MaxRestock()
    {
        RestockAmmo(maxAmmo);
    }

    public int getAmmoCount()
    {
        return ammoCount;
    }
    public void setAmmoCount(int ammoCountValue)
    {
        ammoCount = ammoCountValue;
        FloorAmmoCountToMax();
    }

    public int getMaxAmmoCount()
    {
        return maxAmmo;
    }
    public void setMaxAmmoCount(int maxAmmoCountValue)
    {
        maxAmmo = maxAmmoCountValue;
        FloorAmmoCountToMax();
    }

    private void FloorAmmoCountToMax()
    {
        if (ammoCount > maxAmmo) ammoCount = maxAmmo;
    }

    public void updateAmmoUI()
    {
        ammoText.text = ammoCount.ToString();
    }
}
