using TSGameDev.Core.Effects;
using UnityEngine;

namespace TSGameDev.Inventories.Actions
{
    public class Throwable : MonoBehaviour
    {
        [Tooltip("Particle System spawned when throwable hits something")]
        [SerializeField] private ParticleSystem splashParticle;
        [Tooltip("Radius of effect of the potion/throwable")]
        [SerializeField] private float splashRadius;

        private IStatusEffect[] _PotionEffects;
        public void SetPotionEffects(IStatusEffect[] _PotionStatusEffects) => _PotionEffects = _PotionStatusEffects;

        private void OnCollisionEnter(Collision collision)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, splashRadius);
            foreach (Collider obj in colliders)
            {
                Debug.Log($"Collision Obj Name: {obj.gameObject.name}");
                //Effect all objects that can be effected in the splash zone
                if (obj.TryGetComponent<IEffectable>(out var interactableObj))
                    interactableObj.Effect(_PotionEffects);
            }
            //Spawn Particle effect
            ParticleSystem _Splash = Instantiate(splashParticle, transform.position, Quaternion.Euler(90,0,0));
            Destroy(_Splash.gameObject, 1.5f);
            //Destory Throwable
            Destroy(gameObject);
        }
    }
}
