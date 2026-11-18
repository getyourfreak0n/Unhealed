
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
        [SerializeField] private GameObject normalPrefab;
        [SerializeField] private GameObject blindingPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform[] FirePoints;

        NavMeshAgent _agent;
        CinemachineImpulseSource _impulse;
        SkinnedMeshRenderer _renderer;
        Animator _animator;
        Coroutine _flickerCoroutine;
        Coroutine _normalAttack;
        Coroutine _blindingAttack;
        
        AudioClip _damageClip;
        EnemyType _type;
        float _health;
        float _moveSpeed;
        bool _hasEffect;
        BoxCollider _collider;
        RuntimeAnimatorController _controller;
        public float Damage { get; set; }
        
        bool _isSeen;
        bool _isDead;

        void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _renderer = GetComponent<SkinnedMeshRenderer>();
            _agent = GetComponent<NavMeshAgent>();
            _impulse = GetComponent<CinemachineImpulseSource>();
            _animator = GetComponent<Animator>();
            
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

            if (_isSeen && !_isDead)
            {
                ChasePlayer();
                EnemyRotator();
                AttackThePlayer();
            }
        }

        void ChasePlayer()
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            { 
                _animator.SetBool("isRunning", false);
            }
            else
            {
                _animator.SetBool("isRunning", true);
            }
            
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
                _isDead = true;
                StartCoroutine(OnDeath());
            }
        }

        IEnumerator OnDeath()
        {
            StopCoroutine(Flicker());
            _animator.SetTrigger("onDeath");
            _collider.enabled = false;

            yield return null;
            
            Destroy(gameObject,3f);
        }

        public void CameraImpulse()
        {
            _impulse.GenerateImpulse();
        }

        public void FlickerEffect()
        {
            _flickerCoroutine ??= StartCoroutine(Flicker());
        }

        public void DamageSFX()
        {
            AudioManager.Instance.PlaySound(_damageClip, 0.5f, 0.3f, 0.5f);
        }

        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection)
        {
            GameObject instantiate = Instantiate(vfxPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(instantiate, 1.5f);
        }

        void AttackThePlayer()
        {
            switch (_type)
            {
                case EnemyType.Type1:
                    _normalAttack ??= StartCoroutine(NormalAttack());
                    break;
                case EnemyType.Type2:
                    _blindingAttack ??= StartCoroutine(BlindingAttack());
                    break;
            }
            
        }

        IEnumerator Flicker()
        {
            if (_renderer == null) yield break;
            
            WaitForSeconds wait = new WaitForSeconds(0.15f);

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

        IEnumerator NormalAttack()
        {
            WaitForSeconds wait = new WaitForSeconds(Random.Range(2f, 3.5f));
            
            Quaternion spawnRotation = transform.localRotation * Quaternion.Euler(Random.Range(-0.85f, 0.85f), 0f, 0f);
            Instantiate(normalPrefab, firePoint.position, spawnRotation);
            _animator.SetTrigger("isAttacking");
            
            yield return wait;
            
            _normalAttack  = null;
        }

        void EnemyRotator()
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);    
        }

        IEnumerator BlindingAttack()
        {
            WaitForSeconds wait = new WaitForSeconds(5f);

            int randomIndex = Random.Range(0, FirePoints.Length);
            Transform selectedFirePoint = FirePoints[randomIndex];

            selectedFirePoint.LookAt(player.transform);
            
            //Quaternion randomOffset = Quaternion.Euler(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
            Quaternion finalSpawnRotation = selectedFirePoint.rotation;

            Instantiate(blindingPrefab, selectedFirePoint.position, finalSpawnRotation);

            _animator.SetTrigger("isAttacking");
            
            yield return wait;
            
            _blindingAttack = null;
        }

        void RandomizeStats()
        {
            _agent.angularSpeed = Random.Range(200f, 350f);
            _agent.stoppingDistance = Random.Range(3f,15f);
            _agent.radius = Random.Range(1f, 4f);
            _agent.avoidancePriority = Random.Range(0, 20);
        }

        void StatsInitializer()
        {
            _health = enemySo.Health;
            _moveSpeed = enemySo.MoveSpeed;
            _damageClip = enemySo.DamageClip;
            _type = enemySo.EnemyType;
            _controller = enemySo.Controller;
            
            _agent.speed = _moveSpeed;
            _animator.runtimeAnimatorController = _controller;
        }
    }
}
