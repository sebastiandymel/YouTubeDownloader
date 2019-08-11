using System;

namespace YTDownloader

{
    public interface IRemovable
    {
        event EventHandler RemoveRequested;
    }
}
