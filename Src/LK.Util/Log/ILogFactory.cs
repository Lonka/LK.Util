namespace LK.Util
{
    internal interface ILogFactory
    {
        LogProvide CreatorProvide(LkLogParams param);
    }
}