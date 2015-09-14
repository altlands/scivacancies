namespace SciVacancies.Domain.Core
{
    public class ResearchDirection
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string TitleEng { get; set; }

        public int? Lft { get; set; }

        public int? Rgt { get; set; }

        public int? Lvl { get; set; }

        public string OecdCode { get; set; }

        public string WosCode { get; set; }

        public int? RootId { get; set; }
    }
}
