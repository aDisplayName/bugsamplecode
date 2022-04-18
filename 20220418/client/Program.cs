// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;

await Test5();


async Task Test5()
{
    // https://stackoverflow.com/a/28242716/3200008
    using var client = new HttpClient()
    {
        Timeout = TimeSpan.FromDays(1)
    };
    client.DefaultRequestHeaders.Add("User-Agent","dotnet-http-client");
    using var content= new MultipartFormDataContent();
    var path = @"C:\bin1234.dat";
    using var sc = new ByteArrayContent(new byte[]{1,2,3,4});
    sc.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
    sc.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
    {
        Name = @"""file""",
        FileName = @$"""{Path.GetFileName(path)}"""
    };
    
    content.Add(sc, "file", Path.GetFileName(path));

    
    var url = "http://localhost:54900/FileUpload/UploadLargeFile";
    using var response = await client.PostAsync(url, content);
    Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");

}

