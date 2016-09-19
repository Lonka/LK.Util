namespace LK.Util
{
    internal class FileLogFactory : ILogFactory
    {
        public LogProvide CreatorProvide()
        {
            LogProvide logProvide = new CommonFileLog();
            switch (LkLog.LogParams.LogImplType)
            {
                case LogImplType.FileDelta:
                    logProvide = new DeltaFileLog();
                    break;
                case LogImplType.FileCommon:
                    break;
            }
            //LogProvide logProvide = new ThreadFileLog();
            return logProvide;
        }
    }
}