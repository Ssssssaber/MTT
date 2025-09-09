namespace DI
{
    public static class UIDGenerator
    {
        private static ulong _freeID = 0;

        public static ulong GetID()
        {
            return _freeID++;
        }
    };
}
