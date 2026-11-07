using _Project.Code.Enemy;
using _Project.Code.Gameplay.StatePattern;
using UnityEngine;

namespace _Project.Code.Gameplay.Combat.WeaponStates
{
    public class PistolState : IState
    {
        PlayerShooter _player;

        public PistolState(PlayerShooter player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.CurrentWeapon = 0;
            _player.LastFireTime = 0f;
            _player.gunRenderer.transform.localScale = _player._weaponsSO[_player.CurrentWeapon].spriteScale;
            _player.gunRenderer.gameObject.transform.localPosition = _player._weaponsSO[_player.CurrentWeapon].spriteTransform;
            _player.Animator.runtimeAnimatorController = _player._weaponsSO[_player.CurrentWeapon].weaponAnimator;
            _player.RecoilAmount = _player._weaponsSO[_player.CurrentWeapon].recoilAmount;
            _player.RecoilSpeed =  _player._weaponsSO[_player.CurrentWeapon].recoilSpeed;
            _player.SpreadAmount = _player._weaponsSO[_player.CurrentWeapon].spreadAmount;
            _player.FireDistance = _player._weaponsSO[_player.CurrentWeapon].fireDistance;   
        }

        public void FrameUpdate()
        {
        }

        public void Exit()
        {
        }
        
        public void DamageHitscan()
        {
            Ray ray = new Ray(_player.crosshair.position, _player.crosshair.forward);
            
            if (Physics.SphereCast(ray, _player.SpreadAmount, 
                    out RaycastHit hit, _player.FireDistance, _player.EnemyLayer))
            { 
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                    float damage = _player._weaponsSO[_player.CurrentWeapon].damage;
                    
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
                    enemy.CameraImpulse();
                }
            }
        }
    }
}
