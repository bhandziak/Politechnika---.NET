using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;

namespace NBomberTests
{
    internal class Program
    {
        static void Main()
        {
            var httpClient = new HttpClient();
            string token = "CfDJ8MOCGZzOmwNBriIl71yQaC3_D4snmMaoEKZhnaGWvxx4bzscK_vJay_BYgLcXFDmTORSdai6g-j9jRGfufKziZfOaT6wWro39Z4dfsNls6d2GQgMSPwKbkySKkYxqkifVY5TQteLu7azyy62hppzmjYW_GLa2hFRe1ml9oyfQJwlTalYtSSWdwqeohJwmSSlh3Li5nnh1L6zU0O-dlp4TzX2NKmIrIYsegqiVXP53e7OaNBpQ5gJ-J9WxX7AUUqTA0YKOmoue6398gCP1oz3skGTINryIGHvryCf64CkHK-NWuLF9sJt0Ddmn9Lt-fk7sqHFnZ7kczeenP4SQao0INgk1T3mo8kHk_MLZre2mbQRC2HIbTgPWSWPmOnrtGQusBWUsiMtkqVuD9FV9QksM7c8S53zC4kc-0fgtVq0fWyxDD0IRrES-GEFAESsjXGaMlfXcM68pwX2vgjda_U4pBtIhe1re4xaDp5xcLxXzfqqw-v_7XM1iHfJgBuYcj0clNjjxL0KWfih8tI6cXtrLdQLbnvLT5SEvQ3-xMW6kDHM7G-N67OQXSprNtvUGiN9ptbze_Pj7O9i0IZXNvMLfwLLAouhTFTY89-XYrmqltiVZSu1PVTx2R3WYs73-1lmWLapaca-keMBzE0LkNLhEQBbZWYrt_Dpwsd6C0SObYZiYykMGG9HKOsYWkNLOyEdnHHLXe7ENzEHG-jc-7XE6p5tbSdmN2Ff6G78galXXHKpEWBLqh57559RHoJUTgMSiVlSelJCggJfZjAJalG51dhfxr7PUDoFql3xzZTIVa6vWVwDXWJSkMa_mRVm9ItfGA";

            //var scenario = Scenario.Create("serviceOrder_getAll_api_test", async context =>
            var scenario = Scenario.Create("serviceOrder_part_getAll_api_test", async context =>
            {
                //var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7018/api/serviceOrder/getAll");
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7018/api/part/getAll");
                // dodaj nagłówki ręcznie
                requestMessage.Headers.Add("Cookie", $".AspNetCore.Identity.Application={token}");
                requestMessage.Headers.Add("Accept", "application/json");

                var response = await Http.Send(httpClient, requestMessage);
                return response;
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))//copies - l. użytk.     during - czas trwania (s)
            .WithLoadSimulations(Simulation.KeepConstant(copies: 50, during: TimeSpan.FromSeconds(30)));

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}
