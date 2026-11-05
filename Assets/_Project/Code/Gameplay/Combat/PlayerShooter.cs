using System.Collections;
using _Project.Code.Audio;
using _Project.Code.Gameplay.Combat.WeaponStates;
using _Project.Code.Gameplay.StatePattern;
using _Project.Code.ScriptableObjects;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Code.Gameplay.Combat
{
    public class PlayerShooter : MonoBehaviour
    {
        //serialized fields
        [field:SerializeField] public Transform crosshair {get; private set;}
        [field:SerializeField] public WeaponsSO[] _weaponsSO { get; set; }
        [field:SerializeField] public SpriteRenderer gunRenderer { get; set;}
        [SerializeField] private LayerMask layersToHit; 
        
        //private fields                                                      2
        public LayerMask EnemyLayer { get; private set; }
        public Animator Animator { get; private set; }
        CinemachinePanTilt _tiltAxis;

        //Weapon attributes. Comes from current WeaponSO
        public float LastFireTime { get; set;}
        public float RecoilAmount {get; set;}
        public float RecoilSpeed {get; set;}
        public int CurrentWeapon  {get; set;}
        public float TargetTilt {get; set;}
        public float RecoveryTilt {get; set;}
        public float SpreadAmount  {get; set;}
        public float FireDistance {get; set;}
        
        //states
        StateMachine _stateMachine;
        PistolState _pistolState;
        ShotgunState _shotgunState;
        IState _defaultState;
        
        void Awake()
        {
            _stateMachine = new StateMachine();
            _pistolState = new PistolState(this);
            _shotgunState = new ShotgunState(this);
            _defaultState = _pistolState;
            
            EnemyLayer = LayerMask.GetMask("Enemy");
            Animator = GetComponentInChildren<Animator>();
            _tiltAxis = GetComponentInChildren<CinemachinePanTilt>();
        }

        void Start()
        {
            _stateMachine.InitializeState(_defaultState);
        }

        void Update()
        {
            WeaponSwitcher();
            _stateMachine._currentState.FrameUpdate();
        }

        void WeaponSwitcher()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _stateMachine.ChangeState(_pistolState);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _stateMachine.ChangeState(_shotgunState);
            }
        }
        
        public void Shoot()
        {
            if (RateOfFire()) return;

            if (_stateMachine._currentState is PistolState pistol)
            {
                pistol.DamageHitscan();
                Debug.Log("pistol sikiyo lan!");
            }
            else if (_stateMachine._currentState is ShotgunState shotgun)
            {
                shotgun.DamageHitscan();
                Debug.Log("shotgun sikiyo lan!");
            }
            
            EffectsHitscan();
            StartCoroutine(Recoil());
            
            Animator.SetTrigger("isShooting");
            AudioManager.Instance.PlaySound(_weaponsSO[CurrentWeapon].clip, 0.6f, 0.85f, 1.15f );
        }
        

        void EffectsHitscan()
        {
            Ray ray = new Ray(crosshair.position, crosshair.forward);

            if (Physics.Raycast(ray,  out var hit, 100f, layersToHit))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.HitVFX(hit.point, hit.normal);
                }   
            }
        }
        
        IEnumerator Recoil()
        {
            float remainingRecoil = RecoilAmount;

            while (remainingRecoil > 0.01f)
            {
                float step = Mathf.Min(RecoilSpeed, remainingRecoil);
                remainingRecoil -= step;

                _tiltAxis.TiltAxis.Value = _tiltAxis.TiltAxis.ClampValue
                    (_tiltAxis.TiltAxis.Value - step);

                yield return null;
            }

            float remainingRecovery = RecoilAmount * 0.3f;

            while (remainingRecovery > 0.01f)
            {
                float step = Mathf.Min(RecoilSpeed * 0.3f, remainingRecovery);
                remainingRecovery -= step;

                _tiltAxis.TiltAxis.Value = _tiltAxis.TiltAxis.ClampValue
                    (_tiltAxis.TiltAxis.Value + step);

                yield return null;
            }
        }

        bool RateOfFire()
        {
            if (Time.time - LastFireTime <= _weaponsSO[CurrentWeapon].nextFireTime)
            {
                return true;
            }

            LastFireTime = Time.time;
            return false;
        }
    }
}
