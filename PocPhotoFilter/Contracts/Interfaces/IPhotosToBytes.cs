namespace PocPhotoFilter.Contracts.Interfaces;

public interface IPhotosToBytes
{
    Task<List<byte[]>> ConvertToBytes(List<IFormFile> files);
}
