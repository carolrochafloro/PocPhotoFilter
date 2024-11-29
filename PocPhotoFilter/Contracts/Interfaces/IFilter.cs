using System.Drawing;

namespace PocPhotoFilter.Contracts.Interfaces;

public interface IFilter
{
    Image ApplyFilter(Image original);
}
