using System;
using System.Threading.Tasks;
using StreamRipper.Interfaces;

namespace Logic.Interfaces;

public interface IStreamRipperProxy
{
    IStreamRipper Proxy(Uri uri);

    Task<bool> CheckUrlValidAsync(Uri uri);
}