using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    [HideInInspector] public GameObject particleSystemPrefab;
    
    private List<PooledParticleSystem> m_pooledParticleSystems;

    private void Start()
    {
        m_pooledParticleSystems = new List<PooledParticleSystem>();
    }
    
    public void SetParticleSystemPrefab(GameObject pooledParticleSystemPrefab)
    {
        particleSystemPrefab = pooledParticleSystemPrefab;
    }

    public void SpawnSystem(Vector3 position)
    {
        PooledParticleSystem pooledParticle;
        if (m_pooledParticleSystems.Count > 0)
        {
            pooledParticle = m_pooledParticleSystems[0];
            pooledParticle.RespawnWithPosition(position);
            m_pooledParticleSystems.Remove(pooledParticle);
        }
        else
        {
            pooledParticle = Instantiate(particleSystemPrefab, position, Quaternion.identity)
                .GetComponent<PooledParticleSystem>();

            pooledParticle.SetPool(this);
            //StartCoroutine(SetPoolAfterSpawn(pooledParticle.GetParticleSystemDuration()/2, pooledParticle));
        }
    }
    
    public void ReturnToPool(PooledParticleSystem pooledParticle)
    {
        m_pooledParticleSystems.Add(pooledParticle);
    }

    IEnumerator SetPoolAfterSpawn(float time, PooledParticleSystem pooledParticle)
    {
        yield return new WaitForSeconds(time);
        pooledParticle.SetPool(this);
    }
}
