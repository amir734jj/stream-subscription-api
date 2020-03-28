using System.Collections.Immutable;

namespace Models.ViewModels.Config
{
    public class GlobalConfigViewModel
    {
        public ImmutableHashSet<int> StartedStreams { get; set; }

        public GlobalConfigViewModel()
        {
            StartedStreams = ImmutableHashSet<int>.Empty;;
        }

        public GlobalConfigViewModel(GlobalConfigViewModel globalConfigViewModel) : this()
        {
            StartedStreams = globalConfigViewModel.StartedStreams;
        }
    }
}