namespace LeoAR.Core
{
    public interface IState
    {
        void Begin();
        void End();
        void Update();
    }
}