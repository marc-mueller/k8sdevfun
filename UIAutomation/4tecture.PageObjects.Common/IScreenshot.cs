using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4tecture.PageObjects.Common
{
    public interface IScreenshot
    {
        Task SaveAsFile(string fileName);
        Task<byte[]> AsByteArray();
    }
}
