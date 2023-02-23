namespace Games.GrumpyBear.Core.SaveSystem
{
    public interface ISaveableComponent
    {
        object CaptureState();
        void RestoreState(object state);
    }
}
