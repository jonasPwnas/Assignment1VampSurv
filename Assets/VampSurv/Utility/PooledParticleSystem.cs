using System.Collections;
using UnityEngine;

public class PooledParticleSystem : MonoBehaviour
{
    private ParticleSystem m_system;
    private ParticleSystemPool m_pool;

    private void Start()
    {
        m_system = GetComponent<ParticleSystem>();
        StartCoroutine(DisableAfterDuration(m_system.main.duration));
    }

    public void SetPool(ParticleSystemPool pool)
    {
        m_pool = pool;
    }

    public float GetParticleSystemDuration()
    {
        return m_system.main.duration;
    }
    
    public void RespawnWithPosition(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(true);
        m_system.Play();
        StartCoroutine(DisableAfterDuration(m_system.main.duration));
    }

    IEnumerator DisableAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_pool.ReturnToPool(this);
        gameObject.SetActive(false);
    }
}
