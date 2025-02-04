using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using Photon.Pun;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviourPun
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		//public bool aim;
		public bool shoot;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{

				MoveInput(value.Get<Vector2>());

			
		}

		public void OnLook(InputValue value)
		{

				if (cursorInputForLook)
				{
					LookInput(value.Get<Vector2>());

				
			}
	
		}

		public void OnJump(InputValue value)
		{
				JumpInput(value.isPressed);
			
		}

		public void OnSprint(InputValue value)
		{

				SprintInput(value.isPressed);
			

		}

		//public void OnShoot(InputValue value)
		//{

		//		pw.RPC("ShootInputRPC", RpcTarget.Others, value.isPressed);
			
		//}
#endif
		private void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void MoveInputRPC(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}
		public void JumpInputRPC(bool newJumpState)
		{
			jump = newJumpState;
		}
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInputRPC(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}
		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}
		public void SprintInputRPC(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		//public void AimInput(bool newAimState)
		//{
		//	aim = newAimState;
		//}

		//public void ShootInput(bool newShootState)
  //      {
		//	shoot = newShootState;
  //      }

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}