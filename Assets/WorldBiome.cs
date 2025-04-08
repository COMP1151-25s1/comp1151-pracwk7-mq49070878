using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldBiome", menuName = "Scriptable Objects/WorldBiome")]
public class WorldBiome : ScriptableObject
{
    public string biomeName;
    public string biomeDescription;
    
    public List<BiomeObjectsData> biomeObjects;
}

[System.Serializable]
public class BiomeObjectsData
{
    public float density;
    public GameObject densityObject;
    
    public Vector2 randomScl;
    public Vector2 randomRot;
    public Vector2 randomVel;

    /*public Vector2 GetRandomPos()
    {
        return new Vector2(Random.Range(randomPos.x, randomPos.y), Random.Range(randomPos.x, randomPos.y));
    }*/
    
    public Quaternion GetRandomRot()
    {
        return Quaternion.Euler(0,0,Random.Range(randomRot.x, randomRot.y));
    }
    
    public Vector2 GetRandomScl()
    {
        float randomScale = Random.Range(randomScl.x, randomScl.y);
        return new Vector2(randomScale, randomScale);
    }
    
    public Vector2 GetRandomVel()
    {
        return new Vector2(Random.Range(randomVel.x, randomVel.y), Random.Range(randomVel.x, randomVel.y));
    }
}
