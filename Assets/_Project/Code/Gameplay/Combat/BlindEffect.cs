using UnityEngine;
using System.Collections;

namespace _Project.Code.Gameplay.Combat
{
    public class BlindEffect : MonoBehaviour
    {       
        [SerializeField] GameObject prefab;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float sphereRadius = 1;
        [SerializeField] float blindDuration = 5f;

        Coroutine _blinded;

        void OnEnable()
        {
            EventManager.Instance.Blinded += OnBlinded;
        }

        void OnDisable()
        {
            EventManager.Instance.Blinded -= OnBlinded;
        }


        void OnBlinded()
        {
            _blinded ??= StartCoroutine(BubbleSpawner());
        }
        
        IEnumerator BubbleSpawner() 
        {
                WaitForSeconds wait = new WaitForSeconds(0.45f);

                float shotTime = Time.time;
                
                while (shotTime <= Time.time + blindDuration)
                {
                    Vector3 spawnPos = Random.insideUnitSphere * sphereRadius + spawnPoint.position;
                    spawnPos.z = spawnPoint.position.z + Random.Range(0.5f, 0.8f);
                    var spawnedObj= Instantiate(prefab, spawnPos, Quaternion.identity, transform);
                
                    yield return wait;
                
                    Destroy(spawnedObj, 2.7f); 
                }
                
                _blinded = null;
        }
    }
}
