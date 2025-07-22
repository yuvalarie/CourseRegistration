using _CA.Core.BaseMono;
using _CA.GamePlay;

namespace _CA.Core.Managers
{
    public class CAServiceLocator : CABaseMono
    {
        public static CAServiceLocator Instance { get; private set; }
        public CADamageTracker DamageTracker { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DamageTracker = new CADamageTracker();
        }
    }
}