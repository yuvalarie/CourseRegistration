using System.Collections.Generic;
using _CA.Core.BaseMono;

namespace _CA.GamePlay.Courses
{
    public class CACourseToUiCatalog : CABaseMono
    {
        private Dictionary<ICAAssignment, CAUICourseItem> courseToUI = new();
        
        public static CACourseToUiCatalog Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Register(ICAAssignment course, CAUICourseItem ui)
        {
            courseToUI[course] = ui;
        }

        public void ResetCourseUI(ICAAssignment course)
        {
            if (courseToUI.TryGetValue(course, out var ui))
            {
                ui.ResetPosition();
            }
        }
    }
}