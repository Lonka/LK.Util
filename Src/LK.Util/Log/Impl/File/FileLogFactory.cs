namespace LK.Util
{
    internal class FileLogFactory : ILogFactory
    {
        public LogProvide CreatorProvide(LkLogParams param )
        {
            LogProvide logProvide = new CommonFileLog();
            switch (param.LogImplType)
            {
                case LogImplType.FileDelta:
                    logProvide = new DeltaFileLog();
                    break;
                case LogImplType.FileCommon:
                    break;
            }
            logProvide.LogParams = param;
            //LogProvide logProvide = new ThreadFileLog();
            return logProvide;
        }
    }
}