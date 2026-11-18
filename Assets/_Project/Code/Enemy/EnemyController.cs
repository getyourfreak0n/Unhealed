
using System.Collections;
using _Project.Code.Audio;
using _Project.Code.Gameplay.Combat;
using _Project.Code.ScriptableObjects.EnemySO.Stats;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Code.Enemy
{
    public class EnemyController : MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private EnemySO enemySo;
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] GameObject player;

        NavMeshAgent _agent;
        CinemachineImpulseSource _impulse;
        SkinnedMeshRenderer _renderer;
        Coroutine _flickerCoroutine;
        Coroutine _shootCoroutine;
        
        AudioClip _damageClip;
        float _health;
        float _moveSpeed;
        bool _hasEffect;
        public float Damage { get; set; }
        
        bool _isSeen;

        void Awake()
        {
            _renderer = GetComponent<SkinnedMeshRenderer>();
            _agent = GetComponent<NavMeshAgent>();
            _impulse = GetComponent<CinemachineImpulseSource>();
            _agent.updateRotation = false;
        }

        void Start()
        {
            StatsInitializer();
            RandomizeStats();
        }

        void Update()
        {
            if (player == null) return;

            if (!_isSeen)
            {
                NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit,1f, NavMesh.AllAreas);
                if ((hit.mask & _agent.areaMask) != 0)
                {
                    _isSeen = true;
                    _agent.areaMask = -1;
                }
            }

            if (_isSeen)
            {
                ChasePlayer();
                EnemyRotator();
                ShootAtPlayer();
            }
        }

        void ChasePlayer()
        {
                _agent.SetDestination(player.transform.position);  
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            FlickerEffect();
            DamageSFX();
            Debug.Log($"Ouch! Took {damage} damage!");

            if (_health <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void CameraImpulse()
        {
            _impulse.GenerateImpulse();
        }

        public void FlickerEffect()
        {
            if (_flickerCoroutine == null)
            {
                _flickerCoroutine = StartCoroutine(Flicker());
            }
        }

        public void DamageSFX()
        {
            AudioManager.Instance.PlaySound(_damageClip, 0.5f, 0.3f, 0.5f);
        }

        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection)
        {
            GameObject instantiate =Instantiate(vfxPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(instantiate, 1.5f);
        }

        void ShootAtPlayer()
        {
            if (_shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(Shoot());
            }
        }

        IEnumerator Flicker()
        {
            if (_renderer == null) yield break;
            
            WaitForSeconds wait = new WaitForSeconds(0.2f);

            _renderer.enabled = false;
            yield return wait;
            _renderer.enabled = true;
            yield return wait;
            _renderer.enabled = false;
            yield return wait;
            _renderer.enabled = true;
            yield return wait;

            _flickerCoroutine = null;
        }

        IEnumerator Shoot()
        {
            WaitForSeconds wait = new WaitForSeconds(Random.Range(2.5f, 4f));
            
            Quaternion spawnRotation = transform.localRotation * Quaternion.Euler(Random.Range(-1f, 1f), 0f, 0f);
            Instantiate(bulletPrefab, firePoint.position, spawnRotation);
            
            yield return wait;
            
            _shootCoroutine  = null;
        }

        void EnemyRotator()
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);    
        }
        
        void RandomizeStats()
        {
            _agent.angularSpeed = Random.Range(200f, 350f);
            _agent.stoppingDistance = Random.Range(3f,15f);
            _agent.radius = Random.Range(0.6f, 3.5f);
            _agent.avoidancePriority = Random.Range(0, 20);
        }

        void StatsInitializer()
        {
            _health = enemySo.Health;
            _moveSpeed = enemySo.MoveSpeed;
            _damageClip = enemySo.DamageClip;
            
            _agent.speed = _moveSpeed;
        }
    }
}
