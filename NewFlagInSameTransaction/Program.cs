using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        string conn = @"********";//填入自己的連線字串
        try
        {
            using (var cn = new OracleConnection(conn))
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    string sql = @"UPDATE CALENDAR SET HOLIDAY_FLAG = 'Y' WHERE date_no = '20230515'";//找一筆原本為N更新為Y但不Commit
                    cn.Query(sql, transaction);
                    string validate = @"SELECT holiday_flag from CALENDAR WHERE date_no = '20230515'";//找出剛剛那筆
                    string reslut = cn.Query<string>(validate).ToList().FirstOrDefault();
                    Console.WriteLine($"還沒Commit 原本結果為N 更新旗標後的結果為:{reslut}");
                }
            }
        }
        catch (Exception ex)
        { 
            Console.WriteLine(ex.Message);
        }
    }
}