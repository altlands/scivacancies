namespace SciVacancies.WebApp.ViewModels
{
    /// <summary>
    /// �������� � ���������������� �����������
    /// </summary>
    public class MembershipDetailsViewModel : MembershipEditViewModel
    {
    }

    /// <summary>
    /// �������� � ���������������� �����������
    /// </summary>
    public class MembershipEditViewModel
    {
        public string org { get; set; }
        public string position { get; set; }
        //public DateTime updated { get; set; }
        public int year_from { get; set; }
        public int year_to { get; set; }
    }
}