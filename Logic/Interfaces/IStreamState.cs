using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Logic.DataStructures;

namespace Logic.Interfaces
{
    public interface IStreamState
    {
        void UpdateState(Func<ImmutableList<StreamItem>, Func<Func<StreamItem, bool>, StreamItem>, ImmutableList<StreamItem>> modify);

        IEnumerable<StreamItem> Resolve();
    }
}