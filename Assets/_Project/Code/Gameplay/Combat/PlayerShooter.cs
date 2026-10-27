using System.Collections;
using _Project.Code.Audio;
using _Project.Code.Enemy;
using _Project.Code.ScriptableObjects;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Code.Gameplay.Combat
{
    public class PlayerShooter : MonoBehaviour
    {
        //serialized fields
        [SerializeField] Transform crosshair;
        [SerializeField] WeaponsSO[] _weaponsSO;
        [SerializeField] private SpriteRenderer _gunRenderer;
        
        //private fields
        LayerMask _enemyLayer;
        Animator _animator;
        CinemachinePanTilt _tiltAxis;
        float _lastFireTime;
        
        //Weapon attributes. Comes from current WeaponSO
        float _recoilAmount;
        float _recoilSpeed;
        int _currentWeapon;
        float _targetTilt;
        float _recoveryTilt;
        float _spreadAmount;
        float _fireDistance;
        
        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            _animator = GetComponentInChildren<Animator>();
            _tiltAxis = GetComponentInChildren<CinemachinePanTilt>();
        
            WeaponInit();
        }

        void Update()
        {
            WeaponSwitcher();    
        }

        void WeaponSwitcher()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _currentWeapon = 0;
                _lastFireTime = 0f;
                _gunRenderer.transform.localScale = _weaponsSO[_currentWeapon].spriteScale;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
                _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
                _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
                _spreadAmount = _weaponsSO[_currentWeapon].spreadAmount;
                _fireDistance = _weaponsSO[_currentWeapon].fireDistance;   
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _currentWeapon = 1;
                _lastFireTime = 0f;
                _gunRenderer.transform.localScale = _weaponsSO[_currentWeapon].spriteScale;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
                _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
                _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
                _spreadAmount = _weaponsSO[_currentWeapon].spreadAmount;
                _fireDistance = _weaponsSO[_currentWeapon].fireDistance;   
            }
        }
        
        public void Shoot()
        {
            if (RateOfFire()) return;

            StartCoroutine(Recoil());
            _animator.SetTrigger("isShooting");
            AudioManager.Instance.PlaySound(_weaponsSO[_currentWeapon].clip);
            
            Ray ray = new Ray(crosshair.position, crosshair.forward);
            
            if (Physics.SphereCast(ray, _spreadAmount, out RaycastHit hit, _fireDistance, _enemyLayer))
            { 
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                    float damage = _weaponsSO[_currentWeapon].damage;
                    
                    switch (hit.distance)
                    {
                        case <= 5f:
                            break;  
                        case <= 12f:
                            damage *= 0.8f;
                            break;
                        default:
                            damage *= 0.5f;
                            break;
                    }
                   
                    enemy.TakeDamage(damage);
                    enemy.HitVFX(hit.point, hit.normal);
                }
            }
        }

        IEnumerator Recoil()
        {
            float remainingRecoil = _recoilAmount;

            while (remainingRecoil > 0.01f)
            {
                float step = Mathf.Min(_recoilSpeed, remainingRecoil);
                remainingRecoil -= step;

                _tiltAxis.TiltAxis.Value = _tiltAxis.TiltAxis.ClampValue
                    (_tiltAxis.TiltAxis.Value - step);

                yield return null;
            }

            float remainingRecovery = _recoilAmount * 0.3f;

            while (remainingRecovery > 0.01f)
            {
                float step = Mathf.Min(_recoilSpeed * 0.3f, remainingRecovery);
                remainingRecovery -= step;

                _tiltAxis.TiltAxis.Value = _tiltAxis.TiltAxis.ClampValue
                    (_tiltAxis.TiltAxis.Value + step);

                yield return null;
            }
        }

        bool RateOfFire()
        {
            if (Time.time - _lastFireTime <= _weaponsSO[_currentWeapon].nextFireTime)
            {
                return true;
            }

            _lastFireTime = Time.time;
            return false;
        }

        void WeaponInit()
        {
            _currentWeapon = 0;
            _gunRenderer.transform.localScale = _weaponsSO[_currentWeapon].spriteScale;
            _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
            _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
            _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
            _spreadAmount = _weaponsSO[_currentWeapon].spreadAmount;
            _fireDistance = _weaponsSO[_currentWeapon].fireDistance;   
        }
    }
}
