public struct FrameStatistics
{
    public ulong frameCount;
    public float fps;

    public FrameStatistics(ulong frameCount = 0, float fps = 0.0f)
    {
        this.frameCount = frameCount;
        this.fps = fps;
    }
}