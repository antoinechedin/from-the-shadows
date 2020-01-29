// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/InputMaster.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class InputMaster : InputActionAssetReference
{
    public InputMaster()
    {
    }
    public InputMaster(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // Player
        m_Player = asset.GetActionMap("Player");
        m_Player_Jump = m_Player.GetAction("Jump");
        m_Player_Interact = m_Player.GetAction("Interact");
        m_Player_Move = m_Player.GetAction("Move");
        m_Player_OpenMenu = m_Player.GetAction("OpenMenu");
        m_Player_Accept = m_Player.GetAction("Accept");
        m_Player_Refuse = m_Player.GetAction("Refuse");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        if (m_PlayerActionsCallbackInterface != null)
        {
            Player.SetCallbacks(null);
        }
        m_Player = null;
        m_Player_Jump = null;
        m_Player_Interact = null;
        m_Player_Move = null;
        m_Player_OpenMenu = null;
        m_Player_Accept = null;
        m_Player_Refuse = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        var PlayerCallbacks = m_PlayerActionsCallbackInterface;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
        Player.SetCallbacks(PlayerCallbacks);
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // Player
    private InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private InputAction m_Player_Jump;
    private InputAction m_Player_Interact;
    private InputAction m_Player_Move;
    private InputAction m_Player_OpenMenu;
    private InputAction m_Player_Accept;
    private InputAction m_Player_Refuse;
    public struct PlayerActions
    {
        private InputMaster m_Wrapper;
        public PlayerActions(InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump { get { return m_Wrapper.m_Player_Jump; } }
        public InputAction @Interact { get { return m_Wrapper.m_Player_Interact; } }
        public InputAction @Move { get { return m_Wrapper.m_Player_Move; } }
        public InputAction @OpenMenu { get { return m_Wrapper.m_Player_OpenMenu; } }
        public InputAction @Accept { get { return m_Wrapper.m_Player_Accept; } }
        public InputAction @Refuse { get { return m_Wrapper.m_Player_Refuse; } }
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Jump.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Interact.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                Move.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                OpenMenu.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMenu;
                OpenMenu.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMenu;
                OpenMenu.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnOpenMenu;
                Accept.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAccept;
                Accept.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAccept;
                Accept.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAccept;
                Refuse.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRefuse;
                Refuse.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRefuse;
                Refuse.cancelled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRefuse;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.cancelled += instance.OnJump;
                Interact.started += instance.OnInteract;
                Interact.performed += instance.OnInteract;
                Interact.cancelled += instance.OnInteract;
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.cancelled += instance.OnMove;
                OpenMenu.started += instance.OnOpenMenu;
                OpenMenu.performed += instance.OnOpenMenu;
                OpenMenu.cancelled += instance.OnOpenMenu;
                Accept.started += instance.OnAccept;
                Accept.performed += instance.OnAccept;
                Accept.cancelled += instance.OnAccept;
                Refuse.started += instance.OnRefuse;
                Refuse.performed += instance.OnRefuse;
                Refuse.cancelled += instance.OnRefuse;
            }
        }
    }
    public PlayerActions @Player
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new PlayerActions(this);
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get

        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.GetControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get

        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
}
public interface IPlayerActions
{
    void OnJump(InputAction.CallbackContext context);
    void OnInteract(InputAction.CallbackContext context);
    void OnMove(InputAction.CallbackContext context);
    void OnOpenMenu(InputAction.CallbackContext context);
    void OnAccept(InputAction.CallbackContext context);
    void OnRefuse(InputAction.CallbackContext context);
}
