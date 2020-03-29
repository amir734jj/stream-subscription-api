namespace Logic.Interfaces
{
    public interface IFilterSongLogic
    {
        bool ShouldInclude(string filename, string pattern);
    }
}