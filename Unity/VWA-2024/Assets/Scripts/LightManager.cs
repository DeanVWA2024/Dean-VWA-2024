using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public List<ObjectData> lightsList = new List<ObjectData>();

    public GameObject GetObjectByName(string objectName)
    {
        ObjectData data = lightsList.Find(obj => obj.objectName == objectName);

        if (data != null)
        {
            return data.associatedGameObject;
        }
        else
        {
            Debug.LogWarning("Object with name " + objectName + " not found.");
            return null;
        }
    }

    public void DisableLights(string lightsName)
    {
        GetObjectByName(lightsName).SetActive(false);
    }

    public void EnableLights(string lightsName)
    {
        GetObjectByName(lightsName).SetActive(true);
    }
}
