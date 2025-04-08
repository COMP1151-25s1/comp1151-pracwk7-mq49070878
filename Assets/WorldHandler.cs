using UnityEngine;
using System.Collections.Generic;
public class WorldHandler : MonoBehaviour
{
    public Transform spawnAroundPoint;
    
    public List<WorldBiome> worldBiomes;
    //public WorldBiome currentBiome;
    
    public List<Chunk> chunks;
    public int chunkSize = 16;
    public int chunkRenderRadiusDistance = 4;

    private int updateIndex = 0;

    private void Start()
    {
        SpawnChunksAroundPlayer();
    }

    public void SpawnChunksAroundPlayer()
    {
        Vector2Int centerChunk = GetChunkFromWorldPosition(spawnAroundPoint.position);
        Vector2Int topCornerChunk = new Vector2Int(centerChunk.x + chunkRenderRadiusDistance, centerChunk.y + chunkRenderRadiusDistance);

        for (int x = 0; x < chunkRenderRadiusDistance * 2; x++)
        {
            for (int y = 0; y < chunkRenderRadiusDistance * 2; y++)
            {
                Vector2Int currentChunkPosition =  new Vector2Int(topCornerChunk.x - x, topCornerChunk.y - y);
                
                if (!DoesChunkExist(currentChunkPosition))
                {
                    // biome logic
                    WorldBiome newChunkBiome = worldBiomes[Random.Range(0, worldBiomes.Count)];
                    
                    Chunk newChunk = new Chunk(currentChunkPosition, newChunkBiome, chunkSize);
                    chunks.Add(newChunk);
                }
            }
        }
    }

    private void Update()
    {
        int loopThroughValue = (int)Mathf.Pow(chunkRenderRadiusDistance * 2, 2);
        updateIndex++;
        
        if (updateIndex >= loopThroughValue) updateIndex = 0;
        
        Vector2Int centerChunk = GetChunkFromWorldPosition(spawnAroundPoint.position);

        int renderRadius = chunkRenderRadiusDistance * 2;
    
        int chunkIndexX = updateIndex % renderRadius;
        int chunkIndexY = updateIndex / renderRadius;
    
        Vector2Int chunkPositionToCheck = new Vector2Int(centerChunk.x + chunkIndexX - chunkRenderRadiusDistance, centerChunk.y + chunkIndexY - chunkRenderRadiusDistance);

        if (!DoesChunkExist(chunkPositionToCheck))
        {
            WorldBiome newChunkBiome = worldBiomes[Random.Range(0, worldBiomes.Count)];
            Chunk newChunk = new Chunk(chunkPositionToCheck, newChunkBiome, chunkSize);
            chunks.Add(newChunk);
        }
    }

    public Vector2Int GetChunkFromWorldPosition(Vector2 worldPosition)
    {
        Vector2 divPos = new Vector2(worldPosition.x / chunkSize, worldPosition.y / chunkSize);
        return Vector2Int.RoundToInt(divPos);
    }

    public bool DoesChunkExist(Vector2Int chunkPosition)
    {
        foreach (Chunk chunk in chunks)
        {
            if (chunk.chunkPosition == chunkPosition) return true;
        }
        
        return false;
    }
    
}

[System.Serializable]
public class Chunk
{
    public Vector2Int chunkPosition;
    public List<ChunkObjectData> chunkObjects;
    public WorldBiome chunkBiome;
    private float chunkSize;

    public Chunk(Vector2Int _pos, WorldBiome _biome, float _chunkSize)
    {
        chunkPosition = _pos;
        chunkBiome = _biome;
        chunkSize = _chunkSize;

        SpawnObjectsInChunk();
    }

    public void SpawnObjectsInChunk()
    {
        float sampleSize = 1f;
        float sampleMulti = sampleSize/(chunkSize*chunkSize);

        for (float x = (-chunkSize / 2); x < (chunkSize / 2); x += sampleSize)
        {
            for (float y = (-chunkSize / 2); y < (chunkSize / 2); y += sampleSize)
            {
                Vector2 perlinNoisePos = ((Vector2)chunkPosition * chunkSize) + new Vector2(x, y);
                //float noiseValue = Mathf.PerlinNoise(perlinNoisePos.x, perlinNoisePos.y);

                foreach (BiomeObjectsData biomeObject in chunkBiome.biomeObjects)
                {
                    float noiseValue = Random.Range(0f, 1f);

                    if (noiseValue <= biomeObject.density * sampleMulti)
                    {
                        GameObject newObj = GameObject.Instantiate(biomeObject.densityObject, perlinNoisePos, Quaternion.identity);
                        newObj.transform.rotation = biomeObject.GetRandomRot();
                        newObj.transform.localScale = biomeObject.GetRandomScl();
                        if (newObj.GetComponent<Rigidbody2D>() != null) newObj.GetComponent<Rigidbody2D>().linearVelocity = biomeObject.GetRandomVel();
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class ChunkObjectData
{
    public Vector2 _position;
    public Quaternion _rotation;
    public Vector2 _scale;
    
    public GameObject _gameObject;
}
