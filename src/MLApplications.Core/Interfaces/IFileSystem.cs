using System.IO;
using System.Threading.Tasks;

namespace MLApplications.Core.Interfaces
{
    public interface IFileSystem
    {
        Task<bool> SavePicture(string pictureName, Stream stream);
    }
}
