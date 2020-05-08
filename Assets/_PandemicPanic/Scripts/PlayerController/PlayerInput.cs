using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class PlayerInput : MonoBehaviour
{
    public int m_playerId;

    private APRController m_ragdollController;
    private Player m_playerInputController;

    public static PlayerInput Instance;
    private void Awake()
    {

        Instance = this;
    }
    private void Start()
    {

        m_ragdollController = GetComponent<APRController>();
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }

    public void ChangeCursorState(bool p_activeState)
    {
        Cursor.visible = !false;

        if (!p_activeState)
        {

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        GetInput();
    }



    public void GetInput()
    {
        #region Movement Input
        if (m_playerInputController.GetButton("MoveForward"))
        {
            m_ragdollController.MoveForward(true);
        }
        if (m_playerInputController.GetButtonUp("MoveForward"))
        {
            m_ragdollController.MoveForward(false);
        }

        if (m_playerInputController.GetButton("MoveBackwards"))
        {
            m_ragdollController.MoveBackwards(true);
        }
        if (m_playerInputController.GetButtonUp("MoveBackwards"))
        {
            m_ragdollController.MoveBackwards(false);
        }

        if (m_playerInputController.GetButton("MoveLeft"))
        {
            m_ragdollController.MoveLeft(true);
        }
        if (m_playerInputController.GetButtonUp("MoveLeft"))
        {
            m_ragdollController.MoveLeft(false);
        }

        if (m_playerInputController.GetButton("MoveRight"))
        {
            m_ragdollController.MoveRight(true);
        }
        if (m_playerInputController.GetButtonUp("MoveRight"))
        {
            m_ragdollController.MoveRight(false);
        }
        #endregion

        #region Jump
        if (m_playerInputController.GetButtonDown("Jump"))
        {
            m_ragdollController.JumpInput(true);
        }
        #endregion

        #region Arm Inputs
        if (m_playerInputController.GetButtonDown("LeftPickup"))
        {
            m_ragdollController.LeftPickupInput(true);
        }
        if (m_playerInputController.GetButtonUp("LeftPickup"))
        {
            m_ragdollController.LeftPickupInput(false);
        }

        if (m_playerInputController.GetButtonDown("RightPickup"))
        {
            m_ragdollController.RightPickupInput(true);
        }
        if (m_playerInputController.GetButtonUp("RightPickup"))
        {
            m_ragdollController.RightPickupInput(false);
        }

        if (m_playerInputController.GetButtonDown("Throw"))
        {
            m_ragdollController.PickupAndThrow(true);
        }

        if (m_playerInputController.GetButtonDown("LeftPunch"))
        {
            m_ragdollController.PunchInput(false, true);
        }
        if (m_playerInputController.GetButtonDown("RightPunch"))
        {
            m_ragdollController.PunchInput(true, false);
        }
        #endregion
    }

}