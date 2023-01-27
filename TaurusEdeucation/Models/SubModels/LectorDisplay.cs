using System.Collections.Generic;
using TaurusEdeucation.Models.SubModels;

namespace TaurusEdeucation.Models
{
    public class LectorViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Kraj { get; set; }
        public string Okresy { get; set; }
        public List<LevelViewModel> Levels { get; set; }
    }
}