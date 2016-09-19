namespace LK.Util
{
    public enum LkMessageBoxButtons
    {
        /// <summary>
        /// Window Message -> [確定(OK)]
        /// </summary>
        OK,
        /// <summary>
        /// Window Message -> [確定(OK)][取消(Cancel)]
        /// </summary>
        OKCancel,
        /// <summary>
        /// Error Message -> [中止(Abort)][重試(Retry)][忽略(Ignore)]
        /// </summary>
        AbortRetryIgnore,
        /// <summary>
        /// Dialog Window -> [是(Yes)][否(No)][取消(Cancel)]
        /// </summary>
        YesNoCancel,
        /// <summary>
        /// Dialog Window -> [是(Yes)][否(No)]
        /// </summary>
        YesNo,
        /// <summary>
        /// Warning Message -> [重試(Retry)][取消(Cancel)]
        /// </summary>
        RetryCancel
    }
}
