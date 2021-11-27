using System;
using ScriptableObjectArchitecture;
using UnityEngine;

[Serializable]
public abstract class BaseCommandSO : ScriptableObject
{
    [Header("Settings")]
    public bool isCommandList = false;
    public bool isBreakCommand = false;
    
    [HideInInspector]
    public ProgramListCommandSO parentListCommand;
    
    public BoolVariable stopped;
   
    [Header("Color System")]
    public ColorVariable commandColor; // Used for conditional statements
    public ColorVariable currentPlayerColor;
    public ColorVariable defaultPlayerColor;
    
    public ColorVariableGameEvent setCurrentPlayerColorGameEvent;
    
    [Header("Runtime Set References")]
    public PlayerControllerRuntimeSet playerControllerRuntimeSet;
    
    protected CommandUI _commandUI;

    protected void OnEnable()
    {
        setCurrentPlayerColorGameEvent.AddListener(SetCurrentPlayerColor);
    }

    public virtual void SetCommandUI(CommandUI commandUI)
    {
        _commandUI = commandUI;
    }

    protected void SetCurrentPlayerColor(ColorVariable playerColor)
    {
        currentPlayerColor = playerColor;
    }

    protected void OnDisable()
    {
        setCurrentPlayerColorGameEvent.RemoveListener(SetCurrentPlayerColor);
    }

    public abstract void Execute(Action callback);
}
