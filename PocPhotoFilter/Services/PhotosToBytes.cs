using PocPhotoFilter.Contracts.Interfaces;

namespace PocPhotoFilter.Services;

public class PhotosToBytes : IPhotosToBytes
{
    public async Task<List<byte[]>> ConvertToBytes(List<IFormFile> files)
    {

        // não é thread safe, por isso precisa de um lockObject para que só uma thread adicione um arquivo à lista por vez
        var byteFiles = new List<byte[]>();
        var lockObject = new object();

        await Task.Run(() =>
        {

            Parallel.ForEach(files, file =>
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var fileBytes = stream.ToArray();
                    lock (lockObject)
                    {
                        byteFiles.Add(fileBytes);
                    }
                }
            });

        });

        return byteFiles;
    }
}
