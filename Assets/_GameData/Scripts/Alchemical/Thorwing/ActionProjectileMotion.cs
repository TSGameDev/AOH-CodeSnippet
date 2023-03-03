using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSGameDev
{
    public class ActionProjectileMotion : MonoBehaviour
    {
        public void Setup(Vector3 _IndicatorPos, float _LaunchAngle)
        {
            //Rotate prefab to be looking in the correct direction
            Vector3 _ProjectileXZPos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 _IndicatorXZPos = new Vector3(_IndicatorPos.x, 0f, _IndicatorPos.z);
            transform.LookAt(_IndicatorXZPos);

            //Shorthands for formula
            float _R = Vector3.Distance(_ProjectileXZPos, _IndicatorXZPos);
            float _G = Physics.gravity.y;
            float _TanAlpha = Mathf.Tan(_LaunchAngle * Mathf.Deg2Rad);
            float _H = _IndicatorPos.y - transform.position.y;

            //Calculate the velocities
            float _Vz = Mathf.Sqrt(_G * (_R * _R) / (2f * (_H - _R * _TanAlpha)));
            float _Vy = _TanAlpha * _Vz;

            //Create velocity vector in local space and convert it to global
            Vector3 _LocalVelocity = new Vector3(0f, _Vy, _Vz);
            Vector3 _GlobalVelocity = transform.TransformDirection(_LocalVelocity);

            //get the rigidbody componant of the
            Rigidbody _ThrownObjRB = GetComponent<Rigidbody>();

            //apply projetile motion force
            _ThrownObjRB.velocity = _GlobalVelocity;
        }
    }
}
