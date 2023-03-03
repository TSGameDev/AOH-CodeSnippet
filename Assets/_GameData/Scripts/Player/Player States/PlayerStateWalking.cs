using UnityEngine;
using TSGameDev.Controls.MainPlayer;

namespace TSGameDev.Controls.PlayerStates
{
    public class PlayerStateWalking : PlayerState
    {
        public PlayerStateWalking(Player player) : base(player) { }

        public override void Update()
        {
            CheckDestination();

            if (anim.GetFloat(player.animSpeed) < 1)
                anim.SetFloat(player.animSpeed, 1, 0.35f, 0.1f);

            if (anim.GetFloat(player.animSpeed) > 1)
                anim.SetFloat(player.animSpeed, 1, 1, 0.1f);
        }

        public override void MoveTo()
        {
            base.MoveTo();

            if (player.isRunning)
                StateTransition(PlayerStates.Running, PlayerStates.Walking);
        }

        private void CheckDestination()
        {
            float remainingDis = Vector3.Distance(player.transform.position, agent.destination);
            if(remainingDis <= 0.5f)
            {
                StateTransition(PlayerStates.Idle, PlayerStates.Walking);

            }
        }
    }
}
