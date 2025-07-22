namespace _CA.GamePlay
{
    public class CAResetLastError : ICAError
    {
        public void Execute(CADamageTracker tracker)
        {
            tracker.RemoveLastDamage();
        }
    }
}