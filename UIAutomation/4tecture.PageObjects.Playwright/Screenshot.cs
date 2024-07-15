using _4tecture.PageObjects.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Playwright
{
    public class Screenshot : IScreenshot
    {
        private readonly byte[] _screenshotData;

        public Screenshot(byte[] screenshotData)
        {
            _screenshotData = screenshotData;
        }

        public Task SaveAsFile(string fileName)
        {
            return System.IO.File.WriteAllBytesAsync(fileName, _screenshotData);
        }

        public Task<byte[]> AsByteArray()
        {
            return Task.FromResult(_screenshotData);
        }
    }
}
