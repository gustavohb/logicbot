using System;

public abstract class BaseCommand 
{
    public Action onFinished;
    public float duration = 1f;
    public abstract void Execute();
}
