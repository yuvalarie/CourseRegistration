namespace _CA.GamePlay
{
    public class CAResetAllError : ICAError
    {
        public void Execute(CADamageTracker tracker)
        {
            tracker.ResetAllDamage();
        }
        
    }
}