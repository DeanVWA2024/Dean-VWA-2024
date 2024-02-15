using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs to instantiate
    private List<GameObject> weapons = new List<GameObject>(); // List of instantiated weapon game objects
    private int activeWeaponIndex = 0; // Index of the currently active weapon

    void Start()
    {
        // Instantiate all weapon prefabs and add them to the weapons list as children of the player
        foreach (GameObject weaponPrefab in weaponPrefabs)
        {
            GameObject weapon = Instantiate(weaponPrefab, transform);
            weapon.SetActive(false);
            weapons.Add(weapon);
        }

        // Set the first weapon to active
        weapons[activeWeaponIndex].SetActive(true);
    }

    void Update()
    {
        // Check for weapon switching input (e.g. number keys)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("pressed 1");
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        // Add more else if statements for additional weapons

        // Check for weapon firing input (e.g. mouse click)
        if (Input.GetMouseButton(0))
        {
            FireWeapon();
        }
    }

    void SwitchWeapon(int index)
    {
        // Deactivate the currently active weapon
        weapons[activeWeaponIndex].SetActive(false);

        // Set the new weapon to active
        activeWeaponIndex = index;
        weapons[activeWeaponIndex].SetActive(true);
    }

    void FireWeapon()
    {
        // Get the active weapon game object and call its Fire method
        GameObject activeWeapon = weapons[activeWeaponIndex];
        if (activeWeapon != null)
        {
            activeWeapon.GetComponentInChildren<Weapon>().Fire();
        }
    }

    
    void AddWeapon(GameObject weaponPrefab)
    {
        // Instantiate the new weapon prefab and add it to the weapons list as a child of the player
        GameObject weapon = Instantiate(weaponPrefab, transform);
        weapon.SetActive(false);
        weapons.Add(weapon);
    }

    void RemoveWeapon(int index)
    {
        // Remove the weapon at the specified index from the weapons list and destroy its game object
        GameObject weapon = weapons[index];
        weapons.RemoveAt(index);
        Destroy(weapon);
    }
    
}