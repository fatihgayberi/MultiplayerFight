public static class WonnaTimeDatasExtensionMethods
{
    public static float WonnaTimeDatas2TotalSecond(this WonnaTimeDatas wonnaTimeDatas)
    {
        if (wonnaTimeDatas == null)
        {
            return 0;
        }

        return wonnaTimeDatas.minute * 60 + wonnaTimeDatas.seconds;
    }
}