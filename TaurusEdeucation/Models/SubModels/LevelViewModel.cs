using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaurusEdeucation.Models.SubModels
{
    public class LevelViewModel
    {
        public string Name { get; set; }
        public List<LessonViewModel> Lessons { get; set; }
    }
}