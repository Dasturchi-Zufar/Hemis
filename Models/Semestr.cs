public static class UserState
{
    private static Dictionary<long, string> semesters = new();

    public static void SetSemester(long chatId, string semester)
    {
        semesters[chatId] = semester;
    }

    public static int GetSemester(long chatId)
    {
            string semester = semesters[chatId];
            return int.Parse(semester[0].ToString());
        
    }
}
