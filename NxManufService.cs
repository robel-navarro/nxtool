using System.Diagnostics;

namespace nxtool
{
    public class NxManufService
    {

        private readonly string _appPath;

        public NxManufService(IConfiguration configuration)
        {
            _appPath = configuration["AppSettings:nx_app"];
        }

        public string RunTool(string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _appPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                return $"Error: {error}";
            }

            return string.IsNullOrWhiteSpace(output) ? "Tool executed but returned no output." : output;
        }
    }

}
