using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestructibleFire : MonoBehaviour, IHealthAndDamage
{

    [SerializeField] private GameObject fireExplosionEffect;
    [SerializeField] private GameObjectList foodList;
    [SerializeField] private GameObject fireEffect;
    private MeshRenderer m_meshRenderer;
    private Collider m_collider;
    private bool m_isDestroyed = false;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_collider = GetComponent<Collider>();
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        if(!m_isDestroyed)
        {
            m_isDestroyed = true;
            m_collider.enabled = false;
            Instantiate(fireExplosionEffect, transform.position, Quaternion.identity);
            m_meshRenderer.enabled = false;
            fireEffect.SetActive(false);
            GameObject food = Instantiate(foodList.gameObjects[Random.Range(0, foodList.gameObjects.Count)],
                transform.position, Quaternion.identity);
            Vector3 endValue = transform.position + hitDirection * Random.Range(6f, 12f);
            endValue.y = transform.position.y;
            StartCoroutine(MoveThenDestroy(endValue, food));
        }
    }

    public void Heal(float healAmount)
    {
        //Does not heal
    }
    
    IEnumerator MoveThenDestroy(Vector3 endValue, GameObject food)
    {
        float duration = 0.4f;
        food.transform.DOMove(endValue, duration).SetEase(Ease.OutBack);
        yield return new WaitForSecondsRealtime(duration);
        Destroy(gameObject);
    }
}
