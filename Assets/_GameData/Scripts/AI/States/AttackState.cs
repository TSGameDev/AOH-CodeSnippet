using System.Collections;
using System.Collections.Generic;
using System.Net;
using TSGameDev.Controls.MainPlayer;
using TSGameDev.Core.Effects;
using UnityEditor.PackageManager;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TSGameDev.Core.AI
{
    public class AttackState : State
    {
        private GameObject _ChaseTarget;
        private float ATTACK_ORIGIN_OFFSET = 1f;

        public AttackState(AIController _AIController, GameObject _ChaseTarget) : base(_AIController)
        {
            this._ChaseTarget = _ChaseTarget;
        }

        public override void Enter()
        {
            _EntityAnimator.SetFloat(_AIBrain.speedHash, ANIM_IDLE_SPEED);
            _AIController.SetAnimDelegate(Attack);

            Vector3 _LookAtPoint = _ChaseTarget.transform.position;
            _LookAtPoint.y = _Entity.transform.position.y;
            _Entity.transform.LookAt(_LookAtPoint);
            _EntityAnimator.SetTrigger(_AIBrain.attackHash);
        }

        public void Attack()
        {
            //Created mesh for the attack cone
            Mesh _Mesh = new Mesh();
            //start pos of the attack cone, where all the tris connect to.
            Vector3 _AttackOrigin = new Vector3(_Entity.transform.position.x, _Entity.transform.position.y + ATTACK_ORIGIN_OFFSET, _Entity.transform.position.z);
            Vector3 _Origin = _AttackOrigin;
            //The amount of raycasts performed to smooth the cone and create a more defined area of attack.
            int _RayCount = 25;
            //The current angle being processed
            Vector3 _EntityDirection = (_ChaseTarget.transform.position - _Entity.transform.position).normalized;
            float _Angle = SetAimDirection(_EntityDirection);
            //The incriments to increase the angle, divides the area of attack angle by the amount of rays. Higher attack angles require more rays to be precise.
            float _AngleIncriment = _AIStats.attackAngle / _RayCount;
            //The maximum range of all raycasts
            float _MaxAttackRange = _AIStats.attackRange;

            Vector3[] _Verts = new Vector3[_RayCount + 2];
            Vector2[] _UV = new Vector2[_Verts.Length];
            int[] _Tris = new int[_RayCount * 3];

            //Sets the starting vert to the origin position
            _Verts[0] = _Origin;

            RayMeshCreation(_RayCount, _Origin, _Angle, _MaxAttackRange, _Verts, _Tris, _AngleIncriment);

            SetMesh(_Mesh, _Verts, _UV, _Tris);

            CalculateTransition();
        }

        private Vector3 GetVectorFromAngle(float _Angle)
        {
            float _AngleRad = _Angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(_AngleRad), 0, Mathf.Sin(_AngleRad));
        }

        private float GetAnlgeFromVectorFloat(Vector3 _Dir)
        {
            _Dir = _Dir.normalized;
            float N = Mathf.Atan2(_Dir.z, _Dir.x) * Mathf.Rad2Deg;
            if (N < 0) N += 360;
            return N;
        }

        private float SetAimDirection(Vector3 _AimDir)
        {
            float _StartingAngle = GetAnlgeFromVectorFloat(_AimDir) + (_AIStats.attackAngle / 2);
            return _StartingAngle;
        }

        private void SetMesh(Mesh _Mesh, Vector3[] _Verts, Vector2[] _UV, int[] _Tris)
        {
            Indicator _AttackIndicator = _Entity.GetComponentInParent<Indicator>();
            if (_AttackIndicator == null)
                return;

            MeshFilter _AttackMesh = _AttackIndicator.GetMeshFilter();
            if (_AttackMesh == null)
                return;

            _AttackMesh.mesh = _Mesh;
            _Mesh.vertices = _Verts;
            _Mesh.uv = _UV;
            _Mesh.triangles = _Tris;
        }

        private void RayMeshCreation(int _RayCount, Vector3 _Origin, float _Angle, float _MaxAttackRange, Vector3[] _Verts, int[] _Tris, float _AngleIncriment)
        {
            int _VertIndex = 1;
            int _TriIndex = 0;
            for (int i = 0; i <= _RayCount; i++)
            {
                Vector3 _Vertex;
                if (Physics.Raycast(_Origin, GetVectorFromAngle(_Angle), out RaycastHit _RaycastHit, _MaxAttackRange, ~ENTITY_COLLISION_LAYER))
                {
                    //Hit non entity
                    _Vertex = _RaycastHit.point;
                    //Hit Player
                    if(_RaycastHit.collider.CompareTag("Player"))
                    {
                        CalculateDamage(_RaycastHit.collider.gameObject);
                    }
                }
                else
                {
                    //No Hit
                    _Vertex = _Origin + GetVectorFromAngle(_Angle) * _MaxAttackRange;
                }


                _Verts[_VertIndex] = _Vertex;
                if (i > 0)
                {
                    _Tris[_TriIndex + 0] = 0;
                    _Tris[_TriIndex + 1] = _VertIndex - 1;
                    _Tris[_TriIndex + 2] = _VertIndex;
                    _TriIndex += 3;
                }

                _VertIndex++;
                _Angle -= _AngleIncriment;
            }
        }

        private void CalculateTransition()
        {
            //Calculate distance between target and entity
            float _DisToTarget = DistanceCheckWithCollisionOffset(_ChaseTarget.transform.position);

            //If the distance between target and entity is less than attack range
            if (_DisToTarget < _AIStats.attackTriggerRange)
                _StateMachine.CurrentState = new AttackState(_AIController, _ChaseTarget);
            else if(_DisToTarget > _AIStats.attackTriggerRange)
                _StateMachine.CurrentState = new ChaseState(_AIController, _ChaseTarget);
            else if(_DisToTarget > _AIStats.visualRange)
                _StateMachine.CurrentState = new ReturnState(_AIController);
        }

        private void CalculateDamage(GameObject _HitGameObject)
        {
            IAttackable _TargetAttackable = _HitGameObject.GetComponent<IAttackable>();
            if (_TargetAttackable == null)
                return;

            _TargetAttackable.DealDamage(_AIStats.damage);
        }

        public override void Exit() 
        {
            _AIController.SetAnimDelegate(null);
        }
    }
}
