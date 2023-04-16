using System;
using StreamRipper.Interfaces;

namespace Logic.Interfaces;

public interface IStreamRipperProxy
{
    IStreamRipper Proxy(Uri uri);
}